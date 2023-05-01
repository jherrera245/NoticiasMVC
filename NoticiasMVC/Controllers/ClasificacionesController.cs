using Microsoft.AspNetCore.Mvc;
using NoticiasMVC.Data;
using NoticiasMVC.Models;
using static System.Net.Mime.MediaTypeNames;

namespace NoticiasMVC.Controllers
{
    public class ClasificacionesController : Controller
    {
        //agregamos un objeto de nuestra clase AppDbContext
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //inicializamos nuestro objeto de AppDbContext
        public ClasificacionesController
        (
            AppDbContext context, IWebHostEnvironment webHostEnvironment
        )
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        //ruta -> Views/Clasificaciones/Index
        public IActionResult Index()
        {
            //este objeto contine las Clasificaciones
            IEnumerable<Clasificaciones> ListaClasificaciones = _context.Clasificaciones;
            //pasamos el objeto ListaClasificacion a la vista
            return View(ListaClasificaciones);
        }

        //Metodo HttpGet
        //ruta -> Views/Clasificaciones/Create
        public IActionResult Create() {
            return View();
        }


        //Metodo HttpPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Clasificaciones AddClasificacion) { 
            //verficamos que los datos se enviaron correctamente
            if (ModelState.IsValid)
            {
                //guardando
                _context.Clasificaciones.Add(AddClasificacion);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(AddClasificacion);
        }

        //Metodo HttpGet
        //ruta -> Views/Clasificaciones/Edit/{IdClasificacion}
        public IActionResult Edit(int? Id)
        {
            //verficamos que se paso el id por la url
            if (Id == null || Id == 0) {
                return NotFound();
            }

            //Consultamos el registro segun su ID
            var Clasificacion = _context.Clasificaciones.Find(Id);

            //si no encuentra el registro
            if (Clasificacion == null) {
                return NotFound();
            }
            //Si el registro es encotrado pasamos el objeto a la vista
            return View(Clasificacion);
        }

        //Metodo HttpPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Clasificaciones EditClasificacion)
        {
            //verficamos que los datos se enviaron correctamente
            if (ModelState.IsValid)
            {
                //guardando
                _context.Clasificaciones.Update(EditClasificacion);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(EditClasificacion);
        }

        //Metodo HttpGet
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var Clasificacion = _context.Clasificaciones.Find(Id);

            if (Clasificacion == null)
            {
                return NotFound();
            }
           
            _context.Clasificaciones.Remove(Clasificacion);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
