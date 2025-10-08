using Amazon.Runtime;
using Amazon.SimpleEmail.Model;
using Bh.AppWeb.Servicios;
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
        private readonly AmazonSesEmailSender _emailServicio;

        public ContactoModel(IConfiguration configuration, ILogger<ContactoModel> logger, AmazonSesEmailSender emailServicio)
        {
            _configuration = configuration;
            _logger = logger;
            _emailServicio = emailServicio;
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
           var body =
                $"{Mensaje}\r\n\r\n\r\nNombre: {Nombre}\r\nE-mail: {Email}\r\nTeléfono:{Telefono}\r\n* Mensaje enviado desde el formulario de contacto de la Web";

            var respuesta = await _emailServicio.EnviarEmailDeContactoAsync(
                    Nombre, 
                    Email, 
                    Asunto ?? "Contacto web", 
                    body);

        }
    }
}
