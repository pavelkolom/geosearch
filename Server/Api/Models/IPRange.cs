using Api.Util;

namespace Api.Models
{
    public class IPRange
    {
        /// <summary>
        /// начало диапазона IP адресов
        /// </summary>
        public uint Ip_from { get; set; }

        /// <summary>
        /// конец диапазона IP адресов
        /// </summary>
        public uint Ip_to { get; set; }

        /// <summary>
        /// индекс записи о местоположении
        /// </summary>
        public int Location_index { get; set; }

        public IPRangeInfo GetInfo()
        {
            return new IPRangeInfo
            {
                Ip_from = IPUtility.ConvertUintToIP(Ip_from),
                Ip_to = IPUtility.ConvertUintToIP(Ip_to),
                Location_index = Location_index
            };
        }
    }

    public class IPRangeInfo
    {
        /// <summary>
        /// начало диапазона IP адресов
        /// </summary>
        public string Ip_from { get; set; }

        /// <summary>
        /// конец диапазона IP адресов
        /// </summary>
        public string Ip_to { get; set; }

        /// <summary>
        /// индекс записи о местоположении
        /// </summary>
        public int Location_index { get; set; }
    }
}
