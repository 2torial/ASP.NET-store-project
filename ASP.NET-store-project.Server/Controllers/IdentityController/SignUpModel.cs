using FluentValidation;

namespace ASP.NET_store_project.Server.Controllers.IdentityController
{
    // FromFrom request data class
    public record SignUpModel(string UserName, string PassWord, string RetypedPassWord);

    public class SignUpValidator : AbstractValidator<SignUpModel>
    {
        public SignUpValidator()
        {
            RuleFor(m => m.UserName).NotEmpty().WithMessage("Your username cannot be empty")
                .MinimumLength(4).WithMessage("Your username length must be at least 4.")
                .MaximumLength(16).WithMessage("Your username length must not exceed 16.")
                .Matches(@"[a-zA-Z]+").WithMessage("Your username must start with a letter.")
                .Matches(@"[a-zA-Z0-9]+").WithMessage("Your password must consist solely of english alphabet letters and numbers.");

            RuleFor(m => m.PassWord).NotEmpty().WithMessage("Your password cannot be empty")
                .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");

            RuleFor(m => m.RetypedPassWord).Equal(m => m.PassWord).WithMessage("Both passwords provided must match.");
        }
    }

}
