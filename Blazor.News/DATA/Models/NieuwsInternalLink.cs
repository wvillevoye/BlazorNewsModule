using System.ComponentModel.DataAnnotations;

namespace Blazor.News.Data.Models
{
    // NieuwsInternalLink zou ook taal-specifiek kunnen zijn
    public class NieuwsInternalLink
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? TekstNl { get; set; } // Nederlandse linktekst

        [MaxLength(200)]
        public string? TekstEn { get; set; } // Engelse linktekst

        [Required]
        [MaxLength(1000)]
        public string? Url { get; set; } // URL is meestal taal-onafhankelijk

        public int NieuwsArtikelId { get; set; }
        public NieuwsArtikel? NieuwsArtikel { get; set; }

        public string GetTekst(string culture)
        {
            return culture.Equals("en", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(TekstEn) ? TekstEn : TekstNl ?? "";
        }
    }
}
