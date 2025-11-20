using LoftComputacion.Domain;

public class Foto
{
    public int Id { get; set; }

    public string RutaArchivo { get; set; } = string.Empty;

    public string Url => RutaArchivo;

    public int OrdenDeServicioId { get; set; }
    public OrdenDeServicio OrdenDeServicio { get; set; } = null!;
}
