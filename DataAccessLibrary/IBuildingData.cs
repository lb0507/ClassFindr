using ClassFindrDataAccessLibrary.Models;

namespace ClassFindrDataAccessLibrary
{
    public interface IBuildingData
    {
        Task<List<BuildingModel>> FetchBuildings();
    }
}