namespace CineFinder.Application.Models
{
    /// <summary>
    /// Parâmetros de busca específicos para Usuários
    /// </summary>
    public class UsuarioSearchParameters : SearchParameters
    {
        /// <summary>
        /// Busca por nome do usuário (busca parcial, case-insensitive)
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Busca por email (busca parcial, case-insensitive)
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Data de cadastro inicial
        /// </summary>
        public DateTime? DataCadastroInicio { get; set; }

        /// <summary>
        /// Data de cadastro final
        /// </summary>
        public DateTime? DataCadastroFim { get; set; }

        /// <summary>
        /// Filtra por gênero preferido
        /// </summary>
        public Guid? GeneroPreferidoId { get; set; }
    }
}