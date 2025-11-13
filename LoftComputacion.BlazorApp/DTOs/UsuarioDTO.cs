// LoftComputacion.BlazorApp/DTOs/UsuarioDTO.cs

using System.ComponentModel.DataAnnotations;

public class UsuarioDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "Formato de email incorrecto.")]
    public string Email { get; set; } = string.Empty;

    // Asumimos que Rol es un string para el ClaimTypes.Role
    [Required(ErrorMessage = "El rol es obligatorio.")]
    public string Rol { get; set; } = string.Empty;

    public bool EstaActivo { get; set; }
}