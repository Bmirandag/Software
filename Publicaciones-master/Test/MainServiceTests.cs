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
        public void ObtenerPublicacionesTest(){

        Logger.LogInformation("Testing IMainService.Publicaciones() ..");
         List<Publicacion> publicaciones = Service.Publicaciones();
            // Debe ser !=  de null
            Assert.True(publicaciones != null);

            // Debe retornar vacia
            Assert.True(publicaciones.Count == 0);
        
         for (int i = 0; i < 5; i++) {
            Publicacion publicacion = new Publicacion(); 
            publicacion.Doi = ""+i; 
            publicacion.Titulo = "Titulo "+i; 
            publicacion.PaginaInicio = "1"; 
            publicacion.PaginaFinal = "1"; 
            publicacion.CantidadRechazos = "0"; 
            publicacion.NumeroDePagina = "1";
            Service.AddPublicacion(publicacion);
            }

         Assert.True(publicaciones.Count == 5);

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

            // Debe haber solo 1
            Assert.True(personas.Count == 1);

            // Print de la persona
            foreach(Persona persona in personas) {
                Logger.LogInformation("Persona: {0}", persona);
            }

            Logger.LogInformation("Test IMainService.Initialize() ok");
        }

        void IDisposable.Dispose()
        {
            // Aca eliminar el Service
        }
    }

}