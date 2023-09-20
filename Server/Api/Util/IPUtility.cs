using System.Net;

namespace Api.Util
{
    public static class IPUtility
    {
        public static string ConvertUintToIP(uint ip)
        {
            var part1 = ip % 256;
            var part2 = ip / 256 % 256;
            var part3 = ip / 256 / 256 % 256;
            var part4 = ip / 256 / 256 / 256;
            return $"{part4}.{part3}.{part2}.{part1}";
        }

        public static uint ConvertIPToUint(string ipstr)
        {
            var ipAddress = IPAddress.Parse(ipstr);
            var ipBytes = ipAddress.GetAddressBytes();
            var ip = (uint)ipBytes[0] << 24;
            ip += (uint)ipBytes[1] << 16;
            ip += (uint)ipBytes[2] << 8;
            ip += ipBytes[3];
            return ip;
        }
    }
}
