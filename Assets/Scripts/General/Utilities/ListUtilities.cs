using System.Collections.Generic;

namespace Spellweavers
{
    public static class ListUtilities
    {
        private static readonly System.Random Randomizer = new();
        public static void Shuffle<T>(this List<T> list)
        {
            int element1 = list.Count;
            while (element1 > 1)
            {
                element1--;
                int element2 = Randomizer.Next(element1 + 1);
                (list[element1], list[element2]) = (list[element2], list[element1]);
            }
        }

        public static List<T> SliceFromStart<T>(this List<T> list, int length) {
            List<T> result = new();
            int count = list.Count;
            if (count == 0)
            {
                return result;
            }

            length = length < 1 ? count : length;
            int cut = length > count ? count : length;
            for (int i = 0; i < cut; i++)
            {
                result.Add(list[i]);
            }

            List<T> cuttedList = new(count - cut);
            for (int i = cut; i < count; i++)
            {
                cuttedList.Add(list[i]);
            }

            list.Clear();
            foreach (T item in cuttedList)
            {
                list.Add(item);
            }

            return result;
        }
    }
}