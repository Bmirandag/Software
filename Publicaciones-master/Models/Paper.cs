using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Publicaciones.Models{

    /// <summary>
    /// Clase encargada de representar un paper.
    /// </summary>
    /// <returns></returns>
    public class Paper{

        [Key]
        /// <summary>
        /// Identificador del paper.
        /// </summary>
        /// <returns></returns>
        public string IdentificadorPaper { get; set; }

        /// <summary>
        /// Titulo del paper.
        /// </summary>
        /// <returns></returns>
        public string Titulo { get; set; }

        /// <summary>
        /// Fecha de inicio del paper.
        /// </summary>
        /// <returns></returns>
        public string FechaInicio { get; set; }

        /// <summary>
        /// Fecha de termino del paper.
        /// </summary>
        /// <returns></returns>
        public string FechaTermino { get; set; }

        /// <summary>
        /// Resumen del paper.
        /// </summary>
        /// <returns></returns>
        public string Abstract { get; set; }

        /// <summary>
        /// Estado del paper.
        /// </summary>
        /// <returns></returns>
        public Estado estado { get; set; }

        /// <summary>
        /// Publicacion.
        /// </summary>
        /// <returns></returns>
        public virtual Publicacion publicacion { get; set; }

        /// <summary>
        /// Lista que contiene los autores del paper.
        /// </summary>
        /// <returns></returns>
        public virtual List<Autor> Autores { get; set; }
    }
    /// <summary>
    /// Enumeracion.
    /// </summary>
    /// <returns></returns>
    public enum Estado{
        ACEPTADO,
        RECHAZADO
    }
}