using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using APIReparos.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;
using System.Security.Claims;
using APIReparos.Context;
using System.Threading.Tasks;

namespace APIReparos.Controllers
{

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ReparosContext _context;

        public AuthController(ReparosContext context)
        {
            _context = context;
        }
        
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] Login login)
        {
            if (login == null) {
              return BadRequest("Invalid client request");
            }
            
            var resultado = _context.Usuarios
                        .Where(usuario => usuario.NomeUsuario == login.NomeUsuario &&
                                          usuario.Senha == login.Senha)
                        .FirstOrDefault();

            if (resultado != null) {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret@jwt123#secret@jwt123#"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                
                var claims = new[] {
                            new Claim(ClaimTypes.PrimarySid, resultado.Id.ToString()),
                            new Claim(ClaimTypes.Email, resultado.Email),
                            new Claim(ClaimTypes.Name, resultado.NomeUsuario),
                            new Claim(ClaimTypes.Role, resultado.Role.ToString()) };


                var tokenOptions = new JwtSecurityToken(
                   issuer: "http://localhost:5000",
                   audience: "http://localhost:5000",
                   claims: claims,
                   expires: DateTime.Now.AddMinutes(5),
                   signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }
            else {
                return Unauthorized();
            }
        }
    }
}