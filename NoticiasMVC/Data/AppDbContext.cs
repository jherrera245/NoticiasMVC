using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NoticiasMVC.Models;

namespace NoticiasMVC.Data
{
    /**
     * En esta parte tenemos dos opciones
     * 1) Implementar DbContext que no incluye la las migraciones 
     *    con las tablas de manejo de usuarios
     * 2) Implementar IdentityDbContext el cual incluye las migraciones
     *    con las tablas para el manejo de usuarios
     */
    public class AppDbContext : IdentityDbContext
    {
        //agregamos un construtor vacion para la clase AppDbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        //Definiones de modelos o tablas
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Clasificaciones> Clasificaciones { get; set; }
        public DbSet<Noticias> Noticias { get; set; }
    }
}
