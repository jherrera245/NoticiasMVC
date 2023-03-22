using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NoticiasMVC.Models
{
    public class Clasificaciones
    {
        [Key]
        public int IdClasificacion { get; set; }

        [Required]
        [DisplayName("Clasificación")]
        public string NombreClasificacion { get; set; }

        [Required]
        [DisplayName("Descripción")]
        public string DescripcionClasificacion { get; set; }
    }
}
