using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NoticiasMVC.Data;
using NoticiasMVC.Models;
using NoticiasMVC.ViewModels;

namespace NoticiasMVC.Controllers
{
    public class NoticiasController : Controller
    {
        //agregamos un objeto de nuestra clase AppDbContext
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //inicializamos nuestro objeto de AppDbContext
        public NoticiasController
        (
            AppDbContext context, IWebHostEnvironment webHostEnvironment
        )
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        //ruta -> Views/Noticias/Index
        public IActionResult Index()
        {
            //este objeto contine las Noticias
            IEnumerable<Noticias> ListaNoticias = _context.Noticias;

            foreach (var noticia in ListaNoticias)
            {
                noticia.Categoria = _context.Categorias.FirstOrDefault(
                    cat => cat.IdCategoria == noticia.IdCategoria
                );
                noticia.Clasificacion = _context.Clasificaciones.FirstOrDefault(
                    cla => cla.IdClasificacion == noticia.IdClasificacion
                );
            }
            //pasamos el objeto ListaNoticia a la vista
            return View(ListaNoticias);
        }

        //Metodo HttpGet
        //ruta -> Views/Noticias/Create
        public IActionResult Create() {

            NoticiasView Noticia = new NoticiasView()
            {
                Noticias = new Noticias(),
                CategoriasSelectList = _context.Categorias.Select(
                    i => new SelectListItem {
                        Text = i.NombreCategoria,
                        Value = i.IdCategoria.ToString()
                    }
                ),
                ClasificacionesSelectList = _context.Clasificaciones.Select(
                    i => new SelectListItem
                    {
                        Text = i.NombreClasificacion,
                        Value = i.IdClasificacion.ToString()
                    }
                )
            };
            
            return View(Noticia);
        }

        //metodo para subir imagenes al servidor
        private string SaveImageServer(bool IsUpdate, string Imagen) {
            //objetos para el manejo de archivos
            var ImageFile = HttpContext.Request.Form.Files;
            var WebRootPath = _webHostEnvironment.WebRootPath;
            string Upload = WebRootPath + WebConst.NoticiasPath; //Nueva ubicación del archivo

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
        public IActionResult Create(NoticiasView AddNoticia) { 
            //verficamos que los datos se enviaron correctamente
            if (ModelState.IsValid)
            {
                string SaveImage = SaveImageServer(
                    false, AddNoticia.Noticias.ImagenNoticia
                );
                //actualzamos el nombre de la imagen en el modelo
                AddNoticia.Noticias.ImagenNoticia = SaveImage;
                AddNoticia.Noticias.FechaIngreso = DateTime.Now;
                AddNoticia.Noticias.FechaActualizacion = DateTime.Now;
                //guardando
                _context.Noticias.Add(AddNoticia.Noticias);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            AddNoticia.CategoriasSelectList = _context.Categorias.Select(
                i => new SelectListItem
                {
                    Text = i.NombreCategoria,
                    Value = i.IdCategoria.ToString()
                }
            );

            AddNoticia.ClasificacionesSelectList = _context.Clasificaciones.Select(
                i => new SelectListItem
                {
                    Text = i.NombreClasificacion,
                    Value = i.IdClasificacion.ToString()
                }
            );

            return View(AddNoticia);
        }


    }
}
