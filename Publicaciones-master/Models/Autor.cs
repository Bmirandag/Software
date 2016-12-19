namespace Publicaciones.Models{

    /// <summary>
    /// Clase encargada de representar a un autor.
    /// </summary>
    /// <returns></returns>
    
    public class Autor{
        
        /// <summary>
        /// Fecha en el que la persona hizo la publicacion.
        /// </summary>
        /// <returns></returns>
        public string Fecha { get; set; }

        /// <summary>
        /// Rut de la persona quien hizo la publicacion.
        /// </summary>
        /// <returns></returns>
        public string Rut { get; set; }

        /// <summary>
        /// Identificador de la publicacion.
        /// </summary>
        /// <returns></returns>
        public string Doi { get; set; }

        /// <summary>
        /// Grado academico del autor 
        /// al momento de la publicacion
        /// </summary>
        /// <returns></returns>
        private Grado grado { get; set;}
        
    }
}