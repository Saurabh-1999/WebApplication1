using FluentValidation;
using WebApplication1.Model;

namespace WebApplication1.Validations
{
    
    public class ValidateRequest : AbstractValidator<Employee>
    {
        public ValidateRequest()
        {
            RuleFor(e => e.Id).NotEmpty().NotNull().Must(e => e > 0)
                 .WithMessage("ID must be a positive number.");

            RuleFor(e => e.Name).NotNull()
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");

            RuleFor(e => e.Email).NotNull()
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(e => e.Position).NotNull()
                .NotEmpty().WithMessage("Position is required.")
                .Length(2, 100).WithMessage("Position must be between 2 and 100 characters.");

            RuleFor(e => e.Salary).NotNull()
                .GreaterThan(0).WithMessage("Salary must be greater than zero.")
                .ScalePrecision(2, 18).WithMessage("Salary can have up to 18 digits and 2 decimal places.");
        }
    }
}
