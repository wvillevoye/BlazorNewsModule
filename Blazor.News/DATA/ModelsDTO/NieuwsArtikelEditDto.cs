using Blazor.News.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Blazor.News.DATA.ModelsDTO
{
    public class NieuwsArtikelEditDto
    {
        public int Id { get; set; } // Voor bewerken

        public string? TitelNl { get; set; }
        public string? TitelEn { get; set; }
        public string? OndertitelNl { get; set; }
        public string? OndertitelEn { get; set; }
        public string? Auteur { get; set; }
        public DateTime PublicatieDatumTijd { get; set; } = DateTime.Now;
        public string? HoofdafbeeldingUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? InhoudNl { get; set; }
        public string? InhoudEn { get; set; }
        public int? CategorieId { get; set; }


        // Om geselecteerde categorieën te tonen in een dropdown
        public List<NieuwsCategorieDto> BeschikbareCategorieen { get; set; } = [];
        public List<NieuwsInternalLinkDto> InterneLinks { get; set; } = [];
    }
}
