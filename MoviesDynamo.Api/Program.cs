using MoviesDynamo.Api;

await new DataSeeder().ImportDataAsync();

//var newMovie = new Movie1
//{
//    Id = Guid.NewGuid(),
//    Title = "21 Jump street",
//    AgeRestriction = 18,
//    ReleaseYear = 2012,
//    RottenTomatoesPercentage = 85
//};

//var newMovie2 = new Movie2
//{
//    Id = Guid.NewGuid(),
//    Title = "21 Jump street",
//    AgeRestriction = 18,
//    ReleaseYear = 2012,
//    RottenTomatoesPercentage = 85
//};

//var asJson = JsonSerializer.Serialize(newMovie);
//var attributeMap1 = Document.FromJson(asJson).ToAttributeMap();


//var asJson2 = JsonSerializer.Serialize(newMovie2);
//var attributeMap2 = Document.FromJson(asJson2).ToAttributeMap();

//var transactionRequest = new TransactWriteItemsRequest
//{
//    TransactItems = new List<TransactWriteItem>
//    {
//        new()
//        {
//            Put = new Put
//            {
//                TableName = "movies-year-title",
//                Item = attributeMap1
//            }
//        },
//            new()
//        {
//            Put = new Put
//            {
//                TableName = "movies-title-rotten",
//                Item = attributeMap2
//            }
//        }
//    }
//};

//var dynamoDb = new AmazonDynamoDBClient();
//var response = await dynamoDb.TransactWriteItemsAsync(transactionRequest);
