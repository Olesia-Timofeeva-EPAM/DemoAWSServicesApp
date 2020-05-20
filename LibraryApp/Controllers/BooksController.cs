using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Contracts;
using Infrastructure.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
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
		//[Route("api/Books/Get")]
		public async Task<IEnumerable<Book>> Get()
		{
			return await _dataAccessProvider.GetBooks();
		}
		[HttpPost]
		//[Route("api/Books/Create")]
		public async Task Create([FromBody] Book book)
		{
			if (ModelState.IsValid)
			{
				await _dataAccessProvider.AddBook(book);
				await _sqsProvider.Send($"Book with ISBN {book.ISBN} has been created.");
			}
		}
		[HttpGet]
		//[Route("api/Books/Details/{isbn}")]
		public async Task<Book> Details(string id)
		{
			return await _dataAccessProvider.GetBook(id);
		}
		[HttpPut]
		//[Route("api/Books/Edit")]
		public async Task Edit([FromBody] Book book)
		{
			if (ModelState.IsValid)
			{
				await _dataAccessProvider.UpdateBook(book);
				await _sqsProvider.Send($"Book with ISBN {book.ISBN} has been updated.");
			}
		}
		[HttpDelete]
		//[Route("api/Books/Delete/{isbn}")]
		public async Task DeleteConfirmed(string isbn)
		{
			await _dataAccessProvider.DeleteBook(isbn);
			await _sqsProvider.Send($"Book with ISBN {isbn} has been deleted.");
		}
	}
}
