using FluentValidation;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            // 1. Enforces validation rules for the product name, ensuring it is provided and does not exceed 255 characters
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(255).WithMessage("Product name cannot exceed 255 characters.");

            // 2. Enforces validation rules for the creator's identifier, ensuring it is provided and does not exceed 100 characters
            RuleFor(x => x.CreatedBy)
                .NotEmpty().WithMessage("CreatedBy is required.")
                .MaximumLength(100).WithMessage("CreatedBy cannot exceed 100 characters.");

        }
    }
}