namespace Blazor.News.DATA.ModelsDTO
{
    public class NieuwsArtikelDto
    {
        public int Id { get; set; }
        public string? Titel { get; set; }
        public string? Ondertitel { get; set; }
        public string? Auteur { get; set; }
        public DateTime PublicatieDatumTijd { get; set; }
        public string? HoofdafbeeldingUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? Inhoud { get; set; }
        public string? CategorieNaam { get; set; } // Naam van de categorie in de gekozen taal
        public List<NieuwsInternalLinkDto> InterneLinks { get; set; } = []; // DTO voor interne links
    }
}
