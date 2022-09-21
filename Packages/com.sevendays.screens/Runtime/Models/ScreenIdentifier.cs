using System;

using SevenDays.Screens.Attributes;

using UnityEngine;

namespace SevenDays.Screens.Models
{
    [Serializable]
    public class ScreenIdentifier : IEquatable<ScreenIdentifier>
    {
        internal string Value => _value;

        [Label("Guid: "), SerializeField]
        private string _value;

        public static bool operator ==(ScreenIdentifier obj1, ScreenIdentifier obj2)
        {
            return !ReferenceEquals(obj1, null) && obj1.Equals(obj2);
        }

        public static bool operator !=(ScreenIdentifier obj1, ScreenIdentifier obj2) => !(obj1 == obj2);

        public bool Equals(ScreenIdentifier other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return _value == other.Value;
        }

        public override bool Equals(object obj) => Equals(obj as ScreenIdentifier);

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}