using Microsoft.AspNetCore.Mvc;

namespace SwaggerProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistroController : ControllerBase
    {
        // Acción para el registro
        [HttpPost("Registro")]
        public IActionResult Registro([FromBody] RegistroRequest registroRequest)
        {
            // Validación de que los campos no sean nulos o vacíos
            if (string.IsNullOrEmpty(registroRequest.Correo) || string.IsNullOrEmpty(registroRequest.Contrasena))
            {
                return BadRequest(new { success = false, message = "Correo y contraseña no pueden estar vacíos." });
            }

            // Autenticación del usuario
            bool isAuthenticated = AuthenticateUser(registroRequest.Correo, registroRequest.Contrasena);

            if (isAuthenticated)
            {
                // Si está autenticado, redirige al home
                return Ok(new { success = true, message = "Inicio de sesión exitoso", redirectTo = "/home" });
            }
            else
            {
                // Si no está autenticado, devuelve un mensaje de error
                return Unauthorized(new { success = false, message = "Correo o contraseña incorrectos." });
            }
        }

        // Método de ejemplo para autenticar usuario
        private bool AuthenticateUser(string Correo, string Contraseña)
        {
            // Aquí iría la lógica para autenticar al usuario, como consultar la base de datos
            // Por ahora, usaremos un ejemplo estático
            return Correo == "admin@admin.com" && Contraseña == "admin123";
        }
    }

    // Clase para manejar el request
    public class RegistroRequest
    {
        public string Correo { get; set; }
        public string Contrasena { get; set; }
    }
}
