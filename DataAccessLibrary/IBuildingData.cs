using ClassFindrDataAccessLibrary.Models;

namespace ClassFindrDataAccessLibrary
{
    public interface IBuildingData
    {

        List<BuildingModel> GetBuildingList();
    }
}