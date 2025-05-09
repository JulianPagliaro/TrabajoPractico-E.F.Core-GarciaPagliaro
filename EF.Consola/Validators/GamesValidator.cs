﻿using EF.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Consola.Validators
{
    public class GamesValidator:AbstractValidator<Game>
    {
        public GamesValidator()
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


        }
    }
}
