namespace Publicaciones.Models{

    /// <summary>
    /// Clase encargada de representar a un autor.
    /// </summary>
    /// <returns></returns>
    
    public class Autor{
        
        
        /// <summary>
        /// Identificador de autor.
        /// </summary>
        /// <returns></returns>
        public string IdAutor { get; set; }

        /// <summary>
        /// Fecha en el que la persona hizo la publicacion.
        /// </summary>
        /// <returns></returns>
        public string Fecha { get; set; }

        /// <summary>
        /// Tipo de autor
        /// </summary>
        /// <returns></returns>
        public Tipo tipo { get; set; }

        /// <summary>
        /// Persona que es autor.
        /// </summary>
        /// <returns></returns>
        public virtual Persona persona { get; set; }
        
        /// <summary>
        /// Persona que es autor.
        /// </summary>
        /// <returns></returns>
        public virtual Paper paper { get; set; }
    }
    /// <summary>
    /// Enumearacion de tipos de autor
    /// </summary>
    /// <returns></returns>
    public enum Tipo {
        PRINCIPAL,
        CORRESPONDIENTE,
        NORMAL
    }
}