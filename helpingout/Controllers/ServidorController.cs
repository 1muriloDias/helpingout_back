using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MySqlConnector;
using System;
using System.Linq; // Importante para usar ToList()
using helpingout.Data;
using helpingout.Models;

namespace helpingout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServidorController : ControllerBase
    {
        private readonly ApiContext _context;

        public ServidorController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet("Mysql")]
        public async Task<IActionResult> Chamada()
        {
            try
            {
                var builder = new MySqlConnectionStringBuilder
                {
                    Server = "helpingout.mysql.database.azure.com",
                    Database = "helpingout",
                    UserID = "adminHelpingOut",
                    Password = "Helpingout10$",
                    SslMode = MySqlSslMode.Required,
                };

                using (var conn = new MySqlConnection(builder.ConnectionString))
                {
                    await conn.OpenAsync();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO usuarios (id, nome, admin) VALUES (123, 'murilindo', 0)";
                        int rowCount = await command.ExecuteNonQueryAsync();
                        // Log de inserção, se necessário
                        // _logger.LogInformation($"Number of rows inserted: {rowCount}");
                    }
                }
                return Ok("Inserção realizada com sucesso.");
            }
            catch (Exception ex)
            {
                // Log de erro, se necessário
                // _logger.LogError($"Error accessing MySQL: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao acessar MySQL: {ex.Message}");
            }
        }


        //Criar/Editar Post/Put
        [HttpPost]
        public JsonResult CriarEditar(Usuario usuario)
        {
            if (usuario.id_usuario == 0)
            {
                _context.usuarios.Add(usuario);
            }
            else
            {
                var usuarioNoBD = _context.usuarios.Find(usuario.id_usuario);
                if (usuarioNoBD == null)
                {
                    return new JsonResult(NotFound());
                }
                usuarioNoBD = usuario;
            }
            _context.SaveChanges();
            return new JsonResult(Ok(usuario));
        }
        //Pegar GET
        [HttpGet]
        public JsonResult Pegar(int id)
        {
            var result = _context.usuarios.Find(id);
            if (result == null)
            {
                return new JsonResult(NotFound());
            }
            return new JsonResult(Ok(result));
        }

        //Deletar
        [HttpDelete]
        public JsonResult Deletar(int id)
        {
            var result = _context.usuarios.Find(id);

            if (result == null)
            {
                return new JsonResult(NotFound());
            }
            _context.usuarios.Remove(result);
            _context.SaveChanges();
            return new JsonResult(NoContent());
        }

        //Pegar todos os dados
        [HttpGet("/GetAll")]
        public JsonResult Todos()
        {
            var result = _context.usuarios.ToList();
            return new JsonResult(Ok(result));
        }


        //Pegar todos os dados
        [HttpGet("/Mysql")]
        public async Task<IActionResult> Banco()
        {
            try
            {
                await Chamada();
                return Ok("Chamada realizada com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao realizar a chamada: {ex.Message}");
            }
        }

      

    }
}
