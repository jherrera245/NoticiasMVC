using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoticiasMVC.Data;
using NoticiasMVC.ViewModels;

namespace NoticiasMVC.Controllers
{
    public class DashboardController : Controller
    {
        //objeto de conexion
        public readonly AppDbContext _context;

        //constructor
        public DashboardController(AppDbContext context) { 
            _context = context;
        }

        // GET carga el index
        public ActionResult Index()
        {
            //creamos objeto con el recuento de los valores
            //de cada tabla de nuestra db
            var Totales = new DashboardView()
            {
                TotalNoticias = _context.Noticias.Count(),
                TotalCategorias = _context.Categorias.Count(),
                TotalClasificaciones = _context.Clasificaciones.Count(),
                TotalUsuarios = 0
            };

            return View(Totales);
        }

        //noticias por categoria
        [HttpGet]
        public ActionResult GraficaCategorias()
        {

            var total = (
                from noticia in _context.Noticias
                join categoria in _context.Categorias on noticia.IdCategoria equals categoria.IdCategoria
                group categoria by categoria.IdCategoria into g
                orderby g.Select(x => x.IdCategoria).Count() descending
                select new
                {
                    Categoria = g.Select(x => x.NombreCategoria).First(),
                    Total = g.Select(x => x.IdCategoria).Count()    
                }
            ).Take(10);

            return Json(total);
        }


        //noticias por clasificaciones
        [HttpGet]
        public ActionResult GraficaClasificaciones()
        {

            var total = (
                from noticia in _context.Noticias
                join clasificacion in _context.Clasificaciones on noticia.IdCategoria equals clasificacion.IdClasificacion
                group clasificacion by clasificacion.IdClasificacion into g
                orderby g.Select(x => x.IdClasificacion).Count() descending
                select new
                {
                    Categoria = g.Select(x => x.NombreClasificacion).First(),
                    Total = g.Select(x => x.IdClasificacion).Count()
                }
            ).Take(10);

            return Json(total);
        }

    }
}
