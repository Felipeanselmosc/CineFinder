namespace CineFinder.API.Models
{
    /// <summary>
    /// Representa um link HATEOAS
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Relação do link (self, update, delete, etc)
        /// </summary>
        public string Rel { get; set; }

        /// <summary>
        /// URL do recurso
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// Método HTTP (GET, POST, PUT, DELETE, etc)
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Construtor do Link
        /// </summary>
        /// <param name="rel">Relação do link</param>
        /// <param name="href">URL do recurso</param>
        /// <param name="method">Método HTTP</param>
        public Link(string rel, string href, string method)
        {
            Rel = rel;
            Href = href;
            Method = method;
        }

        /// <summary>
        /// Construtor vazio para serialização
        /// </summary>
        public Link()
        {
            Rel = string.Empty;
            Href = string.Empty;
            Method = string.Empty;
        }
    }
}