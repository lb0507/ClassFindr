using ClassFindrDataAccessLibrary.Models;

namespace ClassFindrDataAccessLibrary
{
    public interface IScheduleData
    {
        Task<List<ClassModel>> GetUserSchedule();

        bool ScheduleMatches(int UID);

        void SetShowable(bool isShowable);

        bool GetRouteShowable();
    }
}