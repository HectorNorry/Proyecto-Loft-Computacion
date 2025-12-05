using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoftComputacion.Shared.DTOs
{
    public class MetricasDto
    {
        // 1. Eficiencia General
        public int TotalOrdenesMes { get; set; }
        public int CantidadFinalizadas { get; set; }
        public int CantidadCanceladas { get; set; }
        public double TasaRebote { get; set; } // % de canceladas sobre el total finalizado

        // 2. Métricas de Hardware (Para gráfico de Torta)
        // Ej: "Notebook": 15, "PC": 5
        public List<DatoGrafico> OrdenesPorTipo { get; set; } = new();

        // 3. Métricas de Equipo (Para gráfico de Barras)
        // Ej: "Admin": 10 finalizadas, "Técnico": 4 finalizadas
        public List<DatoGrafico> RendimientoTecnicos { get; set; } = new();
    }

    public class DatoGrafico
    {
        public string Etiqueta { get; set; } = "";
        public double Valor { get; set; }
    }
}
