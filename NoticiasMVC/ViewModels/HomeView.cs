using NoticiasMVC.Models;

namespace NoticiasMVC.ViewModels
{
    public class HomeView
    {
        public IEnumerable<Noticias> Noticias { get; set; }
        public IEnumerable<Clasificaciones> Clasificaciones { get; set; }
    }
}
