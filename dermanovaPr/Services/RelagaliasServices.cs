using Azure;
using dermanovaPr.Data;
using dermanovaPr.Models;
using dermanovaPr.Models.Dtos;
using dermanovaPr.Models.Responses;
using dermanovaPr.Services.InterfaceServices;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace dermanovaPr.Services
{
    public class RelagaliasServices : IregaliasServices
    {
        private readonly IDbContextFactory<DataContext> _dbContextFactory;

        public RelagaliasServices(IDbContextFactory<DataContext> dbContextFactory)
        {

            _dbContextFactory = dbContextFactory;   
        }
        public async Task<BaseResponses> AddRegalias(RegaliasDTOS dTOS)
        {
            var responses = new BaseResponses();
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    // Convierte RegaliasDTOS a la entidad Regalias antes de añadirla a la base de datos
                    context.Add(new Regalias
                    {
                        Name = dTOS.Name,
                        Marcas = dTOS.Marcas,
                        Unidades = dTOS.Unidades,
                        State = true
                    });

                    // Agrega la entidad Regalias al contexto
                    

                    var result = await context.SaveChangesAsync();
                    if (result == 1)
                    {
                        responses.StatusCode = 200;
                        responses.Message = "Regalia was added successfully!";
                    }
                    else
                    {
                        responses.StatusCode = 400;
                        responses.Message = "Error: Please check the input data.";
                    }
                }
            }
            catch (Exception ex)
            {
                responses.StatusCode = 500;
                responses.Message = "An error occurred: " + ex.Message;
            }
            return responses;
        }

        public async Task<GetResponses> CountReg(int id)
        {
            var response = new GetResponses();

            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    // Obtiene la cantidad disponible de una regalia específica
                    var regalia = await context.regaliases
                        .Where(r => r.RegaliasId == id)
                        .Select(r => r.Unidades)
                        .FirstOrDefaultAsync();

                    if (regalia != null)
                    {
                        response.Contar = regalia;
                        response.StatusCode = 200; // Éxito
                    }
                    else
                    {
                        response.StatusCode = 404; // No encontrada
                        response.Message = "Regalia no encontrada";
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500; // Error de servidor
                response.Message = $"Ocurrió un error al obtener las existencias: {ex.Message}";
            }

            return response;
        }

        public  async Task<GetResponses> DeleteR(int Id)
        {
            var response = new GetResponses();
            try
            {
                using(var context = _dbContextFactory.CreateDbContext())
                {
                    var Reg = await context.regaliases.FindAsync(Id);
                    if (Reg == null)
                    {
                        response.StatusCode = 404;
                        response.Message = "Error.";
                    }
                    else
                    {
                        Reg.State = false;

                        context.regaliases.Update(Reg);
                        await context.SaveChangesAsync();
                        response.StatusCode =200;
                    }
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Error updating device: " + ex.Message;
            }

            return response;
        }

        public async Task<GetResponses> GetRegalias()
        {
            var response =  new GetResponses();
            try
            {
               using (var context = _dbContextFactory.CreateDbContext())
                {
                    var list = await context.regaliases.ToListAsync();
                    response.StatusCode = 200;
                    response.Message = "Success";
                    response.ListRegalias = list;

                }
            }
            catch ( Exception ex )
            {
                response.StatusCode = 404;
                response.Message = "Error";
            }

            return response;
        }



    }
}
