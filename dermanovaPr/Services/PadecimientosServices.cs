using Azure;
using dermanovaPr.Data;
using dermanovaPr.Models.Dtos;
using dermanovaPr.Models;
using dermanovaPr.Models.Responses;
using dermanovaPr.Services.InterfaceServices;
using Microsoft.EntityFrameworkCore;

namespace dermanovaPr.Services
{
    public class PadecimientosServices: IPadecimientoServices
    {

        private readonly IDbContextFactory<DataContext> _dbContextFactory;
        public PadecimientosServices(IDbContextFactory<DataContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

       
        public async Task<BaseResponses> AddPadecimientos(PadecimientoDTOS padecimientoDTOS) 
        {
            var responses = new BaseResponses();

            try {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    context.Add(new Padecimientos
                    {
                        
                        Name= padecimientoDTOS.Name,
                        Description= padecimientoDTOS.Description,
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
            catch (Exception ex) { }
            return responses;
        }

       

        public async Task<GetResponses> GetPadecimientosAsync()
        {
            var response = new GetResponses();
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    var list = await context.Padecimientos.ToListAsync();
                    response.StatusCode = 200;
                    response.Message = "Success";
                    response.ListPadecimientos = list;
                };

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
