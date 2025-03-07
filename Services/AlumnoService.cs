﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MiApi.Models;
using MiApi.Data;
using MongoDB.Bson;

namespace MiApi.Services
{
    public class AlumnoService
    {
        private readonly IMongoCollection<Alumno> _alumnos;

        public AlumnoService(IOptions<MongoDbConfig> config)
        {
            var client = new MongoClient(config.Value.ConnectionString);
            var database = client.GetDatabase(config.Value.DatabaseName);
            _alumnos = database.GetCollection<Alumno>(config.Value.CollectionName);
        }

        // Método para obtener todos los alumnos almacenados en la base de datos.
        public async Task<List<Alumno>> ObtenerAlumnosAsync() =>
            await _alumnos.Find(alumno => true).ToListAsync();

        // Método para insertar un nuevo alumno en la base de datos.
        public async Task<Alumno> InsertarAlumnoAsync(Alumno alumno)
        {
            alumno.Id = ObjectId.GenerateNewId().ToString(); 
            await _alumnos.InsertOneAsync(alumno);
            return alumno;
        }

        // Método para actualizar un alumno existente por su ID.
        public async Task<bool> ActualizarAlumnoAsync(string id, Alumno alumnoActualizado)
        {
            var filter = Builders<Alumno>.Filter.Eq(a => a.Id, id); 
            var update = Builders<Alumno>.Update
                .Set(a => a.Nombre, alumnoActualizado.Nombre)
                .Set(a => a.PrimerApellido, alumnoActualizado.PrimerApellido)
                .Set(a => a.SegundoApellido, alumnoActualizado.SegundoApellido)
                .Set(a => a.Matricula, alumnoActualizado.Matricula)
                .Set(a => a.Correo, alumnoActualizado.Correo);

            var result = await _alumnos.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0; 
        }

        // Método para eliminar un alumno por su ID.
        public async Task<bool> EliminarAlumnoPorId(string id)
        {
            var filter = Builders<Alumno>.Filter.Eq(a => a.Id, id); 
            var result = await _alumnos.DeleteOneAsync(filter);
            return result.DeletedCount > 0; 
        }

        // Método para obtener un alumno por su ID.
        public async Task<Alumno> ObtenerAlumnoPorIdAsync(string id)
        {
            var filter = Builders<Alumno>.Filter.Eq(a => a.Id, id); 
            return await _alumnos.Find(filter).FirstOrDefaultAsync(); 
        }
    }
}
