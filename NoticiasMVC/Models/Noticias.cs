using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoticiasMVC.Models
{
    public class Noticias
    {
        [Key]
        public int IdNoticia { get; set; }

        [Required]
        [DisplayName("Titulo")]
        public string TituloNoticia { get; set;}

        [Required]
        [DisplayName("Descripción de la Noticia")]
        public string DescripcionNoticia { get; set; }

        [DisplayName("Imagen")]
        public string? ImagenNoticia { get; set; }

        [DisplayName("Categoría")]
        public int IdCategoria { get; set; }

        //agregando relaciones entre tablas noticias -> categorias
        [ForeignKey("IdCategoria")]
        public virtual Categorias? Categoria { get; set; }

        [DisplayName("Clasificación")]
        public int IdClasificacion { get; set; }

        //agregando relaciones entre tablas noticias -> clasificaciones
        [ForeignKey("IdClasificacion")]
        public virtual Clasificaciones? Clasificacion { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public DateTime? FechaActualizacion { get; set; }
    }
}
