using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

namespace Bh.AppWeb.Pages
{
    public class ContactoModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ContactoModel> _logger;

        public ContactoModel(IConfiguration configuration, ILogger<ContactoModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [BindProperty]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Por favor, introduce un email válido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [BindProperty]
        [Display(Name = "Asunto")]
        public string? Asunto { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El mensaje es obligatorio")]
        [Display(Name = "Mensaje")]
        public string Mensaje { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Por favor, responde a la pregunta de seguridad")]
        [Display(Name = "Respuesta")]
        public string RespuestaSeguridad { get; set; } = string.Empty;

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Validar respuesta de seguridad (día de la semana = viernes)
            if (!RespuestaSeguridad.Trim().Equals("viernes", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(RespuestaSeguridad), "Respuesta incorrecta. Por favor, intenta de nuevo.");
                return Page();
            }

            try
            {
                await EnviarEmailAsync();
                SuccessMessage = "¡Gracias! Tu mensaje ha sido enviado correctamente. Te responderemos lo antes posible.";
                
                // Limpiar formulario
                ModelState.Clear();
                Nombre = string.Empty;
                Email = string.Empty;
                Telefono = null;
                Asunto = null;
                Mensaje = string.Empty;
                RespuestaSeguridad = string.Empty;
                
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar email de contacto");
                ErrorMessage = "Ha ocurrido un error al enviar tu mensaje. Por favor, inténtalo de nuevo más tarde o envía un email directamente a albert.capdevila@bahiacode.com";
                return Page();
            }
        }

        private async Task EnviarEmailAsync()
        {
            var smtpHost = _configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var smtpUser = _configuration["Email:SmtpUser"] ?? "";
            var smtpPassword = _configuration["Email:SmtpPassword"] ?? "";
            var destinatario = "albert.capdevila@bahiacode.com";

            // Si no hay configuración SMTP, simular envío exitoso en desarrollo
            if (string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPassword))
            {
                _logger.LogWarning("No hay configuración SMTP. Simulando envío de email en desarrollo.");
                _logger.LogInformation("Email simulado - De: {Email}, Nombre: {Nombre}, Asunto: {Asunto}, Mensaje: {Mensaje}", 
                    Email, Nombre, Asunto ?? "Sin asunto", Mensaje);
                await Task.CompletedTask;
                return;
            }

            var mensaje = new MailMessage
            {
                From = new MailAddress(smtpUser, "Formulario Web Bahía Code"),
                Subject = $"Contacto Web: {Asunto ?? "Sin asunto"}",
                Body = $@"
<html>
<body>
    <h2>Nuevo mensaje de contacto desde la web</h2>
    <p><strong>Nombre:</strong> {Nombre}</p>
    <p><strong>Email:</strong> {Email}</p>
    <p><strong>Teléfono:</strong> {Telefono ?? "No proporcionado"}</p>
    <p><strong>Asunto:</strong> {Asunto ?? "Sin asunto"}</p>
    <hr>
    <p><strong>Mensaje:</strong></p>
    <p>{Mensaje.Replace("\n", "<br>")}</p>
</body>
</html>",
                IsBodyHtml = true
            };

            mensaje.To.Add(destinatario);
            mensaje.ReplyToList.Add(new MailAddress(Email, Nombre));

            using var smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUser, smtpPassword)
            };

            await smtpClient.SendMailAsync(mensaje);
        }
    }
}
