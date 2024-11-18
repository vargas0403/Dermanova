using dermanovaPr.Data;
using dermanovaPr.Models;
using dermanovaPr.Models.Dtos;
using dermanovaPr.Models.Responses;
using dermanovaPr.Services.InterfaceServices;
using Microsoft.EntityFrameworkCore;

namespace dermanovaPr.Services
{
    public class PrestacionesServices : IPrestacionesServices

    {

        private readonly IDbContextFactory<DataContext> _dbContextFactory;
        public PrestacionesServices(IDbContextFactory<DataContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task<BaseResponses> AddPrestaciones(PrestacionesDTOS dTOS)
        {
            var responses = new BaseResponses();
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    context.Add(new Prestaciones
                    {
                        Name = dTOS.Name,
                        Description = dTOS.Description,
                        costo = dTOS.costo,
                        State = true
                    });

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

        public async Task<GetResponses> GetPrestaciones()
        {
            var response = new GetResponses();
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    var list = await context.prestaciones.ToListAsync();
                    response.StatusCode = 200;
                    response.Message = "Success";
                    response.ListPres = list;

                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 404;
                response.Message = "Error";
            }

            return response;
        }
    }
    
}
