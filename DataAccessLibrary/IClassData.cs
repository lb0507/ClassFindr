using ClassFindrDataAccessLibrary.Models;

namespace ClassFindrDataAccessLibrary
{
    public interface IClassData
    {
        Task<List<BuildingModel>> GetClassBuildings(List<ClassModel> classes);
    }
}