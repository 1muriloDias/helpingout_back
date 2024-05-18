using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder; // Adicione esta diretiva

namespace helpingout.Controllers
{
    public class Convite
    {
        public string IdConvites { get; set; }
        public string StatusCheckin { get; set; }
        public string StatusCheckout { get; set; }
        public string Tema { get; set; }
        public string Formato { get; set; }
        public string QrCode { get; set; }
        public string Local { get; set; }
        public DateTime Data { get; set; }
        public string IdEvento { get; set; }
        public string IdUsuario { get; set; }
    }

    public class QrCodeGenerator
    {
        public string GenerateQrCode(string userId)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(userId, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrCodeData))
                {
                    using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ConviteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConviteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public IActionResult GetConvite(string userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }

            var convite = new Convite
            {
                IdConvites = Guid.NewGuid().ToString(),
                StatusCheckin = "Not Checked In",
                StatusCheckout = "Not Checked Out",
                Tema = "Default",
                Formato = "Digital",
                Local = "To be defined",
                Data = DateTime.Now,
                IdEvento = Guid.NewGuid().ToString(),
                IdUsuario = userId,
                QrCode = new QrCodeGenerator().GenerateQrCode(userId)
            };

            return Ok(convite);
        }

        public class CheckInRequest
        {
            public string UserId { get; set; }
        }

        [HttpPost("checkin")]
        public IActionResult CheckInUser([FromBody] CheckInRequest request)
        {
            var user = _context.Users.Find(request.UserId);
            if (user == null)
            {
                return NotFound();
            }

            // Lógica para fazer o check-in do usuário e adicionar à lista de nomes
            var checkInList = new List<string>(); // Isso deve ser armazenado em algum lugar persistente
            checkInList.Add(user.Name);

            return Ok(checkInList);
        }
    }

    // Simulação de contexto de banco de dados
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }

    // Simulação de entidade de usuário
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
