using System;

namespace LoftComputacion.Application.DTOs
{
    public class OrdenSimpleDto
    {
        public DateTime Fecha { get; set; }
        public string ClienteNombre { get; set; } = "";
        public string Estado { get; set; } = "";
        public decimal? Total { get; set; }
        public int Id { get; set; }
    }

}
