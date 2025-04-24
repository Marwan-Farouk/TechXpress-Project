public static class Connections
{
    public static readonly string ConnectionString = GetConnectionString();

    private static string GetConnectionString()
    {
        return "Server=localhost;Database=MYDatabase;User Id=root;Password=yourpassword;";
    }
}
