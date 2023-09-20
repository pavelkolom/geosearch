using System.Text;

namespace Api.Models
{
    public record class Header
    {
        /// <summary>
        /// версия база данных
        /// </summary>
        public byte[] Version { get; init; } = new byte[4];

        /// <sum
        /// название/префикс для базы данных
        /// </summary>
        public byte[] Name { get; init; } = new byte[32];

        /// <summary>
        /// время создания базы данных
        /// </summary>
        public byte[] Timestamp { get; init; } = new byte[8];

        /// <summary>
        /// общее количество записей
        /// </summary>
        public int Records { get; init; }

        /// <summary>
        /// смещение относительно начала файла до начала списка записей с геоинформацией
        /// </summary>
        public int Offset_ranges { get; init; }

        /// <summary>
        /// смещение относительно начала файла до начала индекса с сортировкой по названию городов
        /// </summary>
        public int Offset_cities { get; init; }

        /// <summary>
        /// смещение относительно начала файла до начала списка записей о местоположении
        /// </summary>
        public int Offset_locations { get; init; }

        public long ReadInmilliseconds { get; set; }

        /// <summary>
        /// время создания базы данных
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public DateTime GetDate()
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var dtime = dtDateTime.AddSeconds(BitConverter.ToUInt64(Timestamp, 0)).ToLocalTime();
            return dtime;
        }

        /// <summary>
        /// название/префикс для базы данных
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public string GetName()
        {
            return Encoding.UTF8.GetString(Name, 0, Name.Length).Trim('\0');
        }

        public override string ToString()
        {
            return 
                $"Version = {BitConverter.ToInt32(Version, 0)}, " +
                $"Name = {GetName()}, " +
                $"Created = {GetDate().ToLongDateString()}, " +
                $"TotalRecords = {Records}, " +
                $"ReadInmilliseconds = {ReadInmilliseconds}";
        }
    }
}
