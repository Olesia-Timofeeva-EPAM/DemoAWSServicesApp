using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime.Internal.Util;
using DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
	public class DataAccessDynamoDBProvider : IDataAccessProvider
	{
		private readonly ILogger _logger;
		IDynamoDBContext _dynamoDbContext;
		public DataAccessDynamoDBProvider(IDynamoDBContext dynamoDbContext)
		{
			_dynamoDbContext = dynamoDbContext;
		}  

		public async Task AddBook(Book book)
		{
			await _dynamoDbContext.SaveAsync<Book>(book);
		} 
		
		public async Task UpdateBook(Book book)
		{
			List < ScanCondition > conditions = new List<ScanCondition>();
			conditions.Add(new ScanCondition("ISBN", ScanOperator.Equal, book.ISBN));
			var allDocs = await _dynamoDbContext.ScanAsync<Book>(conditions).GetRemainingAsync();
			var editedState = allDocs.FirstOrDefault();
			if (editedState != null)
			{
				editedState = book;
				
				await _dynamoDbContext.SaveAsync<Book>(editedState);
			}
		} 
		
       public async Task DeleteBook(string isbn)
		{
			await _dynamoDbContext.DeleteAsync(isbn);
		} 
		
        public async Task<Book> GetBook(string isbn)
		{
			List < ScanCondition > conditions = new List<ScanCondition>();
			conditions.Add(new ScanCondition("ISBN", ScanOperator.Equal, isbn));
			var allDocs = await _dynamoDbContext.ScanAsync<Book>(conditions).GetRemainingAsync();
			var book = allDocs.FirstOrDefault();
			return book;
		}  

       public async Task<IEnumerable<Book>> GetBooks()
		{
			List < ScanCondition > conditions = new List<ScanCondition>();

			var allDocs = await _dynamoDbContext.ScanAsync<Book>(conditions).GetRemainingAsync();
			
			return allDocs;
		}

		
	}

}

