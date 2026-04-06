using Microsoft.AspNetCore.Mvc;

namespace CineFinder.API.Helpers
{
    public static class HateoasLinkGenerator
    {
        public static List<Models.Link> GenerateFilmeLinks(Guid filmeId, IUrlHelper urlHelper)
        {
            return new List<Models.Link>
            {
                new("self", urlHelper.Action("GetById", "Filmes", new { id = filmeId }) ?? string.Empty, "GET"),
                new("update", urlHelper.Action("Update", "Filmes", new { id = filmeId }) ?? string.Empty, "PUT"),
                new("delete", urlHelper.Action("Delete", "Filmes", new { id = filmeId }) ?? string.Empty, "DELETE"),
                new("all-filmes", urlHelper.Action("GetAll", "Filmes") ?? string.Empty, "GET")
            };
        }

        public static List<Models.Link> GenerateGeneroLinks(Guid generoId, IUrlHelper urlHelper)
        {
            return new List<Models.Link>
            {
                new("self", urlHelper.Action("GetById", "Generos", new { id = generoId }) ?? string.Empty, "GET"),
                new("update", urlHelper.Action("Update", "Generos", new { id = generoId }) ?? string.Empty, "PUT"),
                new("delete", urlHelper.Action("Delete", "Generos", new { id = generoId }) ?? string.Empty, "DELETE"),
                new("all-generos", urlHelper.Action("GetAll", "Generos") ?? string.Empty, "GET")
            };
        }

        public static List<Models.Link> GenerateListaLinks(Guid listaId, IUrlHelper urlHelper)
        {
            return new List<Models.Link>
            {
                new("self", urlHelper.Action("GetById", "Listas", new { id = listaId }) ?? string.Empty, "GET"),
                new("update", urlHelper.Action("Update", "Listas", new { id = listaId }) ?? string.Empty, "PUT"),
                new("delete", urlHelper.Action("Delete", "Listas", new { id = listaId }) ?? string.Empty, "DELETE"),
                new("all-listas", urlHelper.Action("GetAll", "Listas") ?? string.Empty, "GET")
            };
        }

        public static List<Models.Link> GenerateAvaliacaoLinks(Guid avaliacaoId, IUrlHelper urlHelper)
        {
            return new List<Models.Link>
            {
                new("self", urlHelper.Action("GetById", "Avaliacoes", new { id = avaliacaoId }) ?? string.Empty, "GET"),
                new("update", urlHelper.Action("Update", "Avaliacoes", new { id = avaliacaoId }) ?? string.Empty, "PUT"),
                new("delete", urlHelper.Action("Delete", "Avaliacoes", new { id = avaliacaoId }) ?? string.Empty, "DELETE"),
                new("all-avaliacoes", urlHelper.Action("GetAll", "Avaliacoes") ?? string.Empty, "GET")
            };
        }

        public static List<Models.Link> GenerateUsuarioLinks(Guid usuarioId, IUrlHelper urlHelper)
        {
            return new List<Models.Link>
            {
                new("self", urlHelper.Action("GetById", "Usuarios", new { id = usuarioId }) ?? string.Empty, "GET"),
                new("update", urlHelper.Action("Update", "Usuarios", new { id = usuarioId }) ?? string.Empty, "PUT"),
                new("delete", urlHelper.Action("Delete", "Usuarios", new { id = usuarioId }) ?? string.Empty, "DELETE"),
                new("all-usuarios", urlHelper.Action("GetAll", "Usuarios") ?? string.Empty, "GET")
            };
        }

        public static Dictionary<string, string?> GeneratePaginationLinks(IUrlHelper urlHelper, string actionName, string controllerName, int pageNumber, int pageSize, int totalPages, object? routeValues = null)
        {
            var links = new Dictionary<string, string?>();
            var routeDict = new Dictionary<string, object>();
            if (routeValues != null)
                foreach (var prop in routeValues.GetType().GetProperties())
                {
                    var value = prop.GetValue(routeValues);
                    if (value != null) routeDict[prop.Name] = value;
                }
            routeDict["pageNumber"] = 1; routeDict["pageSize"] = pageSize;
            links["firstPage"] = urlHelper.Action(actionName, controllerName, routeDict);
            if (pageNumber > 1) { routeDict["pageNumber"] = pageNumber - 1; links["previousPage"] = urlHelper.Action(actionName, controllerName, routeDict); }
            if (pageNumber < totalPages) { routeDict["pageNumber"] = pageNumber + 1; links["nextPage"] = urlHelper.Action(actionName, controllerName, routeDict); }
            routeDict["pageNumber"] = totalPages;
            links["lastPage"] = urlHelper.Action(actionName, controllerName, routeDict);
            return links;
        }
    }
}
