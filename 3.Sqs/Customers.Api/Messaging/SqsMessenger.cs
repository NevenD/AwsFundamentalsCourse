using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Api.Messaging
{
    public class SqsMessenger : ISnsMessenger
    {
        private readonly IAmazonSimpleNotificationService _sns;
        private readonly IOptions<TopicSettings> _topicSettings;
        private string? Url;
        public SqsMessenger(IAmazonSimpleNotificationService sns, IOptions<TopicSettings> topicSettings)
        {
            _sns = sns;
            _topicSettings = topicSettings;
        }

        public async Task<PublishResponse> PublishMessageAsync<T>(T message)
        {
            var topicArn = await GetTopicArnAsync();

            var sendMessageRequest = new PublishRequest
            {
                TopicArn = topicArn,
                Message = JsonSerializer.Serialize(message),
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        "MessageType", new MessageAttributeValue
                        {
                            DataType = "String",
                            StringValue = typeof(T).Name
                        }
                    }
                }
            };

            return await _sns.PublishAsync(sendMessageRequest);
        }


        private async ValueTask<string> GetTopicArnAsync()
        {

            if (Url is not null)
            {
                return Url;
            }

            var queueUrlResponse = await _sns.FindTopicAsync(_topicSettings.Value.Name);
            Url = queueUrlResponse.TopicArn;
            return Url;
        }


    }
}
