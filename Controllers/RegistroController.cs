using System;
using Microsoft.AspNetCore.Mvc;
using reportesApi.Services;
using reportesApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Net;

namespace reportesApi.Controllers
{
    [Route("api")]
    public class RegistroController : ControllerBase
    {
        private readonly RegistroService _registroService;
        private readonly ILogger<RegistroController> _logger;
        private readonly IJwtAuthenticationService _authService;

        // Instancia de la clase Encrypt para cifrar las contraseñas
        Encrypt enc = new Encrypt();

        public RegistroController(RegistroService registroService, ILogger<RegistroController> logger, IJwtAuthenticationService authService)
        {
            _registroService = registroService;
            _logger = logger;
            _authService = authService;
        }

        // Ruta para el inicio de sesión
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public JsonResult registro([FromBody] InsertRegistro registro) // InsertRegistro es un modelo que ya debes tener en tu proyecto
        {
            ResponseLogin result = new ResponseLogin(); // Clase para el formato de respuesta
            result.response = new ResponseBody(); // Cuerpo de la respuesta
            result.response.data = new DataResponseLogin(); // Datos específicos de la respuesta de login
            result.response.data.Usuario = new UsuarioModel(); // Información del usuario

            // Encriptar la contraseña usando SHA-256
            string cryptedPass = enc.GetSHA256(registro.Contraseña);

            // Llamada al servicio de registro para validar el correo y la contraseña
            var registroResponse = _registroService.registro(registro.Correo, cryptedPass);

            // Verificación de si el registro fue exitoso
            if (registroResponse.Id != 0)
            {
                result.StatusCode = (int)HttpStatusCode.OK; // Código HTTP 200 OK
                result.succes = true;
                result.message = "Bienvenido";
                result.response.data.Status = true;
                result.response.data.Mensaje = "Bienvenido";

                // Genera el token de autenticación
                var token = _authService.Authenticate(registro.Correo, cryptedPass);
                result.response.data.Token = token;
            }
            else
            {
                result.succes = false;
                result.message = "Usuario o contraseña incorrecto.";
            }

            // Devuelve la respuesta como JSON
            return new JsonResult(result);
        }
        
    }
    
}
