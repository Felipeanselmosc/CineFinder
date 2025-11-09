namespace CineFinder.Application.Models
{
    /// <summary>
    /// Parâmetros de busca específicos para Filmes
    /// </summary>
    public class FilmeSearchParameters : SearchParameters
    {
        /// <summary>
        /// Busca por título do filme (busca parcial, case-insensitive)
        /// </summary>
        public string? Titulo { get; set; }

        /// <summary>
        /// Filtra por ID do gênero
        /// </summary>
        public Guid? GeneroId { get; set; }

        /// <summary>
        /// Ano inicial de lançamento (inclusive)
        /// </summary>
        public int? AnoInicio { get; set; }

        /// <summary>
        /// Ano final de lançamento (inclusive)
        /// </summary>
        public int? AnoFim { get; set; }

        /// <summary>
        /// Nota média mínima (0-10)
        /// </summary>
        public int? NotaMinimaMedia { get; set; }

        /// <summary>
        /// Duração mínima em minutos
        /// </summary>
        public int? DuracaoMinima { get; set; }

        /// <summary>
        /// Duração máxima em minutos
        /// </summary>
        public int? DuracaoMaxima { get; set; }

        /// <summary>
        /// Busca por nome do diretor (busca parcial, case-insensitive)
        /// </summary>
        public string? Diretor { get; set; }

        /// <summary>
        /// Busca por múltiplos gêneros (OR lógico)
        /// </summary>
        public List<Guid>? GenerosIds { get; set; }
    }
}