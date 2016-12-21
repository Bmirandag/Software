using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Publicaciones.Backend;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Publicaciones.Models;

namespace Publicaciones.Service {

    public class MainServiceTest : IDisposable
    {
        IMainService Service { get; set; }

        ILogger Logger { get; set; }

        public MainServiceTest()
        {
            // Logger Factory
            ILoggerFactory loggerFactory = new LoggerFactory().AddConsole().AddDebug();
            Logger = loggerFactory.CreateLogger<MainServiceTest>();

            Logger.LogInformation("Initializing Test ..");

            // SQLite en memoria
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            // Opciones de inicializacion del BackendContext
            var options = new DbContextOptionsBuilder<BackendContext>()
            .UseSqlite(connection)
            .Options;

            // inicializacion del BackendContext
            BackendContext backend = new BackendContext(options);
            // Crear la base de datos
            backend.Database.EnsureCreated();

            // Servicio a probar
            Service = new MainService(backend, loggerFactory);

            Logger.LogInformation("Initialize Test ok.");
        }

        [Fact]
        public void InitializeTest()
        {
            Logger.LogInformation("Testing IMainService.Initialize() ..");
            Service.Initialize();

            // No se puede inicializar 2 veces
            Assert.Throws<Exception>( () => { Service.Initialize(); });

            // Personas en la BD
            List<Persona> personas = Service.Personas();

            // Debe ser !=  de null
            Assert.True(personas != null);

            // Debe haber solo 1 persona
            Assert.True(personas.Count == 1);

            // Print de la persona
            foreach(Persona persona in personas) {
                Logger.LogInformation("Persona: {0}", persona);
            }

            Logger.LogInformation("Test IMainService.Initialize() ok");
        }

