using Blazor.News.Data.Models;
using Blazor.News.Data;
using Microsoft.EntityFrameworkCore;
using Blazor.News.DATA.ModelsDTO;

namespace Blazor.News.Services
{
    public class NieuwsService(NewsDBContext context) : INieuwsService
    {
        private readonly NewsDBContext _Context = context;

        // --- Nieuwe methode voor het ophalen van nieuwsartikelen met taalspecifieke data ---
        public async Task<PagedResult<NieuwsArtikelDto>> GetNieuwsArtikelenPagedAsync(string language, int pageNumber, int pageSize)
        {
            var isEnglish = language.Equals("en", StringComparison.CurrentCultureIgnoreCase);

            // Zorg dat pageNumber en pageSize geldig zijn
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 6; // Standaard 6 per pagina

            var query = _Context.NieuwsArtikelen
                .Include(a => a.Categorie)
                .Include(a => a.InterneLinks)
                .AsNoTracking();

            var totalCount = await query.CountAsync(); // Totaal aantal artikelen

            var artikelen = await query
                .OrderByDescending(a => a.PublicatieDatumTijd) // Sorteer altijd voor paginering
                .Skip((pageNumber - 1) * pageSize) // Sla de eerdere pagina's over
                .Take(pageSize) // Neem het aantal items voor deze pagina
                .Select(a => new NieuwsArtikelDto
                {
                    Id = a.Id,
                    Titel = isEnglish ? a.TitelEn : a.TitelNl,
                    Ondertitel = isEnglish ? a.OndertitelEn : a.OndertitelNl,
                    Auteur = a.Auteur,
                    PublicatieDatumTijd = a.PublicatieDatumTijd,
                    HoofdafbeeldingUrl = a.HoofdafbeeldingUrl,
                    VideoUrl = a.VideoUrl,
                    Inhoud = isEnglish ? a.OndertitelEn ?? (a.InhoudEn != null && a.InhoudEn.Length > 150 ? $"{a.InhoudEn.Substring(0, 150)}..." : a.InhoudEn)
                                       : a.OndertitelNl ?? (a.InhoudNl != null && a.InhoudNl.Length > 150 ? $"{a.InhoudNl.Substring(0, 150) }..." : a.InhoudNl),
                    CategorieNaam = isEnglish ? a.Categorie!.NaamEn : a.Categorie!.NaamNl,
                    InterneLinks = a.InterneLinks.Select(link => new NieuwsInternalLinkDto
                    {
                         
                        TekstEn = link.TekstEn,
                        TekstNl = link.TekstNl,
                        Url = link.Url
                    }).ToList()
                })
                .ToListAsync();

            return new PagedResult<NieuwsArtikelDto>
            {
                Items = artikelen,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<NieuwsArtikelDto?> GetNieuwsArtikelByIdAsync(int id, string language)
        {
            bool isEnglish = string.Equals(language, "en", StringComparison.OrdinalIgnoreCase);


            var artikel = await _Context.NieuwsArtikelen
                .Include(a => a.Categorie)
                .Include(a => a.InterneLinks)
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new NieuwsArtikelDto
                {
                    Id = a.Id,
                    Titel = isEnglish ? a.TitelEn : a.TitelNl,
                    Ondertitel = isEnglish ? a.OndertitelEn : a.OndertitelNl,
                    Auteur = a.Auteur,
                    PublicatieDatumTijd = a.PublicatieDatumTijd,
                    HoofdafbeeldingUrl = a.HoofdafbeeldingUrl,
                    VideoUrl = a.VideoUrl,
                    Inhoud = isEnglish ? a.InhoudEn : a.InhoudNl,
                    CategorieNaam = isEnglish ? a.Categorie!.NaamEn : a.Categorie!.NaamNl,
                    InterneLinks = a.InterneLinks.Select(link => new NieuwsInternalLinkDto
                    {
                         
                        TekstEn = link.TekstEn,
                        TekstNl = link.TekstNl,
                        Url = link.Url
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return artikel;
        }

        public async Task<NieuwsArtikelEditDto?> GetNieuwsArtikelForEditAsync(int id, string adminLanguage)
        {
            
            var artikel = await _Context.NieuwsArtikelen
                .Include(a => a.InterneLinks)
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new NieuwsArtikelEditDto
                {
                    Id = a.Id,
                    TitelNl = a.TitelNl,
                    TitelEn = a.TitelEn,
                    OndertitelNl = a.OndertitelNl,
                    OndertitelEn = a.OndertitelEn,
                    Auteur = a.Auteur,
                    PublicatieDatumTijd = a.PublicatieDatumTijd,
                    HoofdafbeeldingUrl = a.HoofdafbeeldingUrl,
                    VideoUrl = a.VideoUrl,
                    InhoudNl = a.InhoudNl,
                    InhoudEn = a.InhoudEn,
                    CategorieId = a.CategorieId,
                    InterneLinks = a.InterneLinks.Select(link => new NieuwsInternalLinkDto
                    {
                        
                        TekstEn = link.TekstEn,
                        TekstNl = link.TekstNl,
                        Url = link.Url
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (artikel != null)
            {
                // Vul de beschikbare categorieën in de taal van de admin interface
                artikel.BeschikbareCategorieen = [..(await GetNieuwsCategorieenForAdminAsync(adminLanguage))];
            }

            return artikel;
        }

        // NIEUW: Categorieën ophalen voor admin dropdowns
        public async Task<IEnumerable<NieuwsCategorieDto>> GetNieuwsCategorieenForAdminAsync(string adminLanguage)
        {
            var isEnglish = adminLanguage.Equals("en", StringComparison.CurrentCultureIgnoreCase);
            return await _Context.NieuwsCategorieen
                .AsNoTracking()
                .Select(c => new NieuwsCategorieDto
                {
                    Id = c.Id,
                    Naam = isEnglish ? c.NaamEn : c.NaamNl
                })
                .OrderBy(c => c.Naam)
                .ToListAsync();
        }

        // NIEUW: Voeg een nieuwsartikel toe vanuit de DTO
        public async Task AddNieuwsArtikelFromDtoAsync(NieuwsArtikelEditDto dto)
        {
            var artikel = new NieuwsArtikel
            {
                TitelNl = dto.TitelNl,
                TitelEn = dto.TitelEn,
                OndertitelNl = dto.OndertitelNl,
                OndertitelEn = dto.OndertitelEn,
                Auteur = dto.Auteur,
                PublicatieDatumTijd = dto.PublicatieDatumTijd,
                HoofdafbeeldingUrl = dto.HoofdafbeeldingUrl,
                VideoUrl = dto.VideoUrl,
                InhoudNl = dto.InhoudNl,
                InhoudEn = dto.InhoudEn,
                CategorieId = dto.CategorieId!.Value,
                InterneLinks = [.. dto.InterneLinks.Select(link => new NieuwsInternalLink
                {
                    TekstNl = link.TekstNl, // afhankelijk van je model, of:
                    TekstEn = link.TekstEn, // beide vullen, of apart splitsen op taal
                    Url = link.Url
                })]
            };
            _Context.NieuwsArtikelen.Add(artikel);
            await _Context.SaveChangesAsync();
        }

        // NIEUW: Update een nieuwsartikel vanuit de DTO
        public async Task UpdateNieuwsArtikelFromDtoAsync(NieuwsArtikelEditDto dto)
        {
            var artikel = await _Context.NieuwsArtikelen
                         .Include(a => a.InterneLinks)
                         .FirstOrDefaultAsync(a => a.Id == dto.Id)
                         ?? throw new InvalidOperationException($"Nieuwsartikel met ID {dto.Id} niet gevonden.");


            artikel.TitelNl = dto.TitelNl;
            artikel.TitelEn = dto.TitelEn;
            artikel.OndertitelNl = dto.OndertitelNl;
            artikel.OndertitelEn = dto.OndertitelEn;
            artikel.Auteur = dto.Auteur;
            artikel.PublicatieDatumTijd = dto.PublicatieDatumTijd;
            artikel.HoofdafbeeldingUrl = dto.HoofdafbeeldingUrl;
            artikel.VideoUrl = dto.VideoUrl;
            artikel.InhoudNl = dto.InhoudNl;
            artikel.InhoudEn = dto.InhoudEn;
            artikel.CategorieId = dto.CategorieId!.Value;
             _Context.NieuwsInternalLinks.RemoveRange(artikel.InterneLinks);
            artikel.InterneLinks = [.. dto.InterneLinks.Select(link => new NieuwsInternalLink
            {
                TekstNl = link.TekstNl, // afhankelijk van je model, of:
                TekstEn = link.TekstEn, // beide vullen, of apart splitsen op taal
                Url = link.Url
            })];

            _Context.NieuwsArtikelen.Update(artikel);
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteNieuwsArtikelAsync(int id)
        {
            var artikel = await _Context.NieuwsArtikelen.FindAsync(id);
            if (artikel != null)
            {
                _Context.NieuwsArtikelen.Remove(artikel);
                await _Context.SaveChangesAsync();
            }
        }
    }
}
   
