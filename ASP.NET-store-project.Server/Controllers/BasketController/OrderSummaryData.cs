namespace ASP.NET_store_project.Server.Controllers.BasketController;

using Data.Enums;
using FluentValidation;
using Models.StructuredData;
using System.ComponentModel;

// FromBody request data
public record OrderSummaryData(IEnumerable<Order> Orders, CustomerInfo CustomerDetails, AdressInfo AdressDetails);

public record Order(IEnumerable<Guid> ProductBasketIds, DeliveryMethod DeliveryMethod)
{
    // Transform recieved DeliveryMethod into decimal format
    public decimal DecimalDeliveryCost() => DeliveryMethod switch
    {
        DeliveryMethod.Standard => 5,
        DeliveryMethod.Express => 25,
        _ => throw new InvalidEnumArgumentException()
    };
};

// Validation ruleset
public class OrderSummaryDataValidator : AbstractValidator<OrderSummaryData>
{
    public OrderSummaryDataValidator()
    {
        RuleFor(m => m.Orders).NotEmpty();
        RuleForEach(m => m.Orders).NotEmpty().ChildRules(n =>
        {
            n.RuleForEach(m => m.ProductBasketIds).NotEmpty();
            n.RuleFor(m => m.DeliveryMethod).NotNull();
        });
        RuleFor(m => m.CustomerDetails).NotEmpty().ChildRules(n =>
        {
            n.RuleFor(m => m.Name).NotEmpty().WithMessage("Your name cannot be empty");
            n.RuleFor(m => m.Surname).NotEmpty().WithMessage("Your surname cannot be empty");
            n.RuleFor(m => m.PhoneNumber).NotEmpty().WithMessage("Your phone number cannot be empty")
                .Matches("[0-9]+").WithMessage("Only numbers are allowed");
            n.RuleFor(m => m.Email).EmailAddress().WithMessage("Inappropriate e-mail adress");
        });
        RuleFor(m => m.AdressDetails).ChildRules(n =>
        {
            n.RuleFor(m => m.Region).NotEmpty().WithMessage("Region cannot be empty");
            n.RuleFor(m => m.City).NotEmpty().WithMessage("City cannot be empty");
            n.RuleFor(m => m.PostalCode).NotEmpty().WithMessage("Postal code cannot be empty");
            n.RuleFor(m => m.StreetName).NotEmpty().WithMessage("Street name cannot be empty");
            n.RuleFor(m => m.HouseNumber).NotEmpty().WithMessage("House number cannot be empty");
        });
    }
}
