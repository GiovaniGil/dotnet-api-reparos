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
    public class EquipamentosController : ControllerBase
    {
        private readonly ReparosContext _context;

        public EquipamentosController(ReparosContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Adm")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var equipamentos = await _context.Equipamentos.ToArrayAsync();

            // var resposta = equipamentos.Select(e => new
            // {
            //     nomeEquipamento = e.NomeEquipamento,
            //     data = new DateTime()
            // });

            return Ok(equipamentos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var resultado = _context.Equipamentos
                                    .Where(equipamento => equipamento.Id == id)
                                    .FirstOrDefault();

            if(resultado == null) {
                return NoContent();
            }
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Equipamento equipamento)
        {
            _context.Equipamentos.Add(equipamento);
            await _context.SaveChangesAsync();

            return Ok(equipamento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Equipamento equipamento)
        {
            if (equipamento.Id != id)
            {
                return BadRequest();
            }
            _context.Entry(equipamento).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(equipamento);
        }
        

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Equipamentos
                                   .Where(equipamento => equipamento.Id == id)
                                   .SingleAsync();

            _context.Equipamentos.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }     
    }
}