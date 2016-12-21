using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Publicaciones.Models
{
    /// <summary>
    /// Clase encargada de representar una publicacion.
    /// </summary>
    /// <returns></returns>
    public class Publicacion
    {
        [Key]
        /// <summary>
        /// Identificador de la publicacion.
        /// </summary>
        /// <returns></returns>
        public string Doi { get; set; }

        /// <summary>
        /// Titulo de la publicacion.
        /// </summary>
        /// <returns></returns>
        public string Titulo { get; set; }

        /// <summary>
        /// Volumen de la publicacion.
        /// </summary>
        /// <returns></returns>
        public string Volumen { get; set; }

        /// <summary>
        /// Pagina de inicio de la publicacion.
        /// </summary>
        /// <returns></returns>
        public string PaginaInicio  { get; set; }

        /// <summary>
        /// Pagina final de la publicacion.
        /// </summary>
        /// <returns></returns>
        public string PaginaFinal { get; set; }

        /// <summary>
        /// Cantidad de Rechazos que ha tenido la publicacion.
        /// </summary>
        /// <returns></returns>
        public string CantidadRechazos { get; set; }

        /// <summary>
        /// Numero de paginas que tiene la revista.
        /// </summary>
        /// <returns></returns>
        public string NumeroDePagina{get; set;}

    }

}