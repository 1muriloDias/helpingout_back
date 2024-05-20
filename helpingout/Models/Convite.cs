using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder; // Adicione esta diretiva


namespace helpingout.Models
{
    public class Convite : Usuario
    {
        public string id_convites { get; set; }
        public string statusCheckin { get; set; }
        public string statusCheckout { get; set; }
        public string tema { get; set; }
        public string formato { get; set; }
        public string qrcode { get; set; }
        public string local { get; set; }
        public DateTime data { get; set; }
        public string id_evento { get; set; }
        public string id_usuario { get; set; }
    }


}

   
