using System; 
using System.Collections.Generic; 
using System.Linq; 
using Microsoft.Extensions.Logging; 
using Publicaciones.Backend; 
using Publicaciones.Models; 

namespace Publicaciones.Service {

    /// <summary>
    /// Metodos de la interface
    /// </summary>
    public interface IMainService {

        void AddPersona(Persona persona); 

        void AddAutor(Autor autor);

        void addGrado(Grado grado);

        void addPaper(Paper paper);

        void AddPublicacion(Publicacion publicacion);

        void AddAutorToPaper(string IdentificadorPaper, Autor autor);

        List < Persona > FindPersonas(string nombre);

        List <Persona> Personas();

        List< Publicacion > Publicaciones();

        List< Publicacion > getPublicacionesByRut(string rut);

        List< Autor > getAutoresRut(string rut);

        List< Paper > getPaperByAutor(string rut);

        void Initialize(); 
    }

    /// <summary>
    /// Implementacion de la interface IMainService
    /// </summary>
    public class MainService:IMainService {

        /// <summary>
        /// Acceso al Backend
        /// </summary>
        /// <returns></returns>
        private BackendContext BackendContext { get; set; }

        /// <summary>
        /// The Logger 
        /// </summary>
        /// <returns></returns>
        private ILogger Logger { get; set; }

        private Boolean Initialized { get; set; }

        /// <summary>
        /// Constructor via DI
        /// </summary>
        /// <param name="backendContext"></param>
        /// <param name="loggerFactory"></param>
        public MainService(BackendContext backendContext, ILoggerFactory loggerFactory) {

            // Inicializacion del Logger
            Logger = loggerFactory?.CreateLogger < MainService > (); 
            if (Logger == null) {
                throw new ArgumentNullException(nameof(loggerFactory)); 
            }

            // Obtengo el backend
            BackendContext = backendContext; 
            if (BackendContext == null) {
                throw new ArgumentNullException("MainService requiere del BackendService != null"); 
            }

            // No se ha inicializado
            Initialized = false;

            Logger.LogInformation("MainService created"); 
        }

        /// <summary>
        /// Servicio que agrega Personas.
        /// </summary>
        public void AddPersona(Persona persona) {

            // Guardo la Persona en el Backend
            BackendContext.Personas.Add(persona); 

            // Guardo los cambios
            BackendContext.SaveChanges(); 
        }

        /// <summary>
        /// Servicio que agrega Autores.
        /// </summary>
        public void AddAutor(Autor autor){

            // Guardo el Autor en el Backend
            BackendContext.Autores.Add(autor); 

            // Guardo los cambios
            BackendContext.SaveChanges(); 
        }
        
        /// <summary>
        /// Servicio que agrega Grado.
        /// </summary>
         public void addGrado(Grado grado){

            // Guardo el Grado en el Backend
            BackendContext.Grados.Add(grado); 

            // Guardo los cambios
            BackendContext.SaveChanges(); 
        }

        /// <summary>
        /// Servicio que agrega paper.
        /// </summary>
         public void addPaper(Paper paper){

            // Guardo el Paper en el Backend
            BackendContext.Papers.Add(paper); 

            // Guardo los cambios
            BackendContext.SaveChanges(); 
        }

        /// <summary>
        /// Servicio que agrega publicaciones.
        /// </summary>
        public void AddPublicacion(Publicacion publicacion) {

            // Guardo la Publicacion en el Backend
            BackendContext.Publicaciones.Add(publicacion); 

            // Guardo los cambios
            BackendContext.SaveChanges(); 
        }
        
        /// <summary>
        /// Servicio que agrega un autor a un paper.
        /// </summary>
        /// <param name="IdentificadorPaper"></param>
        /// <param name="autor"></param>
        /// <returns>Lista de Personas</returns> 
        public void AddAutorToPaper(string IdentificadorPaper, Autor autor){
            BackendContext.Papers
                .Where(p => p.IdentificadorPaper == IdentificadorPaper)
                .First().Autores.Add(autor);
            BackendContext.SaveChanges();
        }

        /// <summary>
        /// Servicio que retorna personas.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns>Lista de Personas</returns>  
        public List < Persona > FindPersonas(string nombre) {
            return BackendContext.Personas
                .Where(p => p.Nombre.Contains(nombre))
                .OrderBy(p => p.Nombre)
                .ToList(); 
        }
        
        /// <summary>
        /// Servicio que retorna personas.
        /// </summary>
        /// <returns>Lista de Personas</returns> 
        public List<Persona> Personas() {
            return BackendContext.Personas.ToList();
        }
        
        /// <summary>
        /// Servicio que retorna publicaciones.
        /// </summary>
        /// <returns>Lista de Publicaciones</returns>  
         public List< Publicacion > Publicaciones() {
            return BackendContext.Publicaciones.ToList();
        }

        /// <summary>
        /// Servicio que retorna publicaciones por rut.
        /// </summary>
        /// <param name="rut"></param>
        /// <returns>Lista de Publicaciones</returns>  
        public List< Publicacion > getPublicacionesPorRut(string rut){
            List< Autor > autorias = this.getAutoresPorRut(rut);
            List< Paper > paperPorAutor = this.getPaperPorAutor(rut);
            List< Publicacion > publicacionesPorAutor = new List<Publicacion>();
            foreach(Paper paper in paperPorAutor){
                if(paper.estado == Estado.ACEPTADO){
                    publicacionesPorAutor.Add(paper.publicacion);
                }
            }
            return publicacionesPorAutor;
        }

        /// <summary>
        /// Servicio que retorna Autores por rut.
        /// </summary>
        /// <param name="rut"></param>
        /// <returns>Lista de Autores</returns> 
        public List< Autor > getAutoresPorRut(string rut){
            return BackendContext.Autores
                    .Where(a => a.persona.Rut.Contains(rut))
                    .OrderBy(a => a.Fecha)
                    .ToList();
        }

        /// <summary>
        /// Servicio que retorna los Papers por Autor.
        /// </summary>
        /// <param name="rut"></param>
        /// <returns>Lista de Paper</returns> 
        public List< Paper > getPaperPorAutor(string rut){
            List< Autor > autorias = this.getAutoresPorRut(rut);
            List< Paper > paperPorAutor = new List<Paper>();
            foreach(Autor autor in autorias){
                paperPorAutor.Add(autor.paper);
            }
            return paperPorAutor;
        }

        public void Initialize() {

            if (Initialized) {
                throw new Exception("Solo se permite llamar este metodo una vez");
            }

            Logger.LogDebug("Realizando Inicializacion .."); 
            // Personas por defecto
            Persona persona = new Persona(); 
            persona.Nombre = "Diego"; 
            persona.Apellido = "Urrutia"; 
            
            // Agrego la persona al backend
            this.AddPersona(persona); 

            foreach (Persona p in BackendContext.Personas) {
                Logger.LogDebug("Persona: {0}", p); 
            }

            for (int i = 0; i < 5; i++) {
                Publicacion publicacion = new Publicacion(); 
                publicacion.Doi = ""+i; 
                publicacion.Titulo = "Titulo "+i; 
                publicacion.PaginaInicio = "1"; 
                publicacion.PaginaFinal = "1"; 
                publicacion.CantidadRechazos = "0"; 
                publicacion.NumeroDePagina = "1";
                this.AddPublicacion(publicacion);
            }

            Initialized = true;

            Logger.LogDebug("Inicializacion terminada :)");
        }
    }

}