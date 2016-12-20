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
        void AddPublicacion(Publicacion publicacion);

        List< Publicacion > Publicaciones();

        void AddPersona(Persona persona); 

        List < Persona > FindPersonas(string nombre);

        List <Persona> Personas();


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
        /// Servicio que agrega publicaciones.
        /// </summary>
        public void AddPublicacion(Publicacion publicacion) {

            // Guardo la Publicacion en el Backend
            BackendContext.Publicaciones.Add(publicacion); 

            // Guardo los cambios
            BackendContext.SaveChanges(); 
        }
        /// <summary>
        /// Servicio que retorna publicaciones.
        /// </summary>
        /// <returns>Lista de Publicaciones</returns>  
         public List< Publicacion > Publicaciones() {
            return BackendContext.Publicaciones.ToList();
        }


        public void AddPersona(Persona persona) {

            // Guardo la Persona en el Backend
            BackendContext.Personas.Add(persona); 

            // Guardo los cambios
            BackendContext.SaveChanges(); 
        }

  
        public List < Persona > FindPersonas(string nombre) {
            return BackendContext.Personas
                .Where(p => p.Nombre.Contains(nombre))
                .OrderBy(p => p.Nombre)
                .ToList(); 
        }

        public List<Persona> Personas() {
            return BackendContext.Personas.ToList();
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
            persona1.Nombre = "David"; 
            persona1.Apellido = "Meza";
            
            Persona persona2 = new Persona(); 
            persona2.Nombre = "Rodrigo"; 
            persona2.Apellido = "Contreras";
            
            Persona persona3 = new Persona(); 
            persona3.Nombre = "Bryan"; 
            persona3.Apellido = "Miranda";
            
            Persona persona4 = new Persona(); 
            persona4.Nombre = "Lorena"; 
            persona4.Apellido = "Aguilera";

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