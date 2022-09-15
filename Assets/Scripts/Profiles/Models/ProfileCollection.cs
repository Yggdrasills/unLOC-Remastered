using System.Collections.Generic;

namespace SevenDays.unLOC.Profiles.Models
{
    public class ProfileCollection
    {
        public List<Profile> Profiles { get; set; }

        public Profile ActiveProfile { get; set; }

        public ProfileCollection()
        {
            Profiles = new List<Profile>();
        }
    }
}