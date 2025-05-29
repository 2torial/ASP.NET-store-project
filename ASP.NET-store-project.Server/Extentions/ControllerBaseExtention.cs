using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_store_project.Server.Extentions
{
    public static class ControllerBaseExtention
    {
        public static BadRequestObjectResult SingleErrorBadRequest(this ControllerBase controller, string name, string message) =>
            controller.BadRequest(new { Errors = new Dictionary<string, string[]>() { { name, new string[] { message } } } });

        public static BadRequestObjectResult SingleErrorBadRequest(this ControllerBase controller, string name, string[] messages) =>
            controller.BadRequest(new { Errors = new Dictionary<string, string[]>() { { name, messages } } });

        public static BadRequestObjectResult SingleErrorBadRequest(this ControllerBase controller, string message) =>
            controller.BadRequest(new { Errors = new Dictionary<string, string[]>() { { "other", new string[] { message } } } });

        public static BadRequestObjectResult SingleErrorBadRequest(this ControllerBase controller, string[] messages) =>
            controller.BadRequest(new { Errors = new Dictionary<string, string[]>() { { "other", messages } } });

        public static BadRequestObjectResult MultipleErrorsBadRequest(this ControllerBase controller, IDictionary<string, string[]> errors) =>
            controller.BadRequest(new { Errors = errors });
    }
}
