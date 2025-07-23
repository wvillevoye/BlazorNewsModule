using System.ComponentModel.DataAnnotations;

namespace Blazor.News.Data.Models
{
    public class NieuwsArtikel
    {
        [Key]
        public int Id { get; set; }

        // Taal-specifieke velden
        [Required(ErrorMessage = "Een titel (NL) is verplicht.")]
        [MaxLength(250, ErrorMessage = "De titel (NL) mag maximaal 250 tekens lang zijn.")]
        public string? TitelNl { get; set; } // Nederlandse titel

        [MaxLength(250, ErrorMessage = "De titel (EN) mag maximaal 250 tekens lang zijn.")]
        public string? TitelEn { get; set; } // Engelse titel

        [MaxLength(500, ErrorMessage = "De ondertitel (NL) mag maximaal 500 tekens lang zijn.")]
        public string? OndertitelNl { get; set; } // Nederlandse ondertitel

        [MaxLength(500, ErrorMessage = "De ondertitel (EN) mag maximaal 500 tekens lang zijn.")]
        public string? OndertitelEn { get; set; } // Engelse ondertitel

        // Inhoud van het nieuwsartikel
        [Required(ErrorMessage = "De inhoud (NL) van het artikel is verplicht.")]
        public string? InhoudNl { get; set; } // Nederlandse inhoud (HTML)

        public string? InhoudEn { get; set; } // Engelse inhoud (HTML)

        // Auteur is vaak taal-onafhankelijk, maar kan ook taal-specifiek zijn indien gewenst
        [Required(ErrorMessage = "De auteur is verplicht.")]
        [MaxLength(100, ErrorMessage = "De auteursnaam mag maximaal 100 tekens lang zijn.")]
        public string? Auteur { get; set; }

        [Required]
        public DateTime PublicatieDatumTijd { get; set; }

        [MaxLength(1000, ErrorMessage = "De URL van de hoofdafbeelding mag maximaal 1000 tekens lang zijn.")]
        public string? HoofdafbeeldingUrl { get; set; }

        [MaxLength(1000, ErrorMessage = "De URL van de video mag maximaal 1000 tekens lang zijn.")]
        public string? VideoUrl { get; set; }

        public int CategorieId { get; set; }
        public NieuwsCategorie? Categorie { get; set; }

        public ICollection<NieuwsInternalLink> InterneLinks { get; set; } = []; // Let op: deze links kunnen ook taal-specifiek moeten zijn!

        // Hulp-methode om de juiste titel te krijgen op basis van de huidige cultuur
        public string GetTitel(string culture)
        {
            return culture.Equals("en", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(TitelEn) ? TitelEn : TitelNl ?? "";
        }
        public string GetOndertitel(string culture)
        {
            return culture.Equals("en", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(OndertitelEn) ? OndertitelEn : OndertitelNl ?? "";
        }
        public string GetInhoud(string culture)
        {
            return culture.Equals("en", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(InhoudEn) ? InhoudEn : InhoudNl ?? "";
        }
    }
}
