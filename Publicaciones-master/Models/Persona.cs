namespace Publicaciones.Models
{
    public class Persona
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Email{get; set; }

        public string FechaNacimiento{get; set;} 
        
        public string Nacionalidad{get; set;}
        
        public string Genero{get; set;}

        public List<Autor> Autor{get; set;}

    }

}