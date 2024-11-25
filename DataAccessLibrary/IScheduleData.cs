using ClassFindrDataAccessLibrary.Models;

namespace ClassFindrDataAccessLibrary
{
    public interface IScheduleData
    {
        Task<List<ClassModel>> GetUserSchedule(UserModel? user);

        public Task<List<ClassModel>> GetClassesNotInSchedule(UserModel? user);

        Task<bool> SaveSchedule(UserModel? user, List<ClassModel> schedule);

        bool ScheduleMatches(int UID);

        void SetShowable(bool isShowable);

        bool GetRouteShowable();
    }
}