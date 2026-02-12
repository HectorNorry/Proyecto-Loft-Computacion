using ClosedXML.Excel;
using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoftComputacion.Application
{
    public class GananciasService
    {
        private readonly ApplicationDbContext _context;

        public GananciasService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrdenDeServicio>> GetGananciasPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            // Ajuste para incluir hasta el último milisegundo del día final
            var fechaHastaAjustada = fechaHasta.Date.AddDays(1).AddTicks(-1);

            return await _context.OrdenesDeServicio
                .Include(o => o.Cliente)
                .Include(o => o.Equipo)
                .Include(o => o.Estado)
                .Where(o => o.FechaIngreso >= fechaDesde.Date && o.FechaIngreso <= fechaHastaAjustada)
                // Filtramos por los IDs reales de tu tabla: 5 = Entregado, 7 = Pagado
                .Where(o => o.EstadoId == 5 || o.EstadoId == 7)
                .OrderByDescending(o => o.FechaIngreso)
                .ToListAsync();
        }

        public async Task<byte[]> GetGananciasExcelAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            var ordenesPagadas = await GetGananciasPorFechaAsync(fechaDesde, fechaHasta);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ganancias");
                var currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = "Fecha Pago";
                worksheet.Cell(currentRow, 2).Value = "Cliente";
                worksheet.Cell(currentRow, 3).Value = "Equipo"; // Agregamos columna Equipo al Excel también
                worksheet.Cell(currentRow, 4).Value = "Monto Cobrado";
                worksheet.Cell(currentRow, 5).Value = "Método Pago";

                worksheet.Row(currentRow).Style.Font.Bold = true;
                worksheet.Row(currentRow).Style.Fill.BackgroundColor = XLColor.LightGray;

                foreach (var orden in ordenesPagadas)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = orden.FechaPago;
                    worksheet.Cell(currentRow, 2).Value = orden.Cliente?.NombreCompleto ?? "N/A";

                    // Usamos la propiedad inteligente de tu dominio
                    string equipoInfo = orden.Equipo != null ? orden.Equipo.DescripcionCompleta : "-";
                    worksheet.Cell(currentRow, 3).Value = equipoInfo;

                    worksheet.Cell(currentRow, 4).Value = orden.PrecioFinal ?? 0;
                    worksheet.Cell(currentRow, 5).Value = orden.MetodoDePago?.Nombre ?? "N/A";
                }

                currentRow++;
                worksheet.Cell(currentRow, 3).Value = "Total Ganancias:";
                worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 4).FormulaA1 = $"=SUM(D2:D{currentRow - 1})"; // Ajustado a columna D

                worksheet.Column(1).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                worksheet.Column(4).Style.NumberFormat.Format = "$ #,##0.00";
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        // --- MÉTODO PDF DEFINITIVO ---
        public async Task<byte[]> GetGananciasPdfAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            var ordenes = await GetGananciasPorFechaAsync(fechaDesde, fechaHasta);
            var totalGanancia = ordenes.Sum(o => o.PrecioFinal ?? 0);
            var cantidad = ordenes.Count();

            QuestPDF.Settings.License = LicenseType.Community;
            QuestPDF.Settings.UseEnvironmentFonts = false;

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1.5f, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Lato"));

                    // --- CABECERA ---
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Loft Computación").Bold().FontSize(22).FontColor(Colors.Blue.Medium);
                            col.Item().Text("Reporte de Ganancias Reales").FontSize(14).SemiBold();
                            col.Item().Text($"Período: {fechaDesde:dd/MM/yyyy} - {fechaHasta:dd/MM/yyyy}").FontColor(Colors.Grey.Medium);
                        });

                        row.ConstantItem(150).Background(Colors.Grey.Lighten5).Padding(10).Column(col =>
                        {
                            col.Item().AlignRight().Text("TOTAL COBRADO").FontSize(8).Bold();
                            col.Item().AlignRight().Text($"{totalGanancia:C}").Bold().FontSize(18).FontColor(Colors.Green.Medium);
                            col.Item().AlignRight().Text($"{cantidad} órdenes").FontSize(9);
                        });
                    });

                    // --- CONTENIDO (TABLA) ---
                    page.Content().PaddingVertical(1, Unit.Centimetre).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(70);  // Fecha
                            columns.RelativeColumn(2);   // Cliente
                            columns.RelativeColumn(2);   // Equipo
                            columns.ConstantColumn(80);  // Estado
                            columns.ConstantColumn(90);  // Monto
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(EstiloHeader).Text("Fecha");
                            header.Cell().Element(EstiloHeader).Text("Cliente");
                            header.Cell().Element(EstiloHeader).Text("Equipo");
                            header.Cell().Element(EstiloHeader).Text("Estado");
                            header.Cell().Element(EstiloHeader).AlignRight().Text("Monto");

                            static IContainer EstiloHeader(IContainer container) =>
                                container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                        });

                        foreach (var orden in ordenes)
                        {
                            table.Cell().Element(EstiloCelda).Text(orden.FechaIngreso.ToString("dd/MM/yyyy"));
                            table.Cell().Element(EstiloCelda).Text(orden.Cliente?.NombreCompleto ?? "Sin Cliente");
                            table.Cell().Element(EstiloCelda).Text(orden.Equipo?.DescripcionCompleta ?? "-");
                            table.Cell().Element(EstiloCelda).Text(orden.Estado?.Nombre ?? "-");
                            table.Cell().Element(EstiloCelda).AlignRight().Text($"{orden.PrecioFinal:C}");

                            static IContainer EstiloCelda(IContainer container) =>
                                container.BorderBottom(1).BorderColor(Colors.Grey.Lighten3).PaddingVertical(5);
                        }

                        // Fila de Total Final (Suma al pie)
                        table.Cell().ColumnSpan(4).AlignRight().PaddingTop(10).Text("SUMA TOTAL:").Bold();
                        table.Cell().AlignRight().PaddingTop(10).Text($"{totalGanancia:C}").Bold().FontColor(Colors.Green.Medium).FontSize(12);
                    });

                    // --- PIE DE PÁGINA ---
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Generado el ");
                        x.Span(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        x.Span(" - Página ");
                        x.CurrentPageNumber();
                    });
                });
            });

            return documento.GeneratePdf();
        }
    }
}