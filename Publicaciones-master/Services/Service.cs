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

        void AddGrado(Grado grado);

        void AddPaper(Paper paper);

        void AddPublicacion(Publicacion publicacion);

        void AddAutorToPaper(string IdentificadorPaper, Autor autor);

        List < Persona > FindPersonas(string nombre);

        List <Persona> Personas();

        List< Publicacion > Publicaciones();

        List< Publicacion > getPublicacionesByRut(string rut);

        List< Autor > getAutoresByRut(string rut);

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
         public void AddGrado(Grado grado){

            // Guardo el Grado en el Backend
            BackendContext.Grados.Add(grado); 

            // Guardo los cambios
            BackendContext.SaveChanges(); 
        }

        /// <summary>
        /// Servicio que agrega paper.
        /// </summary>
         public void AddPaper(Paper paper){

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
        public List< Publicacion > getPublicacionesByRut(string rut){
            List< Autor > autorias = this.getAutoresByRut(rut);
            List< Paper > paperPorAutor = this.getPaperByAutor(rut);
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
        public List< Autor > getAutoresByRut(string rut){
            return BackendContext.Autores
                    .Where(a => a.persona.Rut.Contains(rut))
                    .ToList();
        }

        /// <summary>
        /// Servicio que retorna los Papers por Autor.
        /// </summary>
        /// <param name="rut"></param>
        /// <returns>Lista de Paper</returns> 
        public List< Paper > getPaperByAutor(string rut){
            List< Autor > autorias = this.getAutoresByRut(rut);
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

            Persona persona1 = new Persona(); 
            persona1.Nombre = "Diego"; 
            persona1.Apellido = "Urrutia"; 

            Persona persona2 = new Persona(); 
            persona2.Nombre = "Diego"; 
            persona2.Apellido = "Urrutia"; 

            Persona persona3 = new Persona(); 
            persona3.Nombre = "Diego"; 
            persona3.Apellido = "Urrutia"; 
            
            // Agrego la persona al backend
            this.AddPersona(persona); 
            this.AddPersona(persona1); 
            this.AddPersona(persona2); 
            this.AddPersona(persona3); 

            // Papers por defecto
            Paper paper = new Paper();
            paper.Titulo = "Paper";
            Paper paper1 = new Paper();
            paper1.Titulo = "Paper1";
            Paper paper = new Paper();
            paper2.Titulo = "Paper2";
            Paper paper = new Paper();
            paper3.Titulo = "Paper3";

            // Agrego los papers al backend
            this.AddPaper(paper);
            this.AddPaper(paper1);
            this.AddPaper(paper2);
            this.AddPaper(paper3);

            // Grados asociados a la persona
            Grado grado = new Grado();
            grado.Nombre = "Magister";
            grado.Fecha = "10-12-2016"
            grado.Rut = persona.Rut;

            Grado grado1 = new Grado();
            grado1.Nombre = "Doctor";
            grado1.Fecha = "13-12-2016"
            grado1.Rut = persona1.Rut;

            Grado grado2 = new Grado();
            grado2.Nombre = "Magister en minas";
            grado2.Fecha = "12-21-2016"
            grado2.Rut = persona2.Rut;

            Grado grado3 = new Grado();
            grado3.Nombre = "Doctorado en Geología";
            grado3.Fecha = "12-21-2016"
            grado3.Rut = persona3.Rut;

            Grado grado4 = new Grado();
            grado4.Nombre = "Magister en Minas";
            grado4.Fecha = "12-21-2016"
            grado4.Rut = persona3.Rut;

            // Agrego los paper al backend
            this.AddGrado(grado1);
            this.AddGrado(grado2);
            this.AddGrado(grado3);
            this.AddGrado(grado4);
            this.AddGrado(grado5);

            foreach (Persona p in BackendContext.Personas) {
                Logger.LogDebug("Persona: {0}", p); 
            }

            
            Initialized = true;

            Logger.LogDebug("Inicializacion terminada :)");
        }
    }

}