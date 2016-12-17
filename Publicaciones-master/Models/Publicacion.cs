namespace Publicaciones.Models
{
    public class Publicacion
    {
        public string Doi { get; set; }
        public string Titulo { get; set; }
        public string Volumen { get; set; }
        public string PaginaInicio  { get; set; }
        public string PaginaFinal { get; set; }
        public string ContifadRechazos { get; set; }

        public string NumeroDePagina{get; set;}
    }

}