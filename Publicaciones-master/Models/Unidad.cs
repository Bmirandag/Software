namespace Publicaciones.Models{

    /// <summary>
    /// Clase encargada de representar a una unidad a la que pertence otra unidad o un autor.
    /// </summary>
    /// <returns></returns>
    public class Unidad{

        /// <summary>
        /// Nombre de la unidad.
        /// </summary>
        /// <returns></returns>  
        public string Nombre{set;get;}
      
        /// <summary>
        /// Direccion de la unidad.
        /// </summary>
        /// <returns></returns>  
        public string Direccion{set;get;}
      
        /// <summary>
        /// Unidad a la que pertence la unidad, puede ser null.
        /// </summary>
        /// <returns></returns>  
        public Unidad UnidadSuperior{set;get;}
    }
}