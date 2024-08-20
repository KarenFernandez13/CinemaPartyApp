using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObligatorioTaller1.Models
{
    public partial class Sucursal
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        [Unique]
        public string Name { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public int HoraApertura { get; set; }
        public int HoraCierre { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
