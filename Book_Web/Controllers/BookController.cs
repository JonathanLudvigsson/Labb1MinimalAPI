using Book_MinimalAPI.Models.DTOs;
using Book_Web.Models;
using Book_Web.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Book_Web.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookServiceInject)
        {
            _bookService = bookServiceInject;
        }

        [ActionName("BookIndex")]
        public async Task<IActionResult> BookIndex()
        {
            List<BookDTO> list = new List<BookDTO>();
            APIResponseDTO response = await _bookService.GetAllBooks<APIResponseDTO>();
            if (response != null && response.Success)
            {
                list = JsonConvert.DeserializeObject<List<BookDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> BookDetails(int id)
        {
            BookDTO cDTO = new BookDTO();
            var response = await _bookService.GetBook<APIResponseDTO>(id);
            if (response != null && response.Success)
            {
                BookDTO model = JsonConvert.DeserializeObject<BookDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return View();
        }

        public async Task<IActionResult> BookFromAuthor(string authorName)
        {
            List<BookDTO> list = new List<BookDTO>();
            var response = await _bookService.GetBooksFromAuthor<APIResponseDTO>(authorName);
            if (response != null && response.Success)
            {
                list = JsonConvert.DeserializeObject<List<BookDTO>>(Convert.ToString(response.Result));
                return View(list);
            }
            return View();
        }

        public async Task<IActionResult> BookCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookCreate(BookDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _bookService.CreateBook<APIResponseDTO>(model);
                if (response != null && response.Success)
                {
                    return RedirectToAction(nameof(BookIndex));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> BookUpdate(int id)
        {
            var response = await _bookService.GetBook<APIResponseDTO>(id);
            if (response != null && response.Success)
            {
                BookDTO model = JsonConvert.DeserializeObject<BookDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookUpdate(BookDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _bookService.UpdateBook<APIResponseDTO>(model);
                if (response != null && response.Success)
                {
                    return RedirectToAction(nameof(BookIndex));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> BookDelete(int id)
        {
            var response = await _bookService.GetBook<APIResponseDTO>(id);
            if (response != null && response.Success)
            {
                BookDTO model = JsonConvert.DeserializeObject<BookDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookDelete(BookDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _bookService.DeleteBook<APIResponseDTO>(model.Id);
                if (response != null && response.Success)
                {
                    return RedirectToAction(nameof(BookIndex));
                }
            }

            return NotFound();
        }
    }
}
