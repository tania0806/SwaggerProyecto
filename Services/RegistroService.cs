using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using reportesApi.DataContext;
using reportesApi.Models;
namespace reportesApi.Services
{
    public class RegistroService
    {
        private  string connection;
        
        
        public RegistroService(IMarcatelDatabaseSetting settings)
        {
             connection = settings.ConnectionString;
        }
        public RegistroModel registro(string Correo, string Contraseña)
        {
            
            RegistroModel registro = new RegistroModel();
            ConexionDataAccess dac = new ConexionDataAccess(connection);
            try
            {
                ArrayList parametros = new ArrayList();
                parametros.Add(new SqlParameter { ParameterName = "@Correo", SqlDbType = SqlDbType.VarChar, Value = Correo });
                parametros.Add(new SqlParameter { ParameterName = "@Contraseña", SqlDbType = SqlDbType.VarChar, Value = Contraseña });
                DataSet ds = dac.Fill("Validar_Login", parametros);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        registro.Correo = row["Correo"].ToString();
                        registro.Contraseña = row["Contraseña"].ToString();
                    
                    }
                }
                return registro;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
    }
}
