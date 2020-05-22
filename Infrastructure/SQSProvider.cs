using Amazon.SQS;
using Amazon.SQS.Model;
using Infrastructure.Contracts;
using System.Threading.Tasks;

namespace Infrastructure
{
	public class SQSProvider : ISQSProvider
	{
		private const string QueueName = "DemoAppQueue";

		IAmazonSQS _sqsClient;
		public SQSProvider(IAmazonSQS sqsClient)
		{
			_sqsClient = sqsClient;
		}

		public string QueueUrl
		{
			get; set;
		}

		public async Task Send(string message)
		{
			if (!await DoesQueueExist())
				CreateQueue();

				var request = new SendMessageRequest
				{
					QueueUrl = QueueUrl,
					MessageBody = message
				};
			await _sqsClient.SendMessageAsync(request);
		}

		private async void CreateQueue()
		{
			var request = new CreateQueueRequest
			{
				QueueName = QueueName
			};
			var response = await _sqsClient.CreateQueueAsync(request);
			QueueUrl = response.QueueUrl;
		}

		private async Task<bool> DoesQueueExist()
		{
			var request = new ListQueuesRequest();
			var response = await _sqsClient.ListQueuesAsync(request);
			foreach (var queueUrl in response.QueueUrls)
			{
				if (queueUrl.Contains(QueueName))
				{
					QueueUrl = queueUrl;
					return true;
				}
			}

			return false;
		}
	}
}
