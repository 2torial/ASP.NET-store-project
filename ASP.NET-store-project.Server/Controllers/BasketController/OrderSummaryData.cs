using FluentValidation;

namespace ASP.NET_store_project.Server.Controllers.BasketController
{
    public record OrderSummaryData(
        string Name, 
        string Surname, 
        string PhoneNumber, 
        string Email, 
        string Region, 
        string City, 
        string PostalCode, 
        string StreetName, 
        string HouseNumber, 
        string? ApartmentNumber);

    public class OrderSummaryDataValidator : AbstractValidator<OrderSummaryData>
    {
        public OrderSummaryDataValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Your name cannot be empty");
            RuleFor(m => m.Surname).NotEmpty().WithMessage("Your surname cannot be empty");
            RuleFor(m => m.PhoneNumber).NotEmpty().WithMessage("Your phone number cannot be empty")
                .Matches("[0-9]+").WithMessage("Only numbers are allowed");
            RuleFor(m => m.Email).EmailAddress().WithMessage("Inappropriate e-mail adress");
            RuleFor(m => m.Region).NotEmpty().WithMessage("Region cannot be empty");
            RuleFor(m => m.StreetName).NotEmpty().WithMessage("Street name cannot be empty");
            RuleFor(m => m.HouseNumber).NotEmpty().WithMessage("House number cannot be empty");
        }
    }
}
