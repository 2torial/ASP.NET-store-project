using FluentValidation;

namespace ASP.NET_store_project.Server.Controllers.IdentityController
{
    public record SignInModel(string UserName, string PassWord);

    public class SignInValidator : AbstractValidator<SignInModel>
    {
        public SignInValidator()
        {
            RuleFor(m => m.UserName).NotEmpty().WithMessage("Your username cannot be empty");

            RuleFor(m => m.PassWord).NotEmpty().WithMessage("Your password cannot be empty");

        }
    }

}
