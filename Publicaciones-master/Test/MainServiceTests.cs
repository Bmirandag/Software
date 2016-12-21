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

        public void getPublicacionesByRutTest(){
            Logger.LogInformation("Testing IMainService.getPublicacionesByRut(string rut) ..");
            Service.Initialize();
            
            //Crear persona
            Persona persona = new Persona();
            persona.Rut = "18-2";
            persona.Nombre = "Juan";
            persona.Apellido ="Calderon";
            persona.Email = "Juan@hotmail.cl";

            Persona persona1 = new Persona();
            persona1.Rut = "18-1";
            persona1.Nombre = "Alfredox";
            persona1.Apellido ="Rodriguez";
            persona1.Email = "alRo@hotmail.cl";

            //insertamos en el backend
            Service.AddPersona(persona);
            Service.AddPersona(persona1);

            //Crear las publicaciones (ya que los paper se asumen como "ACEPTADOS")
            Publicacion publicacion = new Publicacion();
            publicacion.Titulo = "Aplicaciones Remotas";
            publicacion.Volumen = 123;
            publicacion.PaginaInicio = 10;
            publicacion.PaginaFinal = 100;

            Publicacion publicacion1 = new Publicacion();
            publicacion1.Titulo = "Web Informáticas";
            publicacion1.Volumen = 111;
            publicacion1.PaginaInicio = 30;
            publicacion1.PaginaFinal = 131;

            Publicacion publicacion2 = new Publicacion();
            publicacion2.Titulo = "Compiladores";
            publicacion2.Volumen = 12;
            publicacion2.PaginaInicio = 110;
            publicacion2.PaginaFinal = 1030;

            //insertamos en el backend
            Service.AddPublicacion(publicacion);
            Service.AddPublicacion(publicacion1);
            Service.AddPublicacion(publicacion2);

            //Crear Autor
            Autor autor = new Autor();
            autor.persona = persona;
            autor.fecha = "22-10-2016";
            autor.tipo = tipo.PRINCIPAL;
            autor.publicacion = publicacion;

            Autor autor1 = new Autor();
            autor1.persona = persona;
            autor1.fecha = "24-10-2016";
            autor1.tipo = tipo.CORRESPONDIENTE;
            autor1.publicacion = publicacion1;

            Autor autor2 = new Autor();
            autor2.persona = persona;
            autor2.fecha = "25-10-2016";
            autor2.tipo = tipo.PRINCIPAL;
            autor2.publicacion = publicacion2;
            
            //insertamos en el backend
            Service.AddAutor(autor);
            Service.AddAutor(autor1);

            //Crear Paper
            Paper paper = new Paper();
            paper.Titulo = "Aplicaciones Remotas";
            paper.estado = estado.ACEPTADO;
            paper.autores = new List<Autor>();

            Paper paper1 = new Paper();
            paper1.Titulo = "Web Informáticas";
            paper1.estado = estado.ACEPTADO;
            paper1.autores = new List<Autor>();
            
            Paper paper2 = new Paper();
            paper2.Titulo = "Compiladores";
            paper2.estado = estado.ACEPTADO;
            paper2.autores = new List<Autor>();
            
            //Designamos las publicaciones a los papers
            paper.publicacion = publicacion;
            paper1.publicacion = publicacion1;
            paper2.publicacion = publicacion2;

            //insertamos los paper en el backend
            Service.AddPaper(paper);
            Service.AddPaper(paper1);
            Service.AddPaper(paper2);

            //agregamos el autor al paper
            Service.AddAutorToPaper(paper.IdentificadorPaper, autor);
            Service.AddAutorToPaper(paper1.IdentificadorPaper, autor);
            Service.AddAutorToPaper(paper2.IdentificadorPaper, autor);       

        }

        void IDisposable.Dispose()
        {
            // Aca eliminar el Service
        }
    }

}