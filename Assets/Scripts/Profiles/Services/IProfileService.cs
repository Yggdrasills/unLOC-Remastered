using SevenDays.unLOC.Profiles.Models;

namespace SevenDays.unLOC.Profiles.Services
{
    public interface IProfileService
    {
        Profile[] Profiles { get; }

        // todo: скорее всего должно возвращать значение
        void CreateProfile();

        void SetActiveProfile(int profileIndex);
    }
}