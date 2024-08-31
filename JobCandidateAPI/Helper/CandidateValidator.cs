using FluentValidation;
using JobCandidateAPI.Models;

namespace JobCandidateAPI.Helper
{
    public class CandidateValidator : AbstractValidator<Candidate>
    {
        public CandidateValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Valid email is required.");
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Comment is required.");
            RuleFor(x => x.PhoneNumber).Matches(@"^\d{3}-\d{3}-\d{4}$").WithMessage("Phone number must be in the format xxx-xxx-xxxx");
            RuleFor(x => x.LinkedInProfileUrl).Must(IsValidUrl).When(x => !string.IsNullOrEmpty(x.LinkedInProfileUrl)).WithMessage("LinkedIn URL must be a valid URL.");
            RuleFor(x => x.GitHubProfileUrl).Must(IsValidUrl).When(x => !string.IsNullOrEmpty(x.GitHubProfileUrl)).WithMessage("GitHub URL must be a valid URL.");
        }

        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}