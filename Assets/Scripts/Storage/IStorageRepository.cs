namespace SevenDays.unLOC.Storage
{
    public interface IStorageRepository
    {
        void Save<T>(string key, T data);

        void Remove(string key);

        T Load<T>(string key);

        bool TryLoad<T>(string key, out T data);

        bool IsExists(string key);
    }
}