using System;

namespace SevenDays.unLOC.Profiles.Models
{
    public class Profile
    {
        public DateTime DateCreation { get; set; }

        public DateTime DateActivity { get; set; }

        public int SceneIndex { get; set; } = 1;
        public int Index { get; set; }
    }
}