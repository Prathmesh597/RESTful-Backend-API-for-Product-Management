using FluentValidation;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Validators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        // 1. Initializes the validation rules for the user login request payload
        public LoginValidator()
        {
            // 2. Ensures the username field is provided and returns a specific error message if left blank
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.");

            // 3. Verifies that the password field is not empty before allowing the authentication process to continue
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");

        }

    }

}