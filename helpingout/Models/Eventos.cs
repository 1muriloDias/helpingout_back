using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace helpingout.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private static List<eventos> eventosList = new List<eventos>();

        [HttpPost]
        public IActionResult CreateEvento([FromBody] eventos novoEvento)
        {
            novoEvento.id_evento = Guid.NewGuid().ToString();
            eventosList.Add(novoEvento);
            return Ok(novoEvento);
        }

        [HttpGet]
        public IActionResult GetEventos()
        {
            return Ok(eventosList);
        }
    }

    public class eventos
    {
        public DateTime data { get; set; }
        public string local { get; set; }
        public TimeOnly horario { get; set; }
        public string nome { get; set; }
        public string id_evento { get; set; }
        public string descricao { get; set; }
        public string id_usuario { get; set; }
    }
}

