using LoftComputacion.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsuariosController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsuarios()
    {
        var list = await _usuarioService.GetAllUsuariosAsync();
        return Ok(list);
    }

    [HttpPost]
    public async Task<IActionResult> CrearUsuario(CreateUsuarioDto dto)
    {
        var result = await _usuarioService.CreateUsuarioAsync(dto);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { result.UserId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditarUsuario(int id, UpdateUsuarioDto dto)
    {
        var ok = await _usuarioService.UpdateUsuarioAsync(id, dto);
        if (!ok)
            return NotFound("Usuario no encontrado");

        return Ok();
    }

    [HttpPut("estado/{id}")]
    public async Task<IActionResult> CambiarEstado(int id)
    {
        var ok = await _usuarioService.DeactivateUsuarioAsync(id);
        if (!ok)
            return NotFound();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarUsuario(int id)
    {
        var ok = await _usuarioService.DeactivateUsuarioAsync(id);
        if (!ok)
            return NotFound();

        return Ok();
    }
}
