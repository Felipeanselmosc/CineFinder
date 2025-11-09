namespace CineFinder.Application.Models
{
    /// <summary>
    /// Classe base para parâmetros de busca com paginação
    /// </summary>
    public class SearchParameters
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        /// <summary>
        /// Número da página (começa em 1)
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Tamanho da página (quantidade de itens por página)
        /// Máximo: 50 itens
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        /// <summary>
        /// Campo para ordenação (ex: "titulo", "data", "nota")
        /// </summary>
        public string? OrderBy { get; set; }

        /// <summary>
        /// Define se a ordenação é descendente (true) ou ascendente (false)
        /// </summary>
        public bool OrderDescending { get; set; } = false;
    }
}