using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Publicaciones.Models
{
    /// <summary>
    /// Clase encargada de representar un Grado.
    /// </summary>
    /// <returns></returns>
    public class Grado
    {
        [Key]
        public string IdGrado { get; set; }
        /// <summary>
        /// Nombre del grado academico
        /// </summary>
        /// <returns></returns>
        public string Nombre { get; set; }

        /// <summary>
        /// Fecha en que se obtuvo el grado academico
        /// </summary>
        /// <returns></returns>
        public string Fecha { get; set;} 

        /// <summary>
        /// Rut de la persona que tiene el grado
        /// </summary>
        /// <returns></returns>
        public string Rut { get; set;} 
    }   

}