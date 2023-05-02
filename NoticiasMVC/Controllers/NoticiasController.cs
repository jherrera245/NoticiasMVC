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

        //Metodo HttpGet
        //ruta -> Views/Noticias/Edit/{IdNoticia}
        public IActionResult Edit(int? Id)
        {
            //verficamos que se paso el id por la url
            if (Id == null || Id == 0) {
                return NotFound();
            }

            //Consultamos el registro segun su ID
            NoticiasView Noticia = new NoticiasView()
            {
                Noticias = _context.Noticias.Find(Id),
                CategoriasSelectList = _context.Categorias.Select(
                    i => new SelectListItem{
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
            //si no encuentra el registro
            if (Noticia == null) {
                return NotFound();
            }
            //Si el registro es encotrado pasamos el objeto a la vista
            return View(Noticia);
        }

        //Metodo HttpPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NoticiasView EditNoticia)
        {
            //verficamos que los datos se enviaron correctamente
            if (ModelState.IsValid)
            {
                string SaveImage = SaveImageServer(true, EditNoticia.Noticias.ImagenNoticia);
                //actualzamos el nombre de la imagen en el modelo
                EditNoticia.Noticias.ImagenNoticia = SaveImage;
                EditNoticia.Noticias.FechaActualizacion = DateTime.Now;
                //guardando
                _context.Noticias.Update(EditNoticia.Noticias);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            EditNoticia.CategoriasSelectList = _context.Categorias.Select(
                i => new SelectListItem
                {
                    Text = i.NombreCategoria,
                    Value = i.IdCategoria.ToString()
                }
            );

            EditNoticia.ClasificacionesSelectList = _context.Clasificaciones.Select(
                i => new SelectListItem
                {
                    Text = i.NombreClasificacion,
                    Value = i.IdClasificacion.ToString()
                }
            );
            return View(EditNoticia);
        }

        //Metodo HttpGet
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var Noticia = _context.Noticias.Find(Id);

            if (Noticia == null)
            {
                return NotFound();
            }
           
            string Upload = _webHostEnvironment.WebRootPath + WebConst.NoticiasPath;
            var OldFile = Path.Combine(Upload, Noticia.ImagenNoticia);
            
            if (System.IO.File.Exists(OldFile))
            {
                System.IO.File.Delete(OldFile);
            }

            _context.Noticias.Remove(Noticia);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
