using Book_MinimalAPI.Models.DTOs;
using FluentValidation;

namespace Book_MinimalAPI.Validators
{
    public class BookCreateValidator : AbstractValidator<BookDTO>
    {
        public BookCreateValidator()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Name).Length(1, 60);
            RuleFor(model => model.Author).NotEmpty();
            RuleFor(model => model.Author).Length(1, 60);
            RuleFor(model => model.Available).NotEmpty();
        }
    }
}
