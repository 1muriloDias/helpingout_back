using Microsoft.AspNetCore.Mvc;
using helpingout.Models;
using helpingout.Data;
using System.Threading.Tasks;
using System.Linq;

namespace helpingout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly ApiContext _context;

        public EventoController(ApiContext context)
        {
            _context = context;
        }

        // POST: api/Evento
        [HttpPost]
        public async Task<IActionResult> PostEvento([FromBody] Evento evento)
        {
            if (evento == null)
            {
                return BadRequest();
            }

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvento), new { id = evento.IdEvento }, evento);
        }

        // GET: api/Evento/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            return Ok(evento);
        }

        // GET: api/Evento
        [HttpGet]
        public IActionResult GetEventos()
        {
            var eventos = _context.Eventos.ToList();
            return Ok(eventos);
        }
    }
}
