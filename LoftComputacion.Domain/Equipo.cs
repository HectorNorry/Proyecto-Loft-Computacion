using LoftComputacion.Shared.Enums;

namespace LoftComputacion.Domain
{
    public class Equipo
    {
        public int Id { get; set; }
        public TipoDeEquipo Tipo { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public string? Componentes { get; set; }
        public string? NumeroDeSerie { get; set; }

        public string DescripcionCompleta
        {
            get
            {
                string tipoTexto = Tipo.ToString().Replace("_", " ");
                return $"{tipoTexto} {Marca} {Modelo}".Trim();
            }
        }
    }
}
