namespace LoftComputacion.Shared.DTOs;

public class UsuarioDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = "";
    public string Email { get; set; } = "";
    public string Rol { get; set; } = "";
    public bool Activo { get; set; } // <--- Agregamos esto
}