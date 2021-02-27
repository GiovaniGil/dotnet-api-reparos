using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

using APIReparos.Models;
using APIReparos.Context;

namespace APIReparos.Controllers
{

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ReparosContext _context;

        public UsuariosController(ReparosContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Adm")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarios = await _context.Usuarios.ToArrayAsync();

            var resposta = usuarios.Select(e => new
            {
                id = e.Id,
                nomeUsuario = e.NomeUsuario,
                email = e.Email
            });

            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var resultado = _context.Usuarios
                                    .Where(usuario => usuario.Id == id)
                                    .Include(usuario => usuario.Reparos)
                                    .FirstOrDefault();

            if (resultado == null)
            {
                return NoContent();
            }
            var resposta = new {
                id = resultado.Id,
                nomeUsuario = resultado.NomeUsuario,
                email = resultado.Email,
                reparos = resultado.Reparos
            };
            return Ok(resposta);
        }

        [Authorize(Roles = "Adm")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            if(usuario.Senha == null)
                usuario.Senha = "1234";
                
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var resposta = new {
                id = usuario.Id,
                nomeUsuario = usuario.NomeUsuario,
                email = usuario.Email,
            };

            return Ok(resposta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Usuario usuario)
        {
            if (usuario.Id != id)
            {
                return BadRequest();
            }
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

        [Authorize(Roles = "Adm")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Usuarios
                                   .Where(usuario => usuario.Id == id)
                                   .SingleAsync();

            _context.Usuarios.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }        
    }
}