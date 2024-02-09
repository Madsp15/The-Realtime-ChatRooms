using Npgsql;

namespace The_Realtime_Chat_Mini_Project.Repositories;

public static class DataConnection
{
    public static readonly Uri Uri;
    public static readonly string ProperlyFormattedConnectionString;
    public static readonly NpgsqlDataSource DataSource;

    static DataConnection()
    {
        string rawConnectionString;
        string envVarKeyName = "pgconn";

        rawConnectionString = Environment.GetEnvironmentVariable(envVarKeyName);
        Console.WriteLine($"Raw Connection String: {rawConnectionString}");
        if (rawConnectionString == null)
        {
            throw new Exception("Looks like you need to add an environment variable to connect to the database!");
        }

        try
        {
            Uri = new Uri(rawConnectionString);
            ProperlyFormattedConnectionString = string.Format(
                "Server={0};Database={1};User Id={2};Password={3};Port={4};Pooling=true;MaxPoolSize=3",
                Uri.Host,
                Uri.AbsolutePath.Trim('/'),
                Uri.UserInfo.Split(':')[0],
                Uri.UserInfo.Split(':')[1],
                Uri.Port > 0 ? Uri.Port : 5432);
            DataSource =
                new NpgsqlDataSourceBuilder(ProperlyFormattedConnectionString).EnableParameterLogging().Build();
            DataSource.OpenConnection().Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.InnerException);
            Console.WriteLine(e.StackTrace);
            throw new Exception("Connection string found but could not be formatted correctly!");
        }
        
    }
    
}