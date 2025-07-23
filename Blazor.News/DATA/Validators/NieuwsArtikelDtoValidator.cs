using Blazor.News.DATA.ModelsDTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.News.DATA.Validators
{
    public class NieuwsArtikelEditDtoValidator : AbstractValidator<NieuwsArtikelEditDto>
    {
        public NieuwsArtikelEditDtoValidator()
        {
            RuleFor(x => x.TitelNl)
                .NotEmpty().WithMessage("Titel (NL) is verplicht.")
                .MaximumLength(250).WithMessage("Titel (NL) mag maximaal 250 tekens zijn.");

            RuleFor(x => x.TitelEn)
                .MaximumLength(250).WithMessage("Titel (EN) mag maximaal 250 tekens zijn.");

            RuleFor(x => x.OndertitelNl)
                .MaximumLength(500).WithMessage("Ondertitel (NL) mag maximaal 500 tekens zijn.");

            RuleFor(x => x.OndertitelEn)
                .MaximumLength(500).WithMessage("Ondertitel (EN) mag maximaal 500 tekens zijn.");

            RuleFor(x => x.Auteur)
                .NotEmpty().WithMessage("Auteur is verplicht.")
                .MaximumLength(100).WithMessage("Auteur mag maximaal 100 tekens zijn.");

            RuleFor(x => x.PublicatieDatumTijd)
                .NotEmpty().WithMessage("Publicatiedatum is verplicht.");

            RuleFor(x => x.HoofdafbeeldingUrl)
                .MaximumLength(1000).WithMessage("Hoofdafbeelding URL mag maximaal 1000 tekens zijn.")
                .Must(BeAValidUrl) // Valideer als URL
                .When(x => !string.IsNullOrEmpty(x.HoofdafbeeldingUrl)) // Alleen valideren als het niet leeg is
                .WithMessage("Ongeldige URL voor hoofdafbeelding.");

            // Hier is de specifieke regel voor VideoUrl die we eerder bespraken:
            RuleFor(x => x.VideoUrl)
                .MaximumLength(1000).WithMessage("Video URL mag maximaal 1000 tekens zijn.")
                .Must(BeAValidUrl) // Valideer als URL
                .When(x => !string.IsNullOrEmpty(x.VideoUrl)) // Alleen valideren als het NIET leeg is
                .WithMessage("Ongeldige URL voor video.");

            RuleFor(x => x.InhoudNl)
                .NotEmpty().WithMessage("Inhoud (NL) is verplicht.");

            // InhoudEn mag leeg zijn, dus geen NotEmpty(), alleen max length als relevant.

            RuleFor(x => x.CategorieId)
                .NotEmpty().WithMessage("Categorie is verplicht.")
                .GreaterThan(0).WithMessage("Selecteer een geldige categorie."); // Optioneel: als 0 een ongeldige default is

            // Voor interne links (als je die ook wilt valideren)
            // RuleForEach(x => x.InterneLinks).SetValidator(new NieuwsInternalLinkDtoValidator());
        }

        // Helper-methode voor URL validatie
        private bool BeAValidUrl(string? url)
        {
            // FluentValidation's When() clausule handelt de "leeg" check af,
            // maar een extra defensieve check hier kan geen kwaad.
            if (string.IsNullOrEmpty(url)) return true;

            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
