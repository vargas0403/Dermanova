﻿using Azure;
using Azure.Core.GeoJson;
using dermanovaPr.Data;
using dermanovaPr.Models;
using dermanovaPr.Models.Dtos;
using dermanovaPr.Models.Responses;
using dermanovaPr.Services.InterfaceServices;
using Microsoft.EntityFrameworkCore;

namespace dermanovaPr.Services
{
    public class Citas_FacturacionServices : Icitas_FacturacionServices
    {

        private readonly IDbContextFactory<DataContext> _dbContextFactory;
        public Citas_FacturacionServices(IDbContextFactory<DataContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;

        }
        public async Task<BaseResponses> AddCitaFac(FacturacionDTOS FCdTOS, List<DetallesDTOS> DTdtosList, CitasDTOS CTdtos)
        {
            var response = new BaseResponses();

            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    // Crear la entidad de Factura a partir del DTO y guardar
                    var factura = new Facturaciones
                    {
                        Fecha = FCdTOS.Fecha,
                        State = FCdTOS.State
                    };

                    context.Facturaciones.Add(factura);
                    await context.SaveChangesAsync(); // Guarda aquí para generar el FacturaId

                    // Ahora `factura.FacturaId` está disponible
                    foreach (var detalleDto in DTdtosList)
                    {
                        var detalleFactura = new DetalleFactura
                        {
                            PrestacionesId = detalleDto.PrestacionesId,
                            Cantidad = detalleDto.Cantidad,
                            Precio = detalleDto.Precio,
                            State = true,
                            FacturacionesId = null // Temporalmente nulo
                        };
                        context.DetalleFacturas.Add(detalleFactura);
                    }

                    await context.SaveChangesAsync(); // Guardar los detalles con FacturaId como nulo

                    // Asignar el FacturaId a cada detalle y actualizar en la base de datos
                    foreach (var detalle in context.DetalleFacturas.Where(d => d.FacturacionesId == null))
                    {
                        detalle.FacturacionesId = factura.FacturaId;
                    }

                    // Recalcular el total de la factura
                    factura.Total = DTdtosList.Sum(d => d.Cantidad * d.Precio);
                    await context.SaveChangesAsync(); // Guardar el total actualizado en la factura

                    // Crear la entidad de Cita asociada a la factura
                    var cita = new Citas
                    {
                        Fecha = CTdtos.Fecha,
                        Hora = CTdtos.Hora,
                        ClienteId = CTdtos.ClienteId,
                        TrabajadorId = CTdtos.TrabajadorId,
                        RegaliaId = CTdtos.RegaliaId,
                        DiagnosticoId = CTdtos.DiagnosticoId,
                        PadecimientoId = CTdtos.PadecimientoId,
                        tipo = CTdtos.tipo,
                        FacturaId = factura.FacturaId, // Relaciona la cita con la factura
                        State = true
                    };

                    // Agregar la cita y guardar cambios
                    context.Citas.Add(cita);
                    await context.SaveChangesAsync();

                    // Responder con éxito
                    //response.IsSuccess = true;
                    response.Message = "Factura y cita guardadas exitosamente";
                }
            }
            catch (Exception ex)
            {
                // response.IsSuccess = false;
                response.Message = $"Error al guardar factura y cita: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponses> AddFacturaAndUpdateCita(FacturacionDTOS FCdTOS, List<DetallesDTOS> DTdtosList, int citaId)
        {
            var response = new BaseResponses();

            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    // Crear la entidad de Factura
                    var factura = new Facturaciones
                    {
                        Fecha = FCdTOS.Fecha,
                        State = FCdTOS.State
                    };

                    context.Facturaciones.Add(factura);
                    await context.SaveChangesAsync(); // Guardar para obtener el ID de la factura

                    // Verificar que el ID de la factura se haya generado correctamente
                    if (factura.FacturaId == 0)
                    {
                        response.Message = "Error al generar la factura.";
                        return response;
                    }

                    // Crear los detalles de la factura y asociarlos con el FacturaId
                    foreach (var detalleDto in DTdtosList)
                    {
                        var detalleFactura = new DetalleFactura
                        {
                            PrestacionesId = detalleDto.PrestacionesId,
                            Cantidad = detalleDto.Cantidad,
                            Precio = detalleDto.Precio,
                            State = true,
                            FacturacionesId = factura.FacturaId // Asociar el ID de la factura
                        };
                        context.DetalleFacturas.Add(detalleFactura);
                    }

                    await context.SaveChangesAsync(); // Guardar los detalles de la factura

                    // Calcular el total de la factura y actualizar
                    factura.Total = DTdtosList.Sum(d => d.Cantidad * d.Precio);
                    context.Facturaciones.Update(factura);
                    await context.SaveChangesAsync(); // Guardar el total actualizado

                    // Buscar la cita existente por su ID
                    var cita = await context.Citas.FindAsync(citaId);
                    if (cita == null)
                    {
                        response.Message = "Cita no encontrada.";
                        return response;
                    }

                    // Actualizar el campo FacturaId en la cita
                    cita.FacturaId = factura.FacturaId;
                    context.Citas.Update(cita);
                    await context.SaveChangesAsync(); // Guardar los cambios en la cita

                    // Responder con éxito
                    response.IsSuccess = true;
                    response.Message = "Factura, detalles y cita actualizada exitosamente";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error al guardar factura, detalles y actualizar cita: {ex.Message}";
            }

            return response;
        }


