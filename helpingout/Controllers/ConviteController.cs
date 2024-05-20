using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder; // Adicione esta diretiva


using helpingout.Models;

public class QrCodeGenerator
{
    public string GenerateQrCode(string userId)
    {
        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        {
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(userId, QRCodeGenerator.ECCLevel.Q);
            using (SvgQRCode qrCode = new SvgQRCode(qrCodeData))
            {
                string svgQrCode = qrCode.GetGraphic(20);
                return "data:image/svg+xml;base64," + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(svgQrCode));
            }
        }
    }
}

[ApiController]
[Route("api/[controller]")]
public class ConviteController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public ConviteController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet("{userId}")]
    public IActionResult GetConvite(string userId)
    {
        var user = _usuarioService.ObterUsuarioPorIdAsync(userId).Result; // Obtenha o usuário por ID
        if (user == null)
        {
            return NotFound();
        }

        var convite = new Convite
        {
            id_convites = Guid.NewGuid().ToString(),
            statusCheckin = "Not Checked In",
            statusCheckout = "Not Checked Out",
            tema = "Default",
            formato = "Digital",
            local = "To be defined",
            data = DateTime.Now,
            id_evento = Guid.NewGuid().ToString(),
            id_usuario = userId,
            qrcode = new QrCodeGenerator().GenerateQrCode(userId)
        };

        return Ok(convite);
    }

    [HttpPost("checkin")]
    public IActionResult CheckInUser([FromBody] CheckInRequest request)
    {
        var user = _usuarioService.ObterUsuarioPorIdAsync(request.IdUsuario).Result; // Obtenha o usuário por ID
        if (user == null)
        {
            return NotFound();
        }

        // Lógica para fazer o check-in do usuário e adicionar à lista de nomes
        var checkInList = new List<string>(); // Isso deve ser armazenado em algum lugar persistente
        checkInList.Add(user.Nome); // Acessando o nome do usuário

        return Ok(checkInList);
    }
}