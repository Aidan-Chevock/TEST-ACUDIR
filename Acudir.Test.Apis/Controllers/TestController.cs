 using Acudir.Test.Apis.Domain;
 using Acudir.Test.Apis.Repositories;
 using Microsoft.AspNetCore.Mvc;

namespace Acudir.Test.Apis.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IRepositorioPersona _repositorioPersona;

        public TestController(IRepositorioPersona repositorioPersona)
        {
            _repositorioPersona = repositorioPersona;
        }

        //Devolver una lista que retorne Personas
        [HttpGet("GetAll")]
        public ActionResult<List<Persona>> GetAll(
            [FromQuery] string? nombreCompleto = null,
            [FromQuery] int? edad = null,
            [FromQuery] string? domicilio = null,
            [FromQuery] string? telefono = null,
            [FromQuery] string? profesion = null)
        {
            var personas = _repositorioPersona.ObtenerTodos();

            // Filtrar por nombre completo
            if (!string.IsNullOrEmpty(nombreCompleto))
            {
                personas = personas.Where(p => p.NombreCompleto.Contains(nombreCompleto)).ToList();
            }

            // Filtrar por edad
            if (edad.HasValue)
            {
                personas = personas.Where(p => p.Edad == edad).ToList();
            }

            // Filtrar por domicilio
            if (!string.IsNullOrEmpty(domicilio))
            {
                personas = personas.Where(p => p.Domicilio.Contains(domicilio)).ToList();
            }

            // Filtrar por telefono
            if (!string.IsNullOrEmpty(telefono))
            {
                personas = personas.Where(p => p.Telefono.Contains(telefono)).ToList();
            }

            // Filtrar por profesion
            if (!string.IsNullOrEmpty(profesion))
            {
                personas = personas.Where(p => p.Profesion.Contains(profesion)).ToList();
            }

            return Ok(personas);
        }

        //Post Guardar una Persona o mas
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<Persona> personas)
        {
            if (personas == null || personas.Count == 0)
            {
                return BadRequest("Se debe proporcionar al menos una persona.");
            }

            foreach (var persona in personas)
            {
                if (persona == null)
                {
                    return BadRequest("Se encontró una persona nula en la lista.");
                }

                _repositorioPersona.Agregar(persona);
            }

            return CreatedAtRoute("default", new { }, personas);
        }

        //Put Modificarlas
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Persona persona)
        {
            if (persona == null)
            {
                return BadRequest("Se debe proporcionar un objeto Persona.");
            }

            //En este caso utilizé el NombreCompleto como identificador unico de cada persona dado que es el dato que menos chances tiene de repetirse o cambiar pero no es una buena practica
            var personaExistente = _repositorioPersona.ObtenerPorNombreCompleto(persona.NombreCompleto);

            if (personaExistente == null)
            {
                return NotFound("No se encontró la persona con el nombre completo especificado.");
            }

            _repositorioPersona.Modificar(persona);

            return Ok(persona);
        }

        //Delete (este me tome la libertad de agregarlo aunque el ejercicio no lo pida)
        [HttpDelete("{nombreCompleto}")]
        public async Task<IActionResult> Delete([FromRoute] string nombreCompleto)
        {
            if (string.IsNullOrEmpty(nombreCompleto))
            {
                return BadRequest("Se debe proporcionar el nombre completo de la persona a eliminar.");
            }

            var personaExistente = _repositorioPersona.ObtenerPorNombreCompleto(nombreCompleto);


            if (personaExistente == null)
            {
                return NotFound("No se encontró la persona con el nombre completo especificado.");
            }

            _repositorioPersona.Eliminar(nombreCompleto);

            return Ok(personaExistente);
        }
    }
}
