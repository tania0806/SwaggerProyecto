using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class RegistroService
{
    private readonly string _connectionString;

    public RegistroService(IConfiguration configuration)
    {
        // Obtener la cadena de conexión desde el archivo de configuración
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<bool> ValidarLoginAsync(RegistroModel registro)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("ValidarLogin", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Agregar parámetros al stored procedure
                cmd.Parameters.AddWithValue("@Correo", registro.Correo);
                cmd.Parameters.AddWithValue("@Contraseña", registro.Contraseña);

                // Abrir la conexión
                await conn.OpenAsync();

                // Ejecutar el stored procedure y obtener el resultado
                var resultado = (int)await cmd.ExecuteScalarAsync();

                // Si el resultado es 1, el login es válido
                return resultado == 1;
            }
        }
    }
}
