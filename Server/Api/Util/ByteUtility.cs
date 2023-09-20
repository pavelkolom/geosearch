using Api.Models;
using System.Text;

namespace Api.Util
{
    public static class ByteUtility
    {
        public static string ConvertArrayToString(byte[] array)
        {
            return Encoding.UTF8.GetString(array, 0, array.Length).Trim('\0');
        }

        public static int ConvertToInt32(byte[] b)
        {
            return b[0] | (b[1] << 8) | (b[2] << 16) | (b[3] << 24);
        }

        public static byte[] ConvertStringToArray(string s)
        {
            byte[] arr = new byte[24];
            var converted = Encoding.UTF8.GetBytes(s);
            for (int i = 0; i < converted.Length; i++) { arr[i] = converted[i]; }
            return arr;
        }

        public static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }

        /// <summary>
        /// сравнение байтовых массивов
        /// -1 - первый больше
        /// 1 - первый меньше
        /// 0 - равны
        /// null - один или оба массива null или их длины не равны
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static int? CompareByteArrays(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return 0;
            if (b1 == null || b2 == null) return null;
            if (b1.Length != b2.Length) return null;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] > b2[i]) return -1;
                if (b1[i] < b2[i]) return 1;
            }
            return 0;
        }

        /// <summary>
        /// убираем пробелы в конце массива
        /// </summary>
        /// <param name="array"></param>
        public static void Trim(byte[] array)
        {
            for (int c = array.Length - 1; c >= 0; c--)
            {
                if (array[c] != 0 && array[c] == 32) array[c] = 0;
                if (array[c] != 0 && array[c] != 32) break;
            }
        }

    }
}
