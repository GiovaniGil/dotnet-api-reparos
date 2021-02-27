using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIReparos.Models
{
    public class Reparo
    {
        public int Id { get; set; }
        [Required]
        public int UsuarioId { get; set; }
        [JsonIgnore]
        public Usuario Usuario { get; set; }
        [Required]
        public int EquipamentoId { get; set;  }
        public Equipamento Equipamento { get; set; }

        public DateTime DataInicio { get; set; }

        public Nullable<DateTime> DataFim { get; set; }        
        [Required]
        public Status StatusReparo { get; set;  }

        public string Observacao { get; set; }
    }

    public enum Status
    {
        Iniciar,
        Parar, 
        Concluir
    }
}