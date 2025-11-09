namespace CineFinder.Application.Models
{
    /// <summary>
    /// Parâmetros de busca específicos para Listas
    /// </summary>
    public class ListaSearchParameters : SearchParameters
    {
        /// <summary>
        /// Filtra por ID do usuário
        /// </summary>
        public Guid? UsuarioId { get; set; }

        /// <summary>
        /// Busca por nome da lista (busca parcial, case-insensitive)
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Filtra apenas listas públicas/privadas
        /// </summary>
        public bool? Publica { get; set; }

        /// <summary>
        /// Data de criação inicial
        /// </summary>
        public DateTime? DataCriacaoInicio { get; set; }

        /// <summary>
        /// Data de criação final
        /// </summary>
        public DateTime? DataCriacaoFim { get; set; }
    }
}