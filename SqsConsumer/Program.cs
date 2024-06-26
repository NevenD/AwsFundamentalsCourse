﻿

using Amazon.SQS;
using Amazon.SQS.Model;

var queueName = args.Length == 1 ? args[0] : "customers";

var cts = new CancellationTokenSource();

var sqsClient = new AmazonSQSClient();

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

var receieveMessageRequest = new ReceiveMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    AttributeNames = new List<string> { "All" },
    MessageAttributeNames = new List<string> { "All" },
};

while (!cts.IsCancellationRequested)
{

    var response = await sqsClient.ReceiveMessageAsync(receieveMessageRequest, cts.Token);

    foreach (var message in response.Messages)
    {
        Console.WriteLine($"Message Id: {message.MessageId}");
        Console.WriteLine($"Message Body: {message.Body}");

        //we need to explcitly delete the messages
        await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
    }
    await Task.Delay(3000);
};

