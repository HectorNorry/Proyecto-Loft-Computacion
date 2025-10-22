using ClosedXML.Excel;
using LoftComputacion.Domain;
using LoftComputacion.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO; // Necesario para MemoryStream
using System.Linq;
using System.Text;
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

        // Método para obtener las órdenes pagadas en un rango de fechas
        public async Task<IEnumerable<OrdenDeServicio>> GetGananciasPorFechaAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            // Ajustamos fechaHasta para incluir todo el día
            var fechaHastaAjustada = fechaHasta.Date.AddDays(1).AddTicks(-1);

            return await _context.OrdenesDeServicio
                .Include(o => o.Cliente) // Incluimos cliente para mostrar nombre
                .Include(o => o.MetodoDePago) // Incluimos método de pago
                .Where(o => o.FechaPago >= fechaDesde.Date && o.FechaPago <= fechaHastaAjustada) // Filtramos por fecha de PAGO
                .OrderByDescending(o => o.FechaPago) // Ordenamos por fecha de pago
                .ToListAsync();
        }


        public async Task<byte[]> GetGananciasExcelAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            // 1. Obtenemos los datos de la misma forma que antes
            var ordenesPagadas = await GetGananciasPorFechaAsync(fechaDesde, fechaHasta);

            // 2. Creamos un libro de Excel en memoria
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ganancias");

                // --- 3. Crear Encabezados ---
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Fecha Pago";
                worksheet.Cell(currentRow, 2).Value = "Cliente";
                worksheet.Cell(currentRow, 3).Value = "Monto Cobrado";
                worksheet.Cell(currentRow, 4).Value = "Método Pago";

                // Aplicar estilo a la fila de encabezado
                worksheet.Row(currentRow).Style.Font.Bold = true;
                worksheet.Row(currentRow).Style.Fill.BackgroundColor = XLColor.LightGray;

                // --- 4. Agregar Datos ---
                foreach (var orden in ordenesPagadas)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = orden.FechaPago;
                    worksheet.Cell(currentRow, 2).Value = orden.Cliente?.NombreCompleto ?? "N/A";
                    worksheet.Cell(currentRow, 3).Value = orden.PrecioFinal ?? 0;
                    worksheet.Cell(currentRow, 4).Value = orden.MetodoDePago?.Nombre ?? "N/A";
                }

                // --- 5. Agregar Fila de Total ---
                currentRow++;
                worksheet.Cell(currentRow, 2).Value = "Total Ganancias:";
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                // Usamos una fórmula de Excel para sumar la columna C
                worksheet.Cell(currentRow, 3).FormulaA1 = $"=SUM(C2:C{currentRow - 1})";

                // --- 6. Aplicar Formatos y Ajustar Columnas ---
                worksheet.Column(1).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
                worksheet.Column(3).Style.NumberFormat.Format = "$ #,##0.00"; // Formato moneda
                worksheet.Columns().AdjustToContents(); // Ajustar ancho de columnas

                // --- 7. Guardar en un stream de memoria ---
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray(); // Devolvemos los bytes del archivo
                }
            }
        }
    }
}