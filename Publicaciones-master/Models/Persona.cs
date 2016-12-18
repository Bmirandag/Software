using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Publicaciones.Models
{
    /// <summary>
    /// Clase encargada de representar una persona.
    /// </summary>
    /// <returns></returns>
    public class Persona
    {
        [Key]
        /// <summary>
        /// Rut de la persona.
        /// </summary>
        /// <returns></returns>
        public string Rut { get; set; }

        /// <summary>
        /// Nombre de la persona.
        /// </summary>
        /// <returns></returns>
        public string Nombre { get; set; }

        /// <summary>
        /// Apellido de la persona.
        /// </summary>
        /// <returns></returns>
        public string Apellido { get; set; }

        /// <summary>
        /// Email de la persona.
        /// </summary>
        /// <returns></returns>
        public string Email { get; set; }

        /// <summary>
        /// Fecha de nacimiento de la persona.
        /// </summary>
        /// <returns></returns>        
        public int FechaNacimiento { get; set; }

        /// <summary>
        /// Nacionalidad de la persona.
        /// </summary>
        /// <returns></returns>
        public string Nacionalidad { get; set; }

        /// <summary>
        /// Genero de la persona.
        /// </summary>
        /// <returns></returns>
        public string Genero { get; set; }
    
    }
}