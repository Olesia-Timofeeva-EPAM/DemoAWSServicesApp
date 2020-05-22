using DAL.Contracts;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BooksController : ControllerBase
	{
		private readonly IDataAccessProvider _dataAccessProvider;
		private ISQSProvider _sqsProvider;
		public BooksController(IDataAccessProvider dataAccessProvider, ISQSProvider sqsProvider)
		{
			_dataAccessProvider = dataAccessProvider;
			_sqsProvider = sqsProvider;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IEnumerable<Book>> Get()
		{
			return await _dataAccessProvider.GetBooks();
		}
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task Create([FromBody] Book book)
		{
				await _dataAccessProvider.AddBook(book);
				await _sqsProvider.Send($"Book with ISBN {book.ISBN} has been created.");
		}

		[HttpGet]
		[Route("Details/{isbn}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<Book> Details(string isbn)
		{
			return await _dataAccessProvider.GetBook(isbn);
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task Edit([FromBody] Book book)
		{
			await _dataAccessProvider.UpdateBook(book);
			await _sqsProvider.Send($"Book with ISBN {book.ISBN} has been updated.");
		}

		[HttpDelete]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task DeleteConfirmed(string isbn)
		{
			await _dataAccessProvider.DeleteBook(isbn);
			await _sqsProvider.Send($"Book with ISBN {isbn} has been deleted.");
		}
	}
}
