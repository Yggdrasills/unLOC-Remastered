using Cysharp.Threading.Tasks;

namespace SevenDays.unLOC.Core.Loaders
{
    public interface IInstallable
    {
        void Install();
    }

    public interface IAsyncInstallable
    {
        UniTask InstallAsync();
    }
}