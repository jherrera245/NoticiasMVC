using Microsoft.AspNetCore.Mvc.Rendering;
using NoticiasMVC.Models;

namespace NoticiasMVC.ViewModels
{
    public class NoticiasView
    {
        //referencia al modelo noticias
        public Noticias? Noticias { get; set; }

        //Propiedades para lsita de seleccion de categorias y clasificaciones
        public IEnumerable<SelectListItem>? CategoriasSelectList { get; set; }
        public IEnumerable<SelectListItem>? ClasificacionesSelectList { get; set; }
    }
}
