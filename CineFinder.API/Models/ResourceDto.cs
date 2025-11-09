namespace CineFinder.API.Models
{
    /// <summary>
    /// Envelope para recursos com links HATEOAS
    /// </summary>
    /// <typeparam name="T">Tipo do recurso</typeparam>
    public class ResourceDto<T>
    {
        /// <summary>
        /// Dados do recurso
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Lista de links HATEOAS relacionados ao recurso
        /// </summary>
        public List<Link> Links { get; set; } = new List<Link>();

        /// <summary>
        /// Construtor com dados
        /// </summary>
        /// <param name="data">Dados do recurso</param>
        public ResourceDto(T data)
        {
            Data = data;
        }

        /// <summary>
        /// Construtor vazio para serialização
        /// </summary>
        public ResourceDto()
        {
            Data = default!;
        }

        /// <summary>
        /// Adiciona um link HATEOAS ao recurso
        /// </summary>
        /// <param name="rel">Relação do link</param>
        /// <param name="href">URL do recurso</param>
        /// <param name="method">Método HTTP</param>
        public void AddLink(string rel, string href, string method)
        {
            Links.Add(new Link(rel, href, method));
        }

        /// <summary>
        /// Adiciona um link HATEOAS ao recurso
        /// </summary>
        /// <param name="link">Link a ser adicionado</param>
        public void AddLink(Link link)
        {
            Links.Add(link);
        }

        /// <summary>
        /// Adiciona múltiplos links HATEOAS ao recurso
        /// </summary>
        /// <param name="links">Lista de links a serem adicionados</param>
        public void AddLinks(IEnumerable<Link> links)
        {
            Links.AddRange(links);
        }
    }
}