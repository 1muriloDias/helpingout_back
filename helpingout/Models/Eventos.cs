using System;
using System.ComponentModel.DataAnnotations;

namespace helpingout.Models
{
    public class Evento
    {
        public DateTime Data { get; set; }

        public string Local { get; set; }

        public TimeSpan Horario { get; set; }

        public string Nome { get; set; }

        [Key]
        public int IdEvento { get; set; }

        public string Descricao { get; set; }

        public int IdUsuario { get; set; }
    }
}