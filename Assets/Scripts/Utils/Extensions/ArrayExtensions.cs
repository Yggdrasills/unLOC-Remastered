using UnityEngine;

namespace SevenDays.unLOC.Utils.Extensions
{
    public static class ArrayExtensions
    {
        public static void Shuffle<T>(this T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                T temp = array[i];
                int random = Random.Range(i, array.Length);
                array[i] = array[random];
                array[random] = temp;
            }
        }
    }
}