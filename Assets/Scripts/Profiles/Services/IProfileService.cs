using SevenDays.unLOC.Profiles.Models;

namespace SevenDays.unLOC.Profiles.Services
{
    public interface IProfileService
    {
        ProfileInfo[] ProfileInfos { get; }

        // todo: скорее всего должно возвращать значение
        void CreateProfile();

        void SetActiveProfile(int profileIndex);

        /// <summary>
        /// returns -1 if no there is no active profile
        /// </summary>
        int GetSceneIndex(int profileIndex);
    }
}