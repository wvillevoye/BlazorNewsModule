using Blazor.News.Data.Models;

namespace Blazor.News.Data.Seed
{
    public static class NieuwsArtikelSeeder
    {
        public static void SeedNieuwsArtikelen(NewsDBContext context)
        {
            if (context.NieuwsArtikelen.Any())
            {
                return;
            }

            var categorien = context.NieuwsCategorieen.ToList();

            if (categorien.Count == 0)
            {
                Console.WriteLine("WAARSCHUWING: Geen categorieën gevonden om nieuwsartikelen aan te koppelen. Voer CategorySeeder uit.");
                return;
            }

            var random = new Random();
            var nieuwsArtikelen = new List<NieuwsArtikel>();

            for (int i = 1; i <= 12; i++)
            {
                var randomCategory = categorien[random.Next(categorien.Count)];
                var publicatieDatum = DateTime.Now.AddDays(-random.Next(1, 365));
                var publicatieTijd = publicatieDatum.AddHours(random.Next(0, 24)).AddMinutes(random.Next(0, 60));

                int imageId = 100 + i;
                if (i > 6) imageId += 50;

                var artikel = new NieuwsArtikel
                {
                    TitelNl = $"Nieuwsbericht nummer {i}: Doorbraak in {randomCategory.NaamNl}",
                    TitelEn = $"News Report Number {i}: Breakthrough in {randomCategory.NaamEn}",
                    OndertitelNl = $"De nieuwste inzichten uit de wereld van {randomCategory.NaamNl} beloven grote vooruitgang.",
                    OndertitelEn = $"The latest insights from the world of {randomCategory.NaamEn} promise great progress.",
                    Auteur = (i % 2 == 0) ? "M. Jansen" : "T. de Bruin",
                    PublicatieDatumTijd = publicatieTijd,
                    HoofdafbeeldingUrl = $"https://picsum.photos/id/{imageId}/1200/600",
                    VideoUrl = (i % 3 == 0) ? "https://www.w3schools.com/html/mov_bbb.mp4" : null,
                    InhoudNl = $@"
                    <p>Dit is de uitgebreide inhoud van nieuwsartikel nummer {i}. Het beschrijft de diepere details van de revolutionaire ontwikkeling binnen de **{randomCategory.NaamNl}** sector. Experts verwachten grote veranderingen.</p>
                    <blockquote>
                        <p>""De implicaties van deze bevindingen zijn enorm en zullen de manier waarop we {randomCategory.NaamNl} benaderen, voorgoed veranderen.""</p>
                        <footer>Dr. A. Schmidt, onderzoeker bij {randomCategory.NaamNl} Instituut</footer>
                    </blockquote>
                ",
                    InhoudEn = $@"
                    <p>This is the detailed content of news article number {i}. It describes the deeper details of the revolutionary development within the **{randomCategory.NaamEn}** sector. Experts expect major changes.</p>
                    <blockquote>
                        <p>""The implications of these findings are immense and will forever change the way we approach {randomCategory.NaamEn}.""</p>
                        <footer>Dr. A. Schmidt, researcher at {randomCategory.NaamEn} Institute</footer>
                    </blockquote>
                ",
                    CategorieId = randomCategory.Id,
                };

                if (i % 2 != 0)
                {
                    artikel.InterneLinks.Add(new NieuwsInternalLink { TekstNl = $"Meer over {randomCategory.NaamNl}", TekstEn = $"More about {randomCategory.NaamEn}", Url = $"https://example.com/{randomCategory.NaamEn.ToLower().Replace(" ", "-")}", NieuwsArtikel = artikel });
                }
                if (i % 3 == 0)
                {
                    artikel.InterneLinks.Add(new NieuwsInternalLink { TekstNl = $"Gerelateerd onderzoek {i}", TekstEn = $"Related Research {i}", Url = $"https://example.com/research/{i}", NieuwsArtikel = artikel });
                }

                nieuwsArtikelen.Add(artikel);
            }

            context.NieuwsArtikelen.AddRange(nieuwsArtikelen);
            context.SaveChanges();
        }
    }
}
