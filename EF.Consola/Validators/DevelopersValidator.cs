using EF.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Consola.Validators
{
    public class DevelopersValidator : AbstractValidator<Developer>
    {
        public DevelopersValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The field {PropertyName} is required")
                .Length(3, 50).WithMessage("The field {PropertyName} must be between {MinLength} and {MaxLength}");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("The field {PropertyName} is required")
                .Length(3, 50).WithMessage("The field {PropertyName} must be between {MinLength} and {MaxLength}");

            RuleFor(x => x.FoundationDate)
                .NotEmpty().WithMessage("The {PropertyName} is required")
                .GreaterThanOrEqualTo(new DateOnly(1979, 1, 1))
                .WithMessage("The {PropertyName} cannot be earlier than January 1, 1979")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.Date))
                .WithMessage("The {PropertyName} cannot be a future date");

            When(b => b.Id == 0, () =>
            {
                RuleFor(b => b.Id).Equal(0).WithMessage("When adding a new Author, AuthorId must be {ComparisonValue}");
            }).Otherwise(() =>
            {
                RuleFor(b => b.Id).GreaterThan(0).WithMessage("The field {PropertyName} must be greater than {ComparisonValue}");
            });
        }
    }
}
