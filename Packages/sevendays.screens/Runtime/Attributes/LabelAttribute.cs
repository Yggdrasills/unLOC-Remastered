using UnityEngine;

namespace SevenDays.Screens.Attributes
{
    internal class LabelAttribute : PropertyAttribute
    {
        internal string Prefix { get; }

        internal LabelAttribute(string prefix)
        {
            Prefix = prefix;
        }
    }
}