using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Contracts
{
	[DynamoDBTable("Book")]
	public class Book
	{
		[DynamoDBHashKey]
		public string ISBN { get; set; }

		public string Title { get; set; }

		public string Description { set; get; }
	}
}
