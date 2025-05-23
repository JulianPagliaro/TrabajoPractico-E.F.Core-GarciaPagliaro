using EF.Entities;
using FluentValidation;

namespace EF.Services.Validators
{
    public class GameValidator : AbstractValidator<Game>
    {
        public GameValidator()
        {
            RuleFor(g => g.Title).NotEmpty().WithMessage("The {PropertyName} is required")
               .MaximumLength(300).WithMessage("The {PropertyName} must have no more than {ComparisonValue} characters");

            RuleFor(g => g.Genre).NotEmpty().WithMessage("The {PropertyName} is required")
                .MaximumLength(100).WithMessage("The {PropertyName} must have no more than {ComparisonValue} characters");

            RuleFor(g => g.PublishDate).NotEmpty().WithMessage("The {PropertyName} is required")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("The {PropertyName} must be less than or equal to {ComparasionValue}");

            RuleFor(g => g.Price).NotNull().WithMessage("The {PropertyName} is required")
            .GreaterThanOrEqualTo(10).WithMessage("The {PropertyName} must be at least {ComparisonValue}")
            .LessThanOrEqualTo(60).WithMessage("The {PropertyName} must be less than or equal to {ComparisonValue}");

            RuleFor(b => b.DeveloperId).GreaterThan(0).WithMessage("The field {PropertyName} must be greater than {ComparisonValue}");
            When(b => b.Id == 0, () =>
            {
                RuleFor(b => b.Id).Equal(0).WithMessage("When adding a new Game, GameId must be {ComparisonValue}");
            }).Otherwise(() =>
            {
                RuleFor(b => b.Id).GreaterThan(0).WithMessage("The field {PropertyName} must be greater than {ComparisonValue}");
            });
        }

    } 
}
