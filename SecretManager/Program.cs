using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

var secretManagerClient = new AmazonSecretsManagerClient();


var listSecretVersionsRequest = new ListSecretVersionIdsRequest
{
    SecretId = "ApiKey",
    IncludeDeprecated = true
};

// secret version
var versionResponse = await secretManagerClient.ListSecretVersionIdsAsync(listSecretVersionsRequest);

var request = new GetSecretValueRequest
{
    SecretId = "ApiKey",
    VersionStage = "AWSPREVIOUS" // get previous secret, one secret before the update
};

var response = await secretManagerClient.GetSecretValueAsync(request);

Console.WriteLine(response.SecretString);

var describeRequest = new DescribeSecretRequest
{
    SecretId = "ApiKey"
};


var describeResponse = await secretManagerClient.DescribeSecretAsync(describeRequest);

Console.WriteLine(describeResponse);

