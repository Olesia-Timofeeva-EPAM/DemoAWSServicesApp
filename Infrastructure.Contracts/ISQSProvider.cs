using System;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
	public interface ISQSProvider
	{
		Task Send(string message);
	}
}
