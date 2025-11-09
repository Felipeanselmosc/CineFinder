namespace CineFinder.Application.Models
{
    /// <summary>
    /// Representa um resultado paginado de uma consulta
    /// </summary>
    /// <typeparam name="T">Tipo dos itens na página</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Itens da página atual
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Número da página atual (começa em 1)
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Tamanho da página (quantidade de itens por página)
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total de páginas disponíveis
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Total de itens em todas as páginas
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Indica se existe página anterior
        /// </summary>
        public bool HasPrevious => PageNumber > 1;

        /// <summary>
        /// Indica se existe próxima página
        /// </summary>
        public bool HasNext => PageNumber < TotalPages;

        /// <summary>
        /// URL da primeira página (será preenchida pelos controllers)
        /// </summary>
        public string? FirstPage { get; set; }

        /// <summary>
        /// URL da página anterior (será preenchida pelos controllers)
        /// </summary>
        public string? PreviousPage { get; set; }

        /// <summary>
        /// URL da próxima página (será preenchida pelos controllers)
        /// </summary>
        public string? NextPage { get; set; }

        /// <summary>
        /// URL da última página (será preenchida pelos controllers)
        /// </summary>
        public string? LastPage { get; set; }

        /// <summary>
        /// Construtor do resultado paginado
        /// </summary>
        /// <param name="items">Itens da página atual</param>
        /// <param name="count">Total de itens</param>
        /// <param name="pageNumber">Número da página atual</param>
        /// <param name="pageSize">Tamanho da página</param>
        public PagedResult(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        /// <summary>
        /// Cria um resultado paginado vazio
        /// </summary>
        public PagedResult()
        {
            Items = new List<T>();
            TotalCount = 0;
            PageNumber = 1;
            PageSize = 10;
            TotalPages = 0;
        }
    }
}