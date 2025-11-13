namespace LoftComputacion.Application.DTOs
{
    public class CreateOrdenDto
    {
        public int ClienteId { get; set; }
        public int EquipoId { get; set; }
        public string FallaDeclaradaPorCliente { get; set; } = string.Empty;
    }
}