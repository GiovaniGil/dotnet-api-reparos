using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIReparos.Models
{
    public class Equipamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string NomeEquipamento { get; set; }
        [Required]
        public string Identificador { get; set; }
    }
}