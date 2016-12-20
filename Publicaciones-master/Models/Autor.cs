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
        /// Tipo de autor
        /// </summary>
        /// <returns></returns>
        public Tipo tipo { get; set; }

        /// <summary>
        /// Rut de la persona quien hizo la publicacion.
        /// </summary>
        /// <returns></returns>
        public virtual Persona persona { get; set; }
    }
    /// <summary>
    /// Enumearacion de tipo de autor
    /// </summary>
    /// <returns></returns>
    public enum Tipo {
        PRINCIPAL,
        CORRESPONDIENTE,
        NORMAL
    }
}