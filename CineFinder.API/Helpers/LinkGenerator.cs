using Microsoft.AspNetCore.Mvc;

namespace CineFinder.API.Helpers
{
    /// <summary>
    /// Helper para gerar links HATEOAS
    /// </summary>
    public static class LinkGenerator
    {
        /// <summary>
        /// Gera links HATEOAS para um Filme
        /// </summary>
        /// <param name="filmeId">ID do filme</param>
        /// <param name="urlHelper">Helper de URL do ASP.NET</param>
        /// <returns>Lista de links HATEOAS</returns>
        public static List<Models.Link> GenerateFilmeLinks(Guid filmeId, IUrlHelper urlHelper)
        {
            var links = new List<Models.Link>
            {
                new Models.Link("self",
                    urlHelper.Action("GetById", "Filmes", new { id = filmeId }) ?? string.Empty,
                    "GET"),
                new Models.Link("update",
                    urlHelper.Action("Update", "Filmes", new { id = filmeId }) ?? string.Empty,
                    "PUT"),
                new Models.Link("delete",
                    urlHelper.Action("Delete", "Filmes", new { id = filmeId }) ?? string.Empty,
                    "DELETE"),
                new Models.Link("avaliacoes",
                    urlHelper.Action("GetByFilme", "Avaliacoes", new { filmeId = filmeId }) ?? string.Empty,
                    "GET"),
                new Models.Link("add-to-lista",
                    urlHelper.Action("AddFilme", "Listas", new { filmeId = filmeId }) ?? string.Empty,
                    "POST"),
                new Models.Link("all-filmes",
                    urlHelper.Action("GetAll", "Filmes") ?? string.Empty,
                    "GET")
            };

            return links;
        }

        /// <summary>
        /// Gera links HATEOAS para um Gênero
        /// </summary>
        /// <param name="generoId">ID do gênero</param>
        /// <param name="urlHelper">Helper de URL do ASP.NET</param>
        /// <returns>Lista de links HATEOAS</returns>
        public static List<Models.Link> GenerateGeneroLinks(Guid generoId, IUrlHelper urlHelper)
        {
            var links = new List<Models.Link>
            {
                new Models.Link("self",
                    urlHelper.Action("GetById", "Generos", new { id = generoId }) ?? string.Empty,
                    "GET"),
                new Models.Link("update",
                    urlHelper.Action("Update", "Generos", new { id = generoId }) ?? string.Empty,
                    "PUT"),
                new Models.Link("delete",
                    urlHelper.Action("Delete", "Generos", new { id = generoId }) ?? string.Empty,
                    "DELETE"),
                new Models.Link("filmes",
                    urlHelper.Action("Search", "Filmes", new { generoId = generoId }) ?? string.Empty,
                    "GET"),
                new Models.Link("all-generos",
                    urlHelper.Action("GetAll", "Generos") ?? string.Empty,
                    "GET")
            };

            return links;
        }

        /// <summary>
        /// Gera links HATEOAS para uma Lista
        /// </summary>
        /// <param name="listaId">ID da lista</param>
        /// <param name="urlHelper">Helper de URL do ASP.NET</param>
        /// <returns>Lista de links HATEOAS</returns>
        public static List<Models.Link> GenerateListaLinks(Guid listaId, IUrlHelper urlHelper)
        {
            var links = new List<Models.Link>
            {
                new Models.Link("self",
                    urlHelper.Action("GetById", "Listas", new { id = listaId }) ?? string.Empty,
                    "GET"),
                new Models.Link("update",
                    urlHelper.Action("Update", "Listas", new { id = listaId }) ?? string.Empty,
                    "PUT"),
                new Models.Link("delete",
                    urlHelper.Action("Delete", "Listas", new { id = listaId }) ?? string.Empty,
                    "DELETE"),
                new Models.Link("add-filme",
                    urlHelper.Action("AddFilme", "Listas", new { listaId = listaId }) ?? string.Empty,
                    "POST"),
                new Models.Link("remove-filme",
                    urlHelper.Action("RemoveFilme", "Listas", new { listaId = listaId }) ?? string.Empty,
                    "DELETE"),
                new Models.Link("all-listas",
                    urlHelper.Action("GetAll", "Listas") ?? string.Empty,
                    "GET")
            };

            return links;
        }

