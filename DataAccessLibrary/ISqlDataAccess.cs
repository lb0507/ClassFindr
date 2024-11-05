
namespace ClassFindrDataAccessLibrary
{
    public interface ISqlDataAccess
    {
        string ConnectionStringName { get; set; }

        Task<T?> LoadSingle<T>(string sql);
        Task<List<T>> LoadData<T>(string sql);
        Task<List<T>> LoadData<T, U>(string sql, U parameters);
        Task SaveData<T>(string sql, T parameters);
    }
}