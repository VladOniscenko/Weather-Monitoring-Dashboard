namespace Seeder;
public class Program
{
    public static async Task Main()
    {
        Console.WriteLine("[SEEDER] Starting Country and City Seeder");
        try
        {
            await CountriesAndCitiesSeerder.Seed();
            Console.WriteLine("[SEEDER] Country and City Seeder Done");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SEEDER] Error: {ex.Message}");
        }
    }
}
