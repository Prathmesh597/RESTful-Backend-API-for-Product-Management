using FluentValidation;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Validators
{
    public class UpdateItemValidator : AbstractValidator<UpdateItemDto>
    {
        public UpdateItemValidator()
        {
            // 1. Enforces validation rules for the item quantity, ensuring it is a valid positive amount
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        }
    }
}