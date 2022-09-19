namespace SevenDays.unLOC.Storage
{
    public interface IStorageDecorator : IStorageRepository
    {
        void SetStorage<T>(T storage) where T : IStorageRepository;

        void SetProfileIndex(int index);
    }
}