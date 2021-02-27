using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace APIReparos.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string NomeUsuario { get; set; }
        public string Email { get; set;  }
        public string Senha { get; set; }
        public Role Role { get; set; }
        [JsonIgnore]
        public List<Reparo> Reparos { get; set; }
    }
    
    public enum Role
    {
        User,
        Adm
    }
}