        public async Task<GetResponses> GetCitas(int TrabajadoresId)
        {

            var response = new GetResponses();

            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    // Obtener las citas directamente con el trabajador asociado
                    var citas = await context.Citas
                        .Where(c => c.TrabajadorId == TrabajadoresId) // Filtrar por TrabajadorId
                        .Include(c => c.Cliente) // Incluir la información del cliente
                        .Include(c => c.Padecimiento) // Incluir la información de la factura
                        .Select(c => new CitasDTOS // Proyectar los datos a un DTO
                        {
                            CitasId = c.CitasId,
                            Fecha = c.Fecha,
                            Hora = c.Hora,
                            ClienteNombre = c.Cliente.Nombre,
                           padecimientoNombre = c.Padecimiento.Name,
                           tipo= c.tipo
                           
                          
                        })
                        .ToListAsync();

                    if (!citas.Any())
                    {
                        response.Message = "No hay citas registradas para este trabajador.";
                        return response;
                    }

                    // Configurar la respuesta
                    response.Data = citas;
                    response.Message = "Citas obtenidas exitosamente.";
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                response.Message = $"Error al obtener citas: {ex.Message}";
                response.IsSuccess = false;
            }

            return response;
         }



        public async Task<GetResponses> GetCitaTotal()
        {
            var response = new GetResponses();

            try
            {
                // Crear el contexto de base de datos
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    // Obtener todas las citas de manera asíncrona
                    var result = await context.Citas
                        .Include(c => c.Cliente) // Si necesitas incluir relaciones
                        .Include(c => c.Trabajador) // Incluir otras entidades relacionadas si aplica
                        .ToListAsync();

                    // Asignar el resultado a la propiedad de respuesta
                    response.ListCitas = result;
                    response.Message = "Citas obtenidas exitosamente.";
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                response.Message = $"Error al obtener citas: {ex.Message}";
                response.IsSuccess = false;
            }

            return response;
        }



        public async Task<GetResponses> GetCitasAgrupadasPorFechaYHora()
        {
            var response = new GetResponses();

            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    // Agrupar y proyectar los datos en los DTOs
                    var citasAgrupadas = await context.Citas
                        .Include(c => c.Cliente)
                        .Include(c => c.Trabajador)
                        .GroupBy(c => c.Fecha.Date) // Agrupar por fecha
                        .Select(g => new FechaGrupoDTO
                        {
                            Fecha = g.Key,
                            Horas = g.GroupBy(c => c.Hora) // Agrupar las citas de cada fecha por hora
                                     .Select(h => new HoraGrupoDTO
                                     {
                                         Hora = h.Key.ToString(@"hh\:mm"),
                                         Citas = h.Select(c => new CitaDTO
                                         {
                                             CitasId = c.CitasId,
                                             ClienteNombre = c.Cliente.Nombre,
                                             Trabajador = c.Trabajador.Nombre,
                                             Hora = c.Hora.ToString(@"hh\:mm"),
                                             Fecha = c.Fecha
                                         }).ToList()
                                     }).ToList()
                        })
                        .ToListAsync();

                    response.Data = citasAgrupadas; // Usar la propiedad Data en lugar de ListCitas
                    response.IsSuccess = true;
                    response.Message = "Citas agrupadas por fecha y hora obtenidas con éxito.";
                }
            }
            catch (Exception ex)
            {
                response.Message = $"Error al obtener citas agrupadas: {ex.Message}";
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<GetResponses> GetCitasOrdenadasPorFecha()
        {
            var response = new GetResponses();

            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    // Obtener y ordenar las citas directamente desde la entidad Citas
                    var citasOrdenadas = await context.Citas
                        .Include(c => c.Cliente) // Incluir la relación con Cliente
                        .Include(c => c.Trabajador) // Incluir la relación con Trabajador
                        .Include(c=> c.Padecimiento)
                        .OrderBy(c => c.Fecha) // Ordenar por Fecha
                        .ThenBy(c => c.Hora) // Ordenar por Hora dentro de la misma Fecha
                        .ToListAsync();

                    response.ListCitas = citasOrdenadas;
                    response.IsSuccess = true;
                    response.Message = "Citas ordenadas por fecha obtenidas con éxito.";
                }
            }
            catch (Exception ex)
            {
                response.Message = $"Error al obtener citas ordenadas: {ex.Message}";
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<BaseResponses> AddCita(CitasDTOS dTOS)
        {
          var responses = new BaseResponses();
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    context.Add(new Citas
                    {

                      Fecha = dTOS.Fecha,
                      Hora = dTOS.Hora,
                      ClienteId =dTOS.ClienteId,
                      TrabajadorId = dTOS.TrabajadorId,
                      PadecimientoId = dTOS.PadecimientoId,
                      DiagnosticoId = dTOS.DiagnosticoId,
                      RegaliaId = dTOS.RegaliaId,
                      tipo = dTOS.tipo


                    }

                    );
                    var resutl = await context.SaveChangesAsync();
                    if (resutl == 1)
                    {
                        responses.IsSuccess = true;
                        responses.StatusCode = 200;
                        responses.Message = " Cliente was added succesfully!";
                    }
                    else
                    {
                        responses.StatusCode = 400;
                        responses.Message = "Error Please Check";
                    }
                }
            }
            catch
            {
                
            }
            return responses;
        }

        public async Task<BaseResponses> UpdateCita(Citas citas)
        {
            var response = new BaseResponses();
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    context.Update(citas);
                    response.StatusCode = 200;
                    response.Message = "Success";
                    response.IsSuccess = true;
                    var result = await context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {

                response.StatusCode = 500;
                response.Message = "Error Deleting Device" + ex.Message;
            }
            return response;
        }
    }


    public class FechaGrupoDTO
    {
        public DateTime Fecha { get; set; }
        public List<HoraGrupoDTO> Horas { get; set; } = new();
    }

    public class HoraGrupoDTO
    {
        public string Hora { get; set; }
        public List<CitaDTO> Citas { get; set; } = new();
    }

    public class CitaDTO
    {
        public int CitasId { get; set; }
        public string ClienteNombre { get; set; }
        public string Trabajador { get; set; }
        public string Hora { get; set; }
        public DateTime Fecha { get; set; }
    }

}
