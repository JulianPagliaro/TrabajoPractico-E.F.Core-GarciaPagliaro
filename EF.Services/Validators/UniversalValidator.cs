using FluentValidation;

namespace EF.Services.Validators
{
    public static class UniversalValidator
    {
        public static bool IsValid<T>(T entity, IValidator<T> validator, out List<string> errors)
        {
            var result = validator.Validate(entity);
            errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            return result.IsValid;
        }
    }
}
