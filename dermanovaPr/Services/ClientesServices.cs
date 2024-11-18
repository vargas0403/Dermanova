using Azure;
using dermanovaPr.Data;
using dermanovaPr.Models;
using dermanovaPr.Models.Dtos;
using dermanovaPr.Models.Responses;
using dermanovaPr.Services.InterfaceServices;
using Microsoft.EntityFrameworkCore;

namespace dermanovaPr.Services
{
    public class ClientesServices : IClientes
    {
        private readonly IDbContextFactory<DataContext> _dbContextFactory;
        public ClientesServices(IDbContextFactory<DataContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            
        }

        public async Task<BaseResponses> AddClientes(ClientesDTOS clientesDtos)
        {
            var responses = new BaseResponses();
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                     context.Add(new Clientes
                    {
                        ClienteId = clientesDtos.ClienteId,
                        Cedula = clientesDtos.Cedula,
                        Celular = clientesDtos.Celular,
                        Nombre = clientesDtos.Nombre,
                        State = true
                    }
                     );
                    var resutl = await context.SaveChangesAsync();
                    if (resutl == 1)
                    {
                        responses.StatusCode = 200;
                        responses.Message = " Cliente was added succesfully!";
                    }
                    else
                    {
                        responses.StatusCode = 400;
                        responses.Message = "Error Please Check";
                    }
                };

            }
            catch (Exception ex)
            {
                responses.StatusCode = 500;

            }
            return responses;
        }
        public async Task<GetResponses> CedulaExist(string Cedula)
        {
            var response = new GetResponses();
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    var exist = await context.Clientes
                                             .Where(c=> c.Cedula == Cedula )
                                             .FirstOrDefaultAsync();

                    if (exist != null)
                    {
                        response.Message = "Cédula existente.";
                        response.StatusCode = 404;
                    }
                    else
                    {
                        response.Message = "Cédula no encontrada.";
                        response.StatusCode = 202;
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí podrías registrar el error
                response.Message = "Ocurrió un error: " + ex.Message;
                response.StatusCode= 500;
            }

            return response;
        }

        public async Task<GetResponses> CelularExist(string Celular)
        {

            var response = new GetResponses();
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    var exist = await context.Clientes
                                             .Where(c => c.Celular==Celular)
                                             .FirstOrDefaultAsync();

                    if (exist != null)
                    {
                        response.Message = "Celular existente.";
                        response.StatusCode = 404;
                    }
                    else
                    {
                        response.Message = "Celular no encontrado.";
                        response.StatusCode = 202;
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí podrías registrar el error
                response.Message = "Ocurrió un error: " + ex.Message;
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<GetResponses> GetClientes()
        {
            var response = new GetResponses();
            try
            {
                using(var context = _dbContextFactory.CreateDbContext()) {
                    
                    var result = await context.Clientes.Where(C=>C.State==true).ToListAsync();
                    response.StatusCode = 200;
                    response.Message = "Success";
                    response.Listclientes = result;
                }
                
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Error";
            }
            return response;
        }
    }
}
