
using FluentValidation.Results;
using WebApplication1.Model;

namespace WebApplication1.Validations
{
    public static class ValidationsRequest
    {
        public static void ValidateBody(Employee apiRequest)
        {
            if (apiRequest is null)
            {
                throw new ArgumentNullException("Invalid request.", new Exception("Request Body must be in valid format."));
            }

            
            ValidationResult requestValidationResult = new ValidateRequest().Validate(apiRequest);

            if (!requestValidationResult.IsValid)
            {
                throw new ArgumentNullException(string.Join("|", requestValidationResult.Errors));
            }
        }
    }
}
