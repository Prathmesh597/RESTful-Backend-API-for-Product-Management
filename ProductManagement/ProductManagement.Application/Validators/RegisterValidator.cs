using FluentValidation;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        // 1. Sets up the validation rules for the user registration data payload
        public RegisterValidator()
        {
            // 2. Ensures the username is provided and falls within the acceptable 3 to 50 character limit
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            // 3. Validates that the email field is not empty and follows a standard email address format
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            // 4. Requires a password to be present and contain a minimum of 6 characters for basic security
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            // 5. Restricts the role assignment strictly to predefined 'Admin' or 'User' values
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(x => x == "Admin" || x == "User")
                .WithMessage("Role must be either 'Admin' or 'User'.");

        }

    }

}