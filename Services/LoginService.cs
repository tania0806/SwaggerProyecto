using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using reportesApi.DataContext;
using reportesApi.Models;
using System.Collections.Generic;
using reportesApi.Models.Compras;
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
namespace reportesApi.Services
{
    public class LoginService
    {
        private  string connection;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private ArrayList parametros = new ArrayList();


        public LoginService(IMarcatelDatabaseSetting settings, IWebHostEnvironment webHostEnvironment)
        {
             connection = settings.ConnectionString;

             _webHostEnvironment = webHostEnvironment;
             
        }

        public List<GetLoginModel> GetLogin()
        {
            ConexionDataAccess dac = new ConexionDataAccess(connection);
            GetLoginModel login = new GetLoginModel();

            List<GetLoginModel> lista = new List<GetLoginModel>();
            try
            {
                parametros = new ArrayList();
                DataSet ds = dac.Fill("sp_get_registro", parametros);
                if (ds.Tables[0].Rows.Count > 0)
                {

                  lista = ds.Tables[0].AsEnumerable()
                    .Select(dataRow => new GetLoginModel {
                        Id = int.Parse(dataRow["Id"].ToString()),
                        Nombres = dataRow["Nombres"].ToString(),
                        ApellidoPaterno = dataRow["ApellidoPaterno"].ToString(),
                        Correo = dataRow["Correo"].ToString(),
                        Contraseña = dataRow["Contraseña"].ToString(),
                        NumeroTelefono= dataRow["NumeroTelefono"].ToString(),
                        Token = dataRow["Token"].ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lista;
        }
    }
}