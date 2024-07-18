using Acudir.Test.Apis.Domain;

namespace Acudir.Test.Apis.Repositories
{
    public interface IRepositorioPersona
    {
        void Agregar(Persona persona);
        void Eliminar(string nombreCompleto);
        void Modificar(Persona persona);
        Persona ObtenerPorNombreCompleto(string nombreCompleto);
        List<Persona> ObtenerTodos();
    }
}
