using ClassFindrDataAccessLibrary.Models;

namespace ClassFindrDataAccessLibrary
{
    public interface IScheduleData
    {
        Task<List<ClassModel>> GetUserSchedule();
    }
}