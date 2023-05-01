using Microsoft.AspNetCore.Mvc;
using NoticiasMVC.Data;
using NoticiasMVC.Models;
using static System.Net.Mime.MediaTypeNames;

namespace NoticiasMVC.Controllers
{
    public class CategoriasController : Controller
    {
        //agregamos un objeto de nuestra clase AppDbContext
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //inicializamos nuestro objeto de AppDbContext
        public CategoriasController
        (
            AppDbContext context, IWebHostEnvironment webHostEnvironment
        )
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        //ruta -> Views/Categorias/Index
        public IActionResult Index()
        {
            //este objeto contine las Categorias
            IEnumerable<Categorias> ListaCategorias = _context.Categorias;
            //pasamos el objeto ListaNoticia a la vista
            return View(ListaCategorias);
        }

        //Metodo HttpGet
        //ruta -> Views/Categorias/Create
        public IActionResult Create() {
            return View();
        }

        //metodo para subir imagenes al servidor
        private string SaveImageServer(bool IsUpdate, string Imagen) {
            //objetos para el manejo de archivos
            var ImageFile = HttpContext.Request.Form.Files;
            var WebRootPath = _webHostEnvironment.WebRootPath;
            string Upload = WebRootPath + WebConst.CategoriasPath; //Nueva ubicación del archivo

            if (IsUpdate)
            {
                if (ImageFile.Count > 0)
                {
                    var OldFile = Path.Combine(Upload, Imagen);
                    if (System.IO.File.Exists(OldFile))
                    {
                        System.IO.File.Delete(OldFile);
                    }
                }
                else
                {
                    return Imagen;
                }
            }
            string ImageFileName = Guid.NewGuid().ToString(); //Generando un nuevo nombre para la imagen
            string Extension = Path.GetExtension(ImageFile[0].FileName);//Obteniendo extension de archivo
            //Guardando archivo en el servidor
            using var ImageFileStream = new FileStream(
                Path.Combine(Upload, ImageFileName + Extension),
                FileMode.Create
            );
            ImageFile[0].CopyTo(ImageFileStream);
            return ImageFileName + Extension;
        }

        //Metodo HttpPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categorias AddCategoria) { 
            //verficamos que los datos se enviaron correctamente
            if (ModelState.IsValid)
            {
                string SaveImage = SaveImageServer(
                    false, AddCategoria.ImagenCategoria
                );
                //actualzamos el nombre de la imagen en el modelo
                AddCategoria.ImagenCategoria = SaveImage;
                //guardando
                _context.Categorias.Add(AddCategoria);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(AddCategoria);
        }

        //Metodo HttpGet
        //ruta -> Views/Categorias/Edit/{IdCategoria}
        public IActionResult Edit(int? Id)
        {
            //verficamos que se paso el id por la url
            if (Id == null || Id == 0) {
                return NotFound();
            }

            //Consultamos el registro segun su ID
            var Categoria = _context.Categorias.Find(Id);

            //si no encuentra el registro
            if (Categoria == null) {
                return NotFound();
            }
            //Si el registro es encotrado pasamos el objeto a la vista
            return View(Categoria);
        }

        //Metodo HttpPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categorias EditCategoria)
        {
            //verficamos que los datos se enviaron correctamente
            if (ModelState.IsValid)
            {
                string SaveImage = SaveImageServer(
                    true, EditCategoria.ImagenCategoria
                );
                //actualziamos el nombre de la imagen en el modelo
                EditCategoria.ImagenCategoria = SaveImage;
                //guardando
                _context.Categorias.Update(EditCategoria);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(EditCategoria);
        }

        //Metodo HttpGet
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var Categoria = _context.Categorias.Find(Id);

            if (Categoria == null)
            {
                return NotFound();
            }
           
            string Upload = _webHostEnvironment + WebConst.CategoriasPath;
            var OldFile = Path.Combine(Upload, Categoria.ImagenCategoria);
            
            if (System.IO.File.Exists(OldFile))
            {
                System.IO.File.Delete(OldFile);
            }

            _context.Categorias.Remove(Categoria);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
