namespace CineFinder.Application.Models
{
    /// <summary>
    /// Parâmetros de busca específicos para Avaliações
    /// </summary>
    public class AvaliacaoSearchParameters : SearchParameters
    {
        /// <summary>
        /// Filtra por ID do filme
        /// </summary>
        public Guid? FilmeId { get; set; }

        /// <summary>
        /// Filtra por ID do usuário
        /// </summary>
        public Guid? UsuarioId { get; set; }

        /// <summary>
        /// Nota mínima (1-10)
        /// </summary>
        public int? NotaMinima { get; set; }

        /// <summary>
        /// Nota máxima (1-10)
        /// </summary>
        public int? NotaMaxima { get; set; }

        /// <summary>
        /// Data de avaliação inicial
        /// </summary>
        public DateTime? DataAvaliacaoInicio { get; set; }

        /// <summary>
        /// Data de avaliação final
        /// </summary>
        public DateTime? DataAvaliacaoFim { get; set; }

        /// <summary>
        /// Filtra apenas avaliações com comentário
        /// </summary>
        public bool? TemComentario { get; set; }
    }
}