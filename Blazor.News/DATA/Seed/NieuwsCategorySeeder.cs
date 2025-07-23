using Blazor.News.Data.Models;

namespace Blazor.News.Data.Seed
{
    public static class NieuwsCategorySeeder
    {

        public static void SeedCategories(NewsDBContext context)
        {
            if (!context.NieuwsCategorieen.Any())
            {
                var categories = new List<NieuwsCategorie>
                {
                    new() { NaamNl = "Algemeen Nieuws", NaamEn = "General News" },
                    new() { NaamNl = "Technologie", NaamEn = "Technology" },
                    new() { NaamNl = "Economie", NaamEn = "Economy" },
                    new() { NaamNl = "Lokaal Nieuws", NaamEn = "Local News" },
                    new() { NaamNl = "Sport", NaamEn = "Sports" },
                    new() { NaamNl = "Cultuur", NaamEn = "Culture" },
                    new() { NaamNl = "Wetenschap", NaamEn = "Science" },
                    new() { NaamNl = "Politiek", NaamEn = "Politics" },
                    new() { NaamNl = "Gezondheid", NaamEn = "Health" },
                    new() { NaamNl = "Milieu", NaamEn = "Environment" }
                };

                context.NieuwsCategorieen.AddRange(categories);
                context.SaveChanges();
            }
        }


    }
}
