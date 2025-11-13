// LoftComputacion.BlazorApp/DTOs/UsuarioCreacionDTO.cs

// Hereda de UsuarioDTO para reutilizar las propiedades
using System.ComponentModel.DataAnnotations;

public class UsuarioCreacionDTO : UsuarioDTO
{
    [Required(ErrorMessage = "La contraseña es obligatoria al crear.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Password { get; set; } = string.Empty;
}