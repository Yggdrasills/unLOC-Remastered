using System;

namespace SevenDays.unLOC.Profiles.Models
{
    public class Profile
    {
        public ProfileInfo Info { get; set; }

        public int SceneIndex { get; set; } = 1;
    }

    public class ProfileInfo
    {
        public DateTime DateCreation { get; set; }

        public DateTime DateActivity { get; set; }

        public int Index { get; set; }
    }
}