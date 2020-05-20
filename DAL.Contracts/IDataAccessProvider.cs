using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Contracts
{
	public interface IDataAccessProvider
	{
		Task AddBook(Book book);  
	       Task UpdateBook(Book book);  
	       Task DeleteBook(string booktId);  
	       Task<Book> GetBook(string bookId);  
	       Task<IEnumerable<Book>> GetBooks();

	}
}
