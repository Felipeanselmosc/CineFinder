namespace CineFinder.Application.Models
{
    /// <summary>
    /// Parâmetros de busca específicos para Gêneros
    /// </summary>
    public class GeneroSearchParameters : SearchParameters
    {
        /// <summary>
        /// Busca por nome do gênero (busca parcial, case-insensitive)
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Filtra apenas gêneros ativos/inativos
        /// </summary>
        public bool? Ativo { get; set; }
    }
}