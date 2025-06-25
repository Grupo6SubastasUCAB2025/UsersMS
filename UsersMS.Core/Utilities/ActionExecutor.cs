using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace UsersMS.Core.Utilities
{
    public static class ActionExecutor
    {
        public static async Task<IActionResult> Execute(
            Func<Task<IActionResult>> action,
            ModelStateDictionary modelState,
            ILogger logger,
            string actionName)
        {
            if (!modelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var modelStateValue in modelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }

                logger.LogWarning("El modelo es inválido para la acción {ActionName}. Errores: {Errors}", actionName, errors);
                return new BadRequestObjectResult(new { Errores = errors });
            }

            try
            {
                logger.LogInformation("Ejecutando acción {ActionName}", actionName);

                var result = await action();

                if (result is ObjectResult objectResult)
                {
                    var outputJson = JsonConvert.SerializeObject(objectResult.Value, new JsonSerializerSettings
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            IgnoreSerializableAttribute = true
                        },
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                    logger.LogInformation("Acción {ActionName} ejecutada correctamente con salida: {Output}", actionName, outputJson);
                }
                else
                {
                    logger.LogInformation("Acción {ActionName} ejecutada correctamente", actionName);
                }

                return result;
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogWarning("Acceso no autorizado en la acción {ActionName}: {Mensaje}", actionName, ex.Message);
                return new UnauthorizedObjectResult(new { Mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                logger.LogError("Error interno en la acción {ActionName}: {Mensaje}", actionName, ex.Message);
                return new ObjectResult($"Error interno del servidor: {ex.Message}") { StatusCode = 500 };
            }
        }
    }
}
