namespace BlazorAPITest.Components
{
    public class MapBoxConfig : IMapBoxConfig
    {
        // Gets the connection string under "MapBox" in appsettings.json
        public string? Key { get; set; }

        public MapBoxConfig(IConfiguration config)
        {
            Key = config.GetConnectionString("MapBox") ?? "";
        }
    }
}
