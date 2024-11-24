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
                    // Crear una nueva instancia del cliente
                    var nuevoCliente = new Clientes
                    {
                        Cedula = clientesDtos.Cedula,
                        Celular = clientesDtos.Celular,
                        Nombre = clientesDtos.Nombre,
                        State = true
                    };

                    // Agregar al contexto
                    context.Clientes.Add(nuevoCliente);
                    var result = await context.SaveChangesAsync();

                    if (result == 1)
                    {
                        // Obtener el ID recién generado
                        responses.StatusCode = 200;
                        responses.Message = "Cliente was added successfully!";
                        responses.NewClienteId = nuevoCliente.ClienteId; // Asignar el ID generado
                        responses.IsSuccess = true;
                    }
                    else
                    {
                        responses.StatusCode = 400;
                        responses.Message = "Error Please Check";
                    }
                }
            }
            catch (Exception ex)
            {
                responses.StatusCode = 500;
                responses.Message = ex.Message; // Registrar el mensaje del error para depuración
            }

            return responses;
        }

        public async Task<BaseResponses> AddDiagnostico(DiagnosticoDTOS dTOS)
        {

            var responses = new BaseResponses();
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    var diag = new Diagnosticos
                    {
                        DiagnosticosId = dTOS.DiagnosticosId,
                        Observaciobes = dTOS.Observaciobes,
                        Padecimientos = dTOS.Padecimientos,
                        State = true

                    };
                    context.Diagnosticos.Add(diag);
                    var result = await context.SaveChangesAsync();

                    if (result == 1)
                    {
                        // Obtener el ID recién generado
                        responses.StatusCode = 200;
                        responses.Message = "Diagnostico was added successfully!";
                        responses.IsSuccess = true;
                        responses.NewDiagnosticoId = diag.DiagnosticosId;
                    }
                    else
                    {
                        responses.StatusCode = 400;
                        responses.Message = "Error Please Check";
                    }

                }
            }
            catch (Exception ex)
            {
                responses.StatusCode = 500;
                responses.Message = ex.Message; // Registrar el mensaje del error para depuración
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

        public async Task<GetResponses> ClienteExist(int id)
        {
            var responses = new GetResponses();
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    // Obtener cliente por ID
                    var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.ClienteId == id);

                    if (cliente != null)
                    {
                        responses.StatusCode = 200;
                        responses.client = cliente;
                        //new List<Clientes> { cliente }; // Envuelve en una lista si es necesario
                        responses .IsSuccess = true;
                    }
                    else
                    {
                        responses.StatusCode = 404;
                        responses.Message = "Cliente no encontrado";
                    }
                }
            }
            catch (Exception ex)
            {
                responses.StatusCode = 500; // Código para errores internos
                responses.Message = $"Error al buscar el cliente: {ex.Message}";
            }

            return responses;
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
