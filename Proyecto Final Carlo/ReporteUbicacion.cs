using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Final_Carlo
{
    internal class ReporteUbicacion
    {
        // Propiedades de la clase ReporteUbicacion 
        // Estas propiedades representan los datos que se enviarán al servidor y se mostrarán en el mapa.
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
    }
}

