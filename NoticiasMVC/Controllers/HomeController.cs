using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoticiasMVC.Data;
using NoticiasMVC.Models;
using NoticiasMVC.ViewModels;
using System.Diagnostics;

namespace NoticiasMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //este objeto contine las Categorias
            IEnumerable<Categorias> ListaCategorias = _context.Categorias;
            //pasamos el objeto ListaCategorias a la vista
            return View(ListaCategorias);
        }

        public IActionResult Details(int? Id)
        {
            //verficamos que se paso el id por la url
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var Home = new HomeView()
            {
                Noticias = _context.Noticias.Include(
                    i => i.Categoria
                ).Include(
                    i => i.Clasificacion
                ).Where(i => i.IdCategoria == Id),
                Clasificaciones = _context.Clasificaciones,
            };

            return View(Home);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}