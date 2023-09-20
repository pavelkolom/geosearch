using Api.Models;

namespace Api.Cache
{
    public static class GeoIPCache
    {
        public static Header DBHeader { get; set; }

        public static byte[] DB { get; set; }
    }
}
