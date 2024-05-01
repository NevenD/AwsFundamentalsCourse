using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SnsPublisher;
using System.Text.Json;

var customer = new CustomerCreated()
{
    Id = Guid.NewGuid(),
    Email = "neven.test@gmail.com",
    FullName = "Neven",
    DateOfBirth = new DateTime(1986, 1, 1),
    GitHubUserName = "NevenD"
};

var snsClient = new AmazonSimpleNotificationServiceClient();

var topicArnResponse = await snsClient.FindTopicAsync("customers");

var publishRequest = new PublishRequest
{
    TopicArn = topicArnResponse.TopicArn,
    Message = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(customer)
            }
        }
    }
};

var response = await snsClient.PublishAsync(publishRequest);
