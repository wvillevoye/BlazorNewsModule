using System.ComponentModel.DataAnnotations;

namespace Blazor.News.Data.Models
{
    public class NieuwsCategorie
    {
        [Key]
        public int Id { get; set; }

        // Taal-specifieke velden voor categorienaam
        [Required(ErrorMessage = "De categorienaam (NL) is verplicht.")]
        [MaxLength(100, ErrorMessage = "De categorienaam (NL) mag maximaal 100 tekens lang zijn.")]
        public string? NaamNl { get; set; } // Nederlandse categorienaam

        [MaxLength(100, ErrorMessage = "De categorienaam (EN) mag maximaal 100 tekens lang zijn.")]
        public string? NaamEn { get; set; } // Engelse categorienaam

        public ICollection<NieuwsArtikel> NieuwsArtikelen { get; set; } = [];

        // Hulp-methode om de juiste categorienaam te krijgen
        public string GetNaam(string culture)
        {
            return culture.Equals("en", StringComparison.CurrentCultureIgnoreCase) && !string.IsNullOrEmpty(NaamEn) ? NaamEn : NaamNl ?? "";
        }
    }
}
