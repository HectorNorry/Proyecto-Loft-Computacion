using System.ComponentModel.DataAnnotations;

namespace LoftComputacion.BlazorApp.DTOs
{
    public class LoginModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
    }
}