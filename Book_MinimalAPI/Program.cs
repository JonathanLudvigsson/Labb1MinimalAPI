
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
using Microsoft.AspNetCore.Mvc;

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

            builder.Services.AddCors((setup) =>
            {
                setup.AddPolicy("default", (options) =>
                {
                    options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                });
            });

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

            booksApi.MapGet("/", GetAllBooks)
                .Produces<IEnumerable<Book>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetAllBooks");

            booksApi.MapGet("/{id:int}", GetBook)
                .Produces<Book>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetBook");

            booksApi.MapGet("/{authorName}", GetBooksFromAuthor)
                .Produces<IEnumerable<Book>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetBooksFromAuthor");

            booksApi.MapPost("/", CreateBook)
                .Accepts<BookDTO>("application/json")
                .Produces<Book>(StatusCodes.Status201Created)
                .Produces<List<FluentValidation.Results.ValidationFailure>>(StatusCodes.Status400BadRequest)
                .Produces<BookDTO>(StatusCodes.Status422UnprocessableEntity)
                .WithName("CreateBook");

            booksApi.MapPut("/{id:int}", UpdateBook)
                .Accepts<BookDTO>("application/json")
                .Produces<Book>(StatusCodes.Status200OK)
                .Produces<List<FluentValidation.Results.ValidationFailure>>(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("UpdateBook");

            booksApi.MapDelete("/{id:int}", DeleteBook)
                .Produces<Book>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("DeleteBook");

            app.Run();

            static async Task<IResult> GetAllBooks([FromServices] IBookRepository bookRepo)
            {
                APIResponse response = new APIResponse { Success = false, StatusCode = System.Net.HttpStatusCode.NotFound  };
                response.Result = await bookRepo.GetAll();
                if (response.Result is IEnumerable<Book>)
                {
                    response.Success = true;
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    return TypedResults.Ok(response);
                }

                response.ErrorMessages.Add("Book list not found.");
                return TypedResults.NotFound(response);
            }

            static async Task<IResult> GetBook([FromRoute] int id, [FromServices] IBookRepository bookRepo)
            {
                APIResponse response = new APIResponse { Success = false, StatusCode = System.Net.HttpStatusCode.NotFound };
                response.Result = await bookRepo.GetSingle(id);
                if (response.Result is Book)
                {
                    response.Success = true;
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    return TypedResults.Ok(response);
                }
                response.ErrorMessages.Add($"Book with id {id} not found.");
                return TypedResults.NotFound(response);
            }

            static async Task<IResult> GetBooksFromAuthor(string authorName, [FromServices] IBookRepository bookRepo)
            {
                APIResponse response = new APIResponse { Success = false, StatusCode = System.Net.HttpStatusCode.NotFound };
                var res = await bookRepo.GetFromAuthor(authorName);
                response.Result = res;
                if (res.Any())
                {
                    response.Success = true;
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    return TypedResults.Ok(response);
                }
                response.ErrorMessages.Add($"No books from author {authorName} found.");
                return TypedResults.NotFound(response);
            }

            static async Task<IResult> CreateBook([FromBody] BookEditDTO bookDTO, [FromServices] IMapper _mapper, [FromServices] IValidator<BookEditDTO> _validator, [FromServices] IBookRepository bookRepo)
            {
                APIResponse response = new APIResponse { Success = false, StatusCode = System.Net.HttpStatusCode.UnprocessableEntity };
                var validationResult = await _validator.ValidateAsync(bookDTO);
                if (!validationResult.IsValid)
                {
                    foreach (var v in validationResult.Errors)
                    {
                        response.ErrorMessages.Add(v.ToString());
                    }
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return TypedResults.BadRequest(response);
                }

                Book bookToAdd = _mapper.Map<Book>(bookDTO);
                response.Result = await bookRepo.Create(bookToAdd);
                if (response.Result is Book)
                {
                    response.Success = true;
                    response.StatusCode = System.Net.HttpStatusCode.Created;
                    return TypedResults.Created($"/api/book/{bookToAdd.Id}", response);
                }
                response.ErrorMessages.Add("Error in JSON data, please check your formating is valid.");
                return TypedResults.UnprocessableEntity(response);
            }

            static async Task<IResult> UpdateBook([FromRoute] int id, [FromBody] BookEditDTO bookDTO, [FromServices] IMapper _mapper, [FromServices] IValidator<BookEditDTO> _validator, [FromServices] IBookRepository bookRepo)
            {
                APIResponse response = new APIResponse { Success = false, StatusCode = System.Net.HttpStatusCode.NotFound };
                var validationResult = await _validator.ValidateAsync(bookDTO);
                if (!validationResult.IsValid)
                {
                    foreach (var v in validationResult.Errors)
                    {
                        response.ErrorMessages.Add(v.ToString());
                    }
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return TypedResults.BadRequest(response);
                }

                Book updatedBook = _mapper.Map<Book>(bookDTO);
                response.Result = await bookRepo.Update(id, updatedBook);
                if (response.Result is Book)
                {
                    response.Success = true;
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    return TypedResults.Ok(response);
                }
                response.ErrorMessages.Add($"Book with id {id} not found.");
                return TypedResults.NotFound(response);
            }

            static async Task<IResult> DeleteBook([FromRoute] int id, [FromServices] IBookRepository bookRepo)
            {
                APIResponse response = new APIResponse { Success = false, StatusCode = System.Net.HttpStatusCode.NotFound };
                response.Result = await bookRepo.Delete(id);
                if (response.Result is Book)
                {
                    response.Success = true;
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    return TypedResults.Ok(response);
                }
                response.ErrorMessages.Add($"Book with id {id} not found.");
                return TypedResults.NotFound(response);
            }
        }
    }
}