
using AutoMapper;
using Book_MinimalAPI.Data;
using Book_MinimalAPI.Models;
using Book_MinimalAPI.Models.DTOs;
using Book_MinimalAPI.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using Book_MinimalAPI.Validators;

namespace Book_MinimalAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("BooksDBConnection")));

            builder.Services.AddValidatorsFromAssemblyContaining<BookCreateValidator>();
            builder.Services.AddAutoMapper(typeof(MappingConfig).Assembly);
            builder.Services.AddScoped<IBookRepository, BookRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var booksApi = app.MapGroup("/api/book");

            booksApi.MapGet("/", GetAllBooks).Produces<IEnumerable<Book>>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).WithName("GetAllBooks");

            booksApi.MapGet("/{id:int}", GetBook).Produces<Book>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).WithName("GetBook");

            booksApi.MapGet("/{author}", GetBooksFromAuthor).Produces<IEnumerable<Book>>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).WithName("GetBooksFromAuthor");

            booksApi.MapPost("/", CreateBook).Accepts<BookDTO>("application/json").Produces<Book>(StatusCodes.Status200OK).Produces(StatusCodes.Status400BadRequest).WithName("CreateBook");

            booksApi.MapPut("/{id:int}", UpdateBook).Accepts<BookDTO>("application/json").Produces<Book>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).WithName("UpdateBook");

            booksApi.MapDelete("/{id:int}", DeleteBook).Produces<Book>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).WithName("DeleteBook");

            app.Run();

            static async Task<IResult> GetAllBooks(IBookRepository bookRepo)
            {
                IEnumerable<Book> result = await bookRepo.GetAll();
                if (result is IEnumerable<Book>)
                {
                    return TypedResults.Ok(result);
                }

                return TypedResults.NotFound();
            }

            static async Task<IResult> GetBook(int id, IBookRepository bookRepo)
            {
                Book result = await bookRepo.GetSingle(id);
                if (result is Book)
                {
                    return TypedResults.Ok(result);
                }
                return TypedResults.NotFound();
            }

            static async Task<IResult> GetBooksFromAuthor(string authorName, IBookRepository bookRepo)
            {
                IEnumerable<Book> result = await bookRepo.GetFromAuthor(authorName);
                if (!result.IsNullOrEmpty())
                {
                    return TypedResults.Ok(result);
                }
                return TypedResults.NotFound();
            }

            static async Task<IResult> CreateBook(BookDTO bookDTO, IMapper _mapper, IValidator<BookDTO> _validator, IBookRepository bookRepo)
            {
                var validationResult = await _validator.ValidateAsync(bookDTO);
                if (!validationResult.IsValid)
                {
                    return TypedResults.BadRequest(validationResult.Errors);
                }

                Book bookToAdd = _mapper.Map<Book>(bookDTO);
                Book result = await bookRepo.Create(bookToAdd);
                if (result is Book)
                {
                    return TypedResults.Created($"/api/book/{result.Id}", result);
                }
                return TypedResults.BadRequest();
            }

            static async Task<IResult> UpdateBook(int id, BookDTO bookDTO, IMapper _mapper, IValidator<BookDTO> _validator, IBookRepository bookRepo)
            {
                var validationResult = await _validator.ValidateAsync(bookDTO);
                if (!validationResult.IsValid)
                {
                    return TypedResults.BadRequest(validationResult.Errors);
                }

                Book updatedBook = _mapper.Map<Book>(bookDTO);
                Book result = await bookRepo.Update(id, updatedBook);
                if (result is Book)
                {
                    return TypedResults.Ok(result);
                }
                return TypedResults.NotFound();
            }

            static async Task<IResult> DeleteBook(int id, IBookRepository bookRepo)
            {
                Book result = await bookRepo.Delete(id);
                if (result is Book)
                {
                    return TypedResults.Ok(result);
                }
                return TypedResults.NotFound();
            }
        }
    }
}