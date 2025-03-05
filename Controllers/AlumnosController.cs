using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using MiApi.Models;
using MiApi.Services;
using MiApi.DTO;

namespace MiApi.Controllers
{
    [EnableCors("Cors")]
    [Route("[controller]")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly AlumnoService _alumnoService;

        public AlumnosController(AlumnoService alumnoService)
        {
            _alumnoService = alumnoService;
        }

        /// <summary>
        /// Obtener todos los alumnos 
        /// </summary>
        /// <returns>Lista de alumnos</returns>
        /// <response code="200">Éxito</response>
        /// <response code="500">Error en la verificación</response>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseBase<List<Alumno>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseBase), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ObtenerAlumnos()
        {
            ResponseBase<List<Alumno>> response = new ResponseBase<List<Alumno>>();
            try
            {
                var alumnos = await _alumnoService.ObtenerAlumnosAsync();
                response.Success = true;
                response.Message = "Lista de alumnos obtenida exitosamente.";
                response.Data = alumnos;
            }
            catch (Exception ex)
            {
                response.Message = $"Error en la consulta: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Crear un nuevo alumno 
        /// </summary>
        /// <param name="alumno">Datos del alumno</param>
        /// <returns>Alumno insertado</returns>
        /// <response code="201">Alumno creado correctamente</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="500">Error en el servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseBase<Alumno>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ResponseBase), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseBase), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> InsertarAlumno([FromBody] AlumnoDTO alumnoDTO)
        {
            ResponseBase<Alumno> response = new ResponseBase<Alumno>();
            try
            {
                if (alumnoDTO == null)
                {
                    response.Success = false;
                    response.Message = "Datos inválidos.";
                    return BadRequest(response);
                }

                // Convertir AlumnoDTO a Alumno
                var alumno = new Alumno
                {
                    Nombre = alumnoDTO.Nombre,
                    PrimerApellido = alumnoDTO.PrimerApellido,
                    SegundoApellido = alumnoDTO.SegundoApellido,
                    Matricula = alumnoDTO.Matricula,
                    Correo = alumnoDTO.Correo
                };

                var nuevoAlumno = await _alumnoService.InsertarAlumnoAsync(alumno);
                response.Success = true;
                response.Message = "Alumno insertado correctamente.";
                response.Data = nuevoAlumno;
            }
            catch (Exception ex)
            {
                response.Message = $"Error al insertar alumno: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return CreatedAtAction(nameof(InsertarAlumno), new { id = response.Data.Id }, response);
        }

        /// <summary>
        /// Actualizar un alumno por su ID
        /// </summary>
        /// <param name="id">ID del alumno</param>
        /// <param name="alumno">Datos actualizados del alumno</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="200">Alumno actualizado correctamente</response>
        /// <response code="404">Alumno no encontrado</response>
        /// <response code="400">Datos inválidos</response>
        /// <response code="500">Error en el servidor</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseBase), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseBase), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseBase), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ActualizarAlumno(string id, [FromBody] AlumnoDTO alumnoDTO)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                if (string.IsNullOrEmpty(id) || alumnoDTO == null)
                {
                    response.Success = false;
                    response.Message = "Datos inválidos.";
                    return BadRequest(response);
                }

                var alumnoActualizado = new Alumno
                {
                    Id = id, // Mantener el Id de la URL
                    Nombre = alumnoDTO.Nombre,
                    PrimerApellido = alumnoDTO.PrimerApellido,
                    SegundoApellido = alumnoDTO.SegundoApellido,
                    Matricula = alumnoDTO.Matricula,
                    Correo = alumnoDTO.Correo
                };

                bool actualizado = await _alumnoService.ActualizarAlumnoAsync(id, alumnoActualizado);
                if (!actualizado)
                {
                    response.Success = false;
                    response.Message = "Alumno no encontrado.";
                    return NotFound(response);
                }

                response.Success = true;
                response.Message = "Alumno actualizado correctamente.";
            }
            catch (Exception ex)
            {
                response.Message = $"Error al actualizar el alumno: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Eliminar un alumno por su ID
        /// </summary>
        /// <param name="id">ID del alumno</param>
        /// <returns>Resultado de la operación</returns>
        /// <response code="200">Alumno eliminado correctamente</response>
        /// <response code="404">Alumno no encontrado</response>
        /// <response code="400">ID inválido</response>
        /// <response code="500">Error en el servidor</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseBase), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseBase), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseBase), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseBase), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> EliminarAlumno(string id)
        {
            ResponseBase response = new ResponseBase();
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    response.Success = false;
                    response.Message = "ID inválido.";
                    return BadRequest(response);
                }

                bool eliminado = await _alumnoService.EliminarAlumnoPorId(id);
                if (!eliminado)
                {
                    response.Success = false;
                    response.Message = "Alumno no encontrado.";
                    return NotFound(response);
                }

                response.Success = true;
                response.Message = "Alumno eliminado correctamente.";
            }
            catch (Exception ex)
            {
                response.Message = $"Error al eliminar alumno: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Obtener un alumno por su ID
        /// </summary>
        /// <param name="id">ID del alumno</param>
        /// <returns>Alumno encontrado</returns>
        /// <response code="200">Éxito</response>
        /// <response code="404">Alumno no encontrado</response>
        /// <response code="400">ID inválido</response>
        /// <response code="500">Error en el servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseBase<Alumno>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseBase), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseBase), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseBase), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ObtenerAlumnoPorId(string id)
        {
            ResponseBase<Alumno> response = new ResponseBase<Alumno>();
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    response.Success = false;
                    response.Message = "ID inválido.";
                    return BadRequest(response);
                }

                var alumno = await _alumnoService.ObtenerAlumnoPorIdAsync(id);
                if (alumno == null)
                {
                    response.Success = false;
                    response.Message = "Alumno no encontrado.";
                    return NotFound(response);
                }

                response.Success = true;
                response.Message = "Alumno encontrado.";
                response.Data = alumno;
            }
            catch (Exception ex)
            {
                response.Message = $"Error al obtener el alumno: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }
    }
}