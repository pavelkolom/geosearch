using Api.Util;
using System.Runtime.Serialization;

namespace Api.Models
{
    public class Location
    {
        /// <summary>
        /// название страны (случайная строка с префиксом "cou_")
        /// </summary>
        public byte[]? Country { get; set; }

        /// <summary>
        /// название области (случайная строка с префиксом "reg_")
        /// </summary>
        public byte[]? Region { get; set; }

        /// <summary>
        /// почтовый индекс (случайная строка с префиксом "pos_")
        /// </summary>
        public byte[]? Postal { get; set; }

        /// <summary>
        /// название города (случайная строка с префиксом "cit_")
        /// </summary>
        public byte[]? City { get; set; }

        /// <summary>
        /// название организации (случайная строка с префиксом "org_")
        /// </summary>
        public byte[]? Organization { get; set; }

        /// <summary>
        /// широта
        /// </summary>
        public byte[]? Latitude { get; set; }

        /// <summary>
        /// долгота
        /// </summary>
        public byte[]? Longitude { get; set; }

        public int Index { get; set; }

        public LocationInfo GetInfo()
        {
            return new LocationInfo
            {
                Country = ByteUtility.ConvertArrayToString(Country),
                Region = ByteUtility.ConvertArrayToString(Region),
                Postal = ByteUtility.ConvertArrayToString(Postal),
                City = ByteUtility.ConvertArrayToString(City),
                Organization = ByteUtility.ConvertArrayToString(Organization),
                Latitude = BitConverter.ToSingle(Latitude, 0),
                Longitude = BitConverter.ToSingle(Longitude, 0),
                Index = Index,
            };
        }
    }

    public record class LocationInfo
    {
        /// <summary>
        /// название страны (случайная строка с префиксом "cou_")
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// название области (случайная строка с префиксом "reg_")
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// почтовый индекс (случайная строка с префиксом "pos_")
        /// </summary>
        public string? Postal { get; set; }

        /// <summary>
        /// название города (случайная строка с префиксом "cit_")
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// название организации (случайная строка с префиксом "org_")
        /// </summary>
        public string? Organization { get; set; }

        /// <summary>
        /// широта
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// долгота
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// IPRange
        /// </summary>
        public IPRangeInfo? Range { get; set; }
        public int Index { get; set; }

        public override string ToString()
        {
            return
                $"Country = {Country}, " +
                $"Region = {Region}, " +
                $"Postal = {Postal}, " +
                $"City = {City}, " +
                $"Organization = {Organization}, " +
                $"Latitude = {Latitude}, " +
                $"Longitude = {Longitude}" +
                $"Index = {Index}";
        }
    }
}