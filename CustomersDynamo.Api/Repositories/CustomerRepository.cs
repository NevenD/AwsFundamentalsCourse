using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using CustomersDynamo.Api.Contracts.Data;
using System.Net;
using System.Text.Json;

namespace CustomersDynamo.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly string _tableName = "customers";

    public CustomerRepository(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }

    public async Task<bool> CreateAsync(CustomerDto customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customersAsJson = JsonSerializer.Serialize(customer);
        var customersAsAttributes = Document.FromJson(customersAsJson).ToAttributeMap();

        // there is no create in Dnymao Db, put is used for update and create
        var createItemRequest = new PutItemRequest
        {
            TableName = _tableName,
            Item = customersAsAttributes,
            ConditionExpression = "attribute_not_exists(pk)" // pk ne smije postojati na serveru da ovaj requet prodje, ako postoji, request će failati i morat će se ponoviti
        };

        var response = await _dynamoDb.PutItemAsync(createItemRequest);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<CustomerDto?> GetAsync(Guid id)
    {
        var getItemRequest = new GetItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                {"pk", new AttributeValue {S = id.ToString()} },
                {"sk", new AttributeValue {S = id.ToString()} },
            }
        };

        var response = await _dynamoDb.GetItemAsync(getItemRequest);

        if (response.Item.Count == 0)
        {
            return null;
        }

        var itemAsDocument = Document.FromAttributeMap(response.Item);
        return JsonSerializer.Deserialize<CustomerDto?>(itemAsDocument.ToJson());
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var scanRequest = new ScanRequest
        {
            TableName = _tableName
        };

        var response = await _dynamoDb.ScanAsync(scanRequest);

        var customerDto = response.Items.Select(x =>
        {
            var json = Document.FromAttributeMap(x).ToJson();
            return JsonSerializer.Deserialize<CustomerDto>(json);
        });

        return customerDto;
    }

    public async Task<bool> UpdateAsync(CustomerDto customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customersAsJson = JsonSerializer.Serialize(customer);
        var customersAsAttributes = Document.FromJson(customersAsJson).ToAttributeMap();

        // there is no create in Dnymao Db, put is used for update and create
        var updateItemRequest = new PutItemRequest
        {
            TableName = _tableName,
            Item = customersAsAttributes
        };

        var response = await _dynamoDb.PutItemAsync(updateItemRequest);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleteItemRequest = new DeleteItemRequest
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                {"pk", new AttributeValue {S = id.ToString()} },
                {"sk", new AttributeValue {S = id.ToString()} },
            }
        };

        var response = await _dynamoDb.DeleteItemAsync(deleteItemRequest);

        return response.HttpStatusCode == HttpStatusCode.OK;
    }
}