        [Fact]
        public void FindPersonasTest(){
            Logger.LogInformation("Testing IMainService.FindPersonas(string nombre) ..");
            Service.Initialize();

            // Crear persona
            Persona persona = new Persona();
            persona.Rut = "18501813-k";
            persona.Nombre = "Alfredox";
            persona.Email = "amg005@hotmail.cl";

            Persona persona1 = new Persona();
            persona1.Rut = "1777777-3";
            persona1.Nombre = "Alfredox";
            persona1.Email = "alfredo@hotmail.cl";

            //insertamos en el backend
            Service.AddPersona(persona);
            Service.AddPersona(persona1);

            //Obtener desde el backend
            List<Persona> personasbd = Service.FindPersonas("Alfredox");

            // Debe ser !=  de null
            Assert.True(personasbd != null);
            // Debe haber 2 personas con el nombre "Alfredox"
            Assert.True(personasbd.Count == 2);

            Logger.LogInformation("Test IMainService.FindPersonas(string nombre) ok");
           
        }
        [Fact]
        public void getPublicacionesByRutTest(){
            Logger.LogInformation("Testing IMainService.getPublicacionesByRut(string rut) ..");
            Service.Initialize();
            
            //Crear persona, cuenta de 4 Autorias de 3 Papers Aceptados y 1 en Espera
            Persona persona = new Persona();
            persona.Rut = "18-2";
            persona.Nombre = "Juan";
            persona.Apellido ="Calderon";
            persona.Email = "Juan@hotmail.cl";

            //insertamos en el backend
            Service.AddPersona(persona);
    
        
            //Crear las publicaciones (ya que los paper se asumen como "ACEPTADOS")
            Publicacion publicacion = new Publicacion();
            publicacion.Titulo = "Aplicaciones Remotas";
            publicacion.Volumen = "123";
            publicacion.PaginaInicio = "10";
            publicacion.PaginaFinal = "100";

            Publicacion publicacion1 = new Publicacion();
            publicacion1.Titulo = "Web Informáticas";
            publicacion1.Volumen = "111";
            publicacion1.PaginaInicio = "30";
            publicacion1.PaginaFinal = "131";

            Publicacion publicacion2 = new Publicacion();
            publicacion2.Titulo = "Compiladores";
            publicacion2.Volumen = "12";
            publicacion2.PaginaInicio = "110";
            publicacion2.PaginaFinal = "1030";

            //insertamos en el backend
            Service.AddPublicacion(publicacion);
            Service.AddPublicacion(publicacion1);
            Service.AddPublicacion(publicacion2);
            
            //Obtener desde el backend
            List<Publicacion> publicaciones = Service.Publicaciones();
            // Debe ser !=  de null
            Assert.True(publicaciones != null);
            // Debe haber 3 Publicaciones
            Assert.True(publicaciones.Count == 3);
            //imprimir las publicaciones
            foreach(Publicacion publicacions in publicaciones) {
                Logger.LogInformation("Publicacion: {0}", publicacions.Titulo);
            }
           
            //Crear Paper
            Paper paper = new Paper();
            paper.Titulo = "Aplicaciones Remotas";
            paper.estado = Estado.ACEPTADO;
            paper.Autores = new List<Autor>();

            Paper paper1 = new Paper();
            paper1.Titulo = "Web Informáticas";
            paper1.estado = Estado.ACEPTADO;
            paper1.Autores = new List<Autor>();
            
            Paper paper2 = new Paper();
            paper2.Titulo = "Compiladores";
            paper2.estado = Estado.ACEPTADO;
            paper2.Autores = new List<Autor>();

            //Designamos las publicaciones a los papers
            paper.publicacion = publicacion;
            paper1.publicacion = publicacion1;
            paper2.publicacion = publicacion2;

            //insertamos los paper en el backend
            Service.AddPaper(paper);
            Service.AddPaper(paper1);
            Service.AddPaper(paper2);
           
            //Crear Autor
            Autor autor = new Autor();
            autor.persona = persona;
            autor.Fecha = "22-10-2016";
            autor.tipo = Tipo.PRINCIPAL;
            autor.paper = paper;

            Autor autor1 = new Autor();
            autor1.persona = persona;
            autor1.Fecha = "24-10-2016";
            autor1.tipo = Tipo.CORRESPONDIENTE;
            autor1.paper = paper1;

            Autor autor2 = new Autor();
            autor2.persona = persona;
            autor2.Fecha = "25-10-2016";
            autor2.tipo = Tipo.PRINCIPAL;
            autor2.paper = paper2;
          
            //insertamos en el backend
            Service.AddAutor(autor);
            Service.AddAutor(autor1);
            Service.AddAutor(autor2);

            //Obtenemos del backend
            List<Autor> autoresbd = Service.Autores();
            //Debe ser distinto de null
            Assert.True(autoresbd != null);
            //Debe haber 3 autores, es el mismo de rut 18-2 pero con diferentes fechas de autoria
            Assert.True(autoresbd.Count == 3);
            //Imprimir autores
            foreach(Autor autorr in autoresbd) {
                Logger.LogInformation("Autor: {0}", autorr.Rut);
            }

            //agregamos el autor al paper
            Service.AddAutorToPaper(paper.IdentificadorPaper, autor);
            Service.AddAutorToPaper(paper1.IdentificadorPaper, autor);
            Service.AddAutorToPaper(paper2.IdentificadorPaper, autor);
           
            //Obtenemos del backend
            List<Paper> papers = Service.getPaperByAutor("18-2");
            //Debe ser distinto de null
            Assert.True(papers != null);
            //Deben haber 3 paper
            Assert.True(papers.Count == 3);
            // Print de los papers
            foreach(Paper papert in papers) {
                Logger.LogInformation("Paper: {0}", paper.Titulo);
            }

            //Buscar publicaciones por rut
            List<Publicacion> publicacionesrutbd = Service.getPublicacionesByRut("18-2");

            Assert.True(publicacionesrutbd != null);
            Assert.True(publicacionesrutbd.Count == 3);

            foreach(Publicacion publicacions in publicacionesrutbd) {
                Logger.LogInformation("Paper: {0}", publicacions.Titulo);
            }

            Logger.LogInformation("Test IMainService.getPublicacionesByRut(string rut).. ok");

        }
        

        void IDisposable.Dispose()
        {
            // Aca eliminar el Service
        }
    }

}