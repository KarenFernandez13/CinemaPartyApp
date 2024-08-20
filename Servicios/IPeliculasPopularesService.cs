using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObligatorioTaller1.Models;

namespace ObligatorioTaller1.Servicios
{
    public interface IPeliculasPopularesService
    {
            public List<Populares> Obtener();
            List<Populares> ObtenerTopRated();
            List<Populares> ObtenerUpcoming();
            Populares ObtenerDetallesPelicula(int movieId);
            string ObtenerTrailer(int movieId);
            Task<List<string>> ObtenerImagenes(int movieId);
            Task<List<string>> ObtenerElenco(int movieId);
            Task<List<string>> ObtenerGeneros(int movieId);
            Task<List<string>> ObtenerCompaniasProductoras(int movieId);

    }
}
