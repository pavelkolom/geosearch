using Api.Cache;
using Api.Models;
using Api.Util;

namespace Api.BusinessLogic
{
    public interface IParser
    {
        void ParseDB(IEnumerable<byte> fileBytes, int headerSize, int rangeRecSize, int locationRecSize);
    }

    public class ParserLogic : IParser
    {
        public void ParseDB(IEnumerable<byte> fileBytes, int headerSize, int rangeRecSize, int locationRecSize)
        {
            var totalwatch = new System.Diagnostics.Stopwatch();
            totalwatch.Start();
            GeoIPCache.DBHeader = ParseHeader(fileBytes.Take(60));
            GeoIPCache.DB = fileBytes.ToArray();
            totalwatch.Stop();
            Console.WriteLine($"Total time: {totalwatch.ElapsedMilliseconds} ms");
            GeoIPCache.DBHeader.ReadInmilliseconds = totalwatch.ElapsedMilliseconds;
        }

        private Header ParseHeader(IEnumerable<byte> fileBytes)
        {
            return new Header()
            {
                // общее количество записей
                Records = BitConverter.ToInt32(fileBytes.Skip(44).Take(4).ToArray(), 0),
                // смещение относительно начала файла до начала списка записей с геоинформацией
                Offset_ranges = BitConverter.ToInt32(fileBytes.Skip(48).Take(4).ToArray(), 0),
                // смещение относительно начала файла до начала индекса с сортировкой по названию городов
                Offset_cities = BitConverter.ToInt32(fileBytes.Skip(52).Take(4).ToArray(), 0),
                // смещение относительно начала файла до начала списка записей о местоположении
                Offset_locations = BitConverter.ToInt32(fileBytes.Skip(56).Take(4).ToArray(), 0)
            };
        }
    }
}
