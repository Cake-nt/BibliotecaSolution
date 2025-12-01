using Biblioteca.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biblioteca.Business.Services
{
    public abstract class BaseService
    {
        protected async Task ExecuteWithExceptionHandlingAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                // Aquí puedes loggear el error
                throw new Exception($"Error en servicio: {ex.Message}", ex);
            }
        }
    }
}