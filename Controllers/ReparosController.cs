using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

using System.Net.Http;
using APIReparos.Models;
using APIReparos.Context;
using Newtonsoft.Json;

namespace APIReparos.Controllers
{

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ReparosController : ControllerBase
    {
        private readonly ReparosContext _context;

        public ReparosController(ReparosContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var reparos = await _context.Reparos
                .Include(reparo => reparo.Equipamento)
                .ToArrayAsync();
            return Ok(reparos);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var resultado = _context.Reparos
                                    .Where(reparo => reparo.Id == id)
                                    .FirstOrDefault();

            if (resultado == null)
            {
                return NoContent();
            }
            return Ok(resultado);
        }

        [Authorize(Roles = "Adm")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Reparo reparo)
        {
            _context.Reparos.Add(reparo);
            await _context.SaveChangesAsync();

            return Ok(reparo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Reparo reparo)
        {
            if (reparo.Id != id)
            {
                return BadRequest();
            }
            _context.Entry(reparo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(reparo);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Reparos
                                    .Where(reparo => reparo.Id == id)
                                    .SingleAsync();

            _context.Reparos.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody]JsonPatchDocument<Reparo> reparo)
        {
            Console.WriteLine(reparo);
            if (reparo == null)
            {
                return BadRequest(ModelState);
            }
            var reparoDB = await _context.Reparos.FirstOrDefaultAsync(rep => rep.Id == id);
            if (reparoDB == null)
            {
                return NotFound();
            }

            reparo.ApplyTo(reparoDB, ModelState);
            var isValid = TryValidateModel(reparoDB);
            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}