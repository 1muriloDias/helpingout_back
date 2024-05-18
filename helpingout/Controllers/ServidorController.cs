using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MySqlConnector;
using System;
using System.Linq; // Importante para usar ToList()
using helpingout.Data;

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
        public async Task<IActionResult> Banco()
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
                        command.CommandText = @"INSERT INTO usuarios (id, nome, admin) VALUES (123, 'teobaldo', 0)";
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

        // Criar/Editar Post/Put
        [HttpPost]
        public async Task<IActionResult> CriarEditar(Usuario usuario)
        {
            if (usuario.Id == 0)
            {
                _context.Usuarios.Add(usuario);
            }
            else
            {
                var usuarioNoBD = await _context.Usuarios.FindAsync(usuario.Id);
                if (usuarioNoBD == null)
                {
                    return NotFound();
                }
                usuarioNoBD.Nome = usuario.Nome;
                // Atualize outras propriedades conforme necessário
            }
            await _context.SaveChangesAsync();
            return Ok(usuario);
        }

        // Pegar GET
        [HttpGet("{id}")]
        public async Task<IActionResult> Pegar(int id)
        {
            var result = await _context.Usuarios.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // Deletar
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var result = await _context.Usuarios.FindAsync(id);

            if (result == null)
            {
                return NotFound();
            }
            _context.Usuarios.Remove(result);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Pegar todos os dados
        [HttpGet("GetAll")]
        public IActionResult Todos()
        {
            var result = _context.Usuarios.ToList();
            return Ok(result);
        }
    }
}