        /// <summary>
        /// Gera links HATEOAS para uma Avaliação
        /// </summary>
        /// <param name="avaliacaoId">ID da avaliação</param>
        /// <param name="urlHelper">Helper de URL do ASP.NET</param>
        /// <returns>Lista de links HATEOAS</returns>
        public static List<Models.Link> GenerateAvaliacaoLinks(Guid avaliacaoId, IUrlHelper urlHelper)
        {
            var links = new List<Models.Link>
            {
                new Models.Link("self",
                    urlHelper.Action("GetById", "Avaliacoes", new { id = avaliacaoId }) ?? string.Empty,
                    "GET"),
                new Models.Link("update",
                    urlHelper.Action("Update", "Avaliacoes", new { id = avaliacaoId }) ?? string.Empty,
                    "PUT"),
                new Models.Link("delete",
                    urlHelper.Action("Delete", "Avaliacoes", new { id = avaliacaoId }) ?? string.Empty,
                    "DELETE"),
                new Models.Link("all-avaliacoes",
                    urlHelper.Action("GetAll", "Avaliacoes") ?? string.Empty,
                    "GET")
            };

            return links;
        }

        /// <summary>
        /// Gera links HATEOAS para um Usuário
        /// </summary>
        /// <param name="usuarioId">ID do usuário</param>
        /// <param name="urlHelper">Helper de URL do ASP.NET</param>
        /// <returns>Lista de links HATEOAS</returns>
        public static List<Models.Link> GenerateUsuarioLinks(Guid usuarioId, IUrlHelper urlHelper)
        {
            var links = new List<Models.Link>
            {
                new Models.Link("self",
                    urlHelper.Action("GetById", "Usuarios", new { id = usuarioId }) ?? string.Empty,
                    "GET"),
                new Models.Link("update",
                    urlHelper.Action("Update", "Usuarios", new { id = usuarioId }) ?? string.Empty,
                    "PUT"),
                new Models.Link("delete",
                    urlHelper.Action("Delete", "Usuarios", new { id = usuarioId }) ?? string.Empty,
                    "DELETE"),
                new Models.Link("listas",
                    urlHelper.Action("GetByUsuario", "Listas", new { usuarioId = usuarioId }) ?? string.Empty,
                    "GET"),
                new Models.Link("avaliacoes",
                    urlHelper.Action("GetByUsuario", "Avaliacoes", new { usuarioId = usuarioId }) ?? string.Empty,
                    "GET"),
                new Models.Link("all-usuarios",
                    urlHelper.Action("GetAll", "Usuarios") ?? string.Empty,
                    "GET")
            };

            return links;
        }

        /// <summary>
        /// Gera links de paginação para resultados paginados
        /// </summary>
        /// <param name="urlHelper">Helper de URL do ASP.NET</param>
        /// <param name="actionName">Nome da action</param>
        /// <param name="controllerName">Nome do controller</param>
        /// <param name="pageNumber">Número da página atual</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="totalPages">Total de páginas</param>
        /// <param name="routeValues">Valores de rota adicionais</param>
        /// <returns>Dicionário com links de paginação</returns>
        public static Dictionary<string, string?> GeneratePaginationLinks(
            IUrlHelper urlHelper,
            string actionName,
            string controllerName,
            int pageNumber,
            int pageSize,
            int totalPages,
            object? routeValues = null)
        {
            var links = new Dictionary<string, string?>();

            // Converter routeValues para dicionário
            var routeDict = new Dictionary<string, object>();
            if (routeValues != null)
            {
                foreach (var prop in routeValues.GetType().GetProperties())
                {
                    var value = prop.GetValue(routeValues);
                    if (value != null)
                    {
                        routeDict[prop.Name] = value;
                    }
                }
            }

            // First Page
            routeDict["pageNumber"] = 1;
            routeDict["pageSize"] = pageSize;
            links["firstPage"] = urlHelper.Action(actionName, controllerName, routeDict);

            // Previous Page
            if (pageNumber > 1)
            {
                routeDict["pageNumber"] = pageNumber - 1;
                links["previousPage"] = urlHelper.Action(actionName, controllerName, routeDict);
            }

            // Next Page
            if (pageNumber < totalPages)
            {
                routeDict["pageNumber"] = pageNumber + 1;
                links["nextPage"] = urlHelper.Action(actionName, controllerName, routeDict);
            }

            // Last Page
            routeDict["pageNumber"] = totalPages;
            links["lastPage"] = urlHelper.Action(actionName, controllerName, routeDict);

            return links;
        }
    }
}