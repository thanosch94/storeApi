namespace StoreApi.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using StoreApi.Data;
    using StoreApi.Data.Dto;
    using StoreApi.Data.Enums;
    using StoreApi.Services;
    using System;
    using System.Text.Json;

    public class ActionLoggingFilter : IActionFilter
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActionLoggingFilter(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            var userId = GetUserId();

            LogService.CreateLog(
                $"Start {action} - {controller}Controller",
                LogTypeEnum.Information,
                LogOriginEnum.StoreApp,
                userId,
                _context
            );
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();
            var userId = GetUserId();

            if (context.Result is ObjectResult objectResult)
            {


                var value = objectResult.Value;
                if (value != null && value.GetType().IsGenericType &&
                value.GetType().GetGenericTypeDefinition() == typeof(OptionsListDto<>))
                {
                    var countProp = value.GetType().GetProperty("TotalPages");

                    var count = countProp.GetValue(value);
                    LogService.CreateLog(
                        $"Result {action} - {controller}Controller: {count} records",
                        LogTypeEnum.Information,
                        LogOriginEnum.StoreApp,
                        userId,
                        _context
                    );
                }
                else
                {
                    var resultJson = JsonConvert.SerializeObject(
                  objectResult.Value,
                  new JsonSerializerSettings
                  {
                      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                  });

                    LogService.CreateLog(
                        $"Result {action} - {controller}Controller: {resultJson}",
                        LogTypeEnum.Information,
                        LogOriginEnum.StoreApp,
                        userId,
                        _context
                    );
                }
              
            }

            LogService.CreateLog(
                $"End {action} - {controller}Controller",
                LogTypeEnum.Information,
                LogOriginEnum.StoreApp,
                userId,
                _context
            );
        }

        private Guid? GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?
                .User?
                .FindFirst("Id")?
                .Value;

            return Guid.TryParse(userIdClaim, out var userId)
                ? userId
                : null;
        }
    }
}
