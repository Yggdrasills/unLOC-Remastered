namespace SevenDays.unLOC.Storage
{
    public class StorageCreationParameters
    {
    }

    public class LocalStorageCreationParameters : StorageCreationParameters
    {
        public int Index { get; set; }

        public LocalStorageCreationParameters()
        {
        }

        public LocalStorageCreationParameters(int index)
        {
            Index = index;
        }
    }
}