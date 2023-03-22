using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NoticiasMVC.Models
{
    public class Categorias
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required]
        [DisplayName("Categoría")]
        public string NombreCategoria { get; set;}

        [Required]
        [DisplayName("Descripción")]
        public string DescripcionCategoria { get; set;}

        [DisplayName("Imagen")]
        public string ImagenCategoria { get; set;}
    }
}
