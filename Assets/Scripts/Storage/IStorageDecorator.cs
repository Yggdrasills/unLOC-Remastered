namespace SevenDays.unLOC.Storage
{
    public interface IStorageDecorator : IStorageRepository
    {
        void SetStorage<T, V>(V creationParams)
            where T : IStorageRepository
            where V : StorageCreationParameters, new();

        void SetStorage<T>()
            where T : IStorageRepository, new();
    }
}