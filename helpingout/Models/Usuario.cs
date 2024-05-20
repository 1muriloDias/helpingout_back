using Microsoft.AspNetCore.Identity;

namespace helpingout.Models
{
    public class Usuario
    {
        public string email { get; set; }
        public string nome { get; set; }
        public string senha { get; set; }
        public string cpf { get; set; }
        public string endereco { get; set; }
        public string telefone { get; set; }
        public string datanascimento { get; set; }
        public string curso { get; set; }
        public string professor { get; set; }
        public string materia { get; set; }
        public int id_usuario { get; set; }



    }
}
