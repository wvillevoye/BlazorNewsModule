
using Blazor.News.DATA.ModelsDTO;

namespace Blazor.News.Services
{
    public interface INieuwsService
    {
        // Aangepaste methode voor paginering
        Task<PagedResult<NieuwsArtikelDto>> GetNieuwsArtikelenPagedAsync(string language, int pageNumber, int pageSize);
        Task<NieuwsArtikelDto?> GetNieuwsArtikelByIdAsync(int id, string language);
        Task<NieuwsArtikelEditDto?> GetNieuwsArtikelForEditAsync(int id, string adminLanguage); // Admin taal voor dropdown
        Task AddNieuwsArtikelFromDtoAsync(NieuwsArtikelEditDto dto);
        Task UpdateNieuwsArtikelFromDtoAsync(NieuwsArtikelEditDto dto);
        Task DeleteNieuwsArtikelAsync(int id); // Deze hadden we al, maar herbevestig
        Task<IEnumerable<NieuwsCategorieDto>> GetNieuwsCategorieenForAdminAsync(string adminLanguage);
       

    }
}
