# Get Started

## Instructions
1. Use Docker already configured to run the API.
2. With the MongoDB running, connect to it and create a database called `developer_store`, and then create a collection with name `products`. After that, load the products using the `products-dump.json` file located in the `.doc` folder.
3. Use the following command to update the database with EF Core:
  `dotnet ef database update --context DefaultContext --connection "Host=localhost;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n"`
