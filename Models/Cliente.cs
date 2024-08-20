using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ObligatorioTaller1.Models
{
    public class Cliente
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public int Telefono { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
