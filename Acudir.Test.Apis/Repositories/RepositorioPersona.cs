using Acudir.Test.Apis.Domain;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Acudir.Test.Apis.Repositories
{
    public class RepositorioPersona : IRepositorioPersona
    {
        private String directorio = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        public void Agregar(Persona persona)
        {
            var personas = LeerPersonas();
            personas.Add(persona);
            EscribirPersonas(personas);
        }

        public void Eliminar(string nombreCompleto)
        {
            var personas = LeerPersonas();
            personas.RemoveAll(p => p.NombreCompleto == nombreCompleto);
            EscribirPersonas(personas);
        }

        public void Modificar(Persona persona)
        {
            var personas = LeerPersonas();
            var personaExistente = personas.FirstOrDefault(p => p.NombreCompleto == persona.NombreCompleto);

            if (personaExistente != null)
            {
                personaExistente.Edad = persona.Edad;
                personaExistente.Domicilio = persona.Domicilio;
                personaExistente.Telefono = persona.Telefono;
                personaExistente.Profesion = persona.Profesion;

                EscribirPersonas(personas);
            }
        }

        public Persona ObtenerPorNombreCompleto(string nombreCompleto)
        {
            var personas = LeerPersonas();
            return personas.FirstOrDefault(p => p.NombreCompleto == nombreCompleto);
        }

        public List<Persona> ObtenerTodos()
        {
            return LeerPersonas();
        }

        private List<Persona> LeerPersonas()
        {
            String archivoJson = directorio.Replace("bin\\Debug\\net6.0", "Test.json");
            if (!File.Exists(archivoJson))
            {
                return new List<Persona>();
            }

            using (var lector = new StreamReader(archivoJson))
            {
                var json = lector.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Persona>>(json);
            }
        }

        private void EscribirPersonas(List<Persona> personas)
        {
            String archivoJson = directorio.Replace("bin\\Debug\\net6.0", "Test.json");
            if (File.Exists(archivoJson)) 
            { 
                using (var escritor = new StreamWriter(archivoJson))
                {
                    var json = JsonConvert.SerializeObject(personas, Newtonsoft.Json.Formatting.Indented);
                    escritor.Write(json);
                }
            }
        }
    }
}
