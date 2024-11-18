using Azure;
using dermanovaPr.Data;
using dermanovaPr.Models.Dtos;
using dermanovaPr.Models.Responses;
using dermanovaPr.Services.InterfaceServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace dermanovaPr.Services
{
    public class TrabajadoresServices:ITrabajadores
    {
        private readonly IDbContextFactory<DataContext> _dbContextFactory;
        public TrabajadoresServices(IDbContextFactory<DataContext> dbContextFactory)
        {
                _dbContextFactory = dbContextFactory;
        }

        public async Task<BaseResponses> AddTrabajador(TrabajadoresDTOS trabajadores)
        {
            var responses = new BaseResponses();
            try
            {
                using( var context= _dbContextFactory.CreateDbContext() )
                {
                    context.Add(new TrabajadoresDTOS
                    {
                        TrabajadoresId = trabajadores.TrabajadoresId,
                        Nombre = trabajadores.Nombre,
                        Cedula = trabajadores.Cedula,
                        Celular = trabajadores.Celular,
                        UserId = trabajadores.UserId,
                        Puesto = trabajadores.Puesto,
                        State = true
                    }) ;

                    var resutl = await context.SaveChangesAsync();
                    if (resutl == 1)
                    {
                        responses.StatusCode = 200;
                        responses.Message = " carro was added succesfully!";
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
                responses.Message = "Error";
               
            }
            return responses;
        }

        public async Task<GetResponses> GetTrabajadores()
        {
           
            var response = new GetResponses();
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    var listT = await  context.Trabajadores.Where(T => T.State == true).ToListAsync();
                    response.StatusCode = 200;
                    response.Message = "Success";
                    response.trabjL= listT;
                };

            }
            catch (Exception ex)
            {

            }
            return response;
        }
    }
}
