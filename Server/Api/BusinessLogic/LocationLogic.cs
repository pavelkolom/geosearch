using Api.Cache;
using Api.Models;

namespace Api.BusinessLogic
{
    public static class LocationLogic
    {
        public static Location GetLocationFromDBByIndex(int index)
        {
            var locationRec = GeoIPCache.DB
                .Skip(GeoIPCache.DBHeader.Offset_locations + index)
                .Take(96)
                .ToArray();

            var loc = new Location
            {
                Country = locationRec.AsSpan(0, 8).ToArray(),
                Region = locationRec.AsSpan(8, 12).ToArray(),
                Postal = locationRec.AsSpan(20, 12).ToArray(),
                City = locationRec.AsSpan(32, 24).ToArray(),
                Organization = locationRec.AsSpan(56, 32).ToArray(),
                Latitude = locationRec.AsSpan(88, 4).ToArray(),
                Longitude = locationRec.AsSpan(92, 4).ToArray()
            };

            return loc;
        }

        public static Location GetLocationFromByteArray(byte[] locationRec)
        {
            var loc = new Location
            {
                Country = locationRec.AsSpan(0, 8).ToArray(),
                Region = locationRec.AsSpan(8, 12).ToArray(),
                Postal = locationRec.AsSpan(20, 12).ToArray(),
                City = locationRec.AsSpan(32, 24).ToArray(),
                Organization = locationRec.AsSpan(56, 32).ToArray(),
                Latitude = locationRec.AsSpan(88, 4).ToArray(),
                Longitude = locationRec.AsSpan(92, 4).ToArray()
            };

            return loc;
        }

        public static List<LocationInfo> GetSortedByCity()
        {
            var locations = new List<LocationInfo>();
            int i = 0;
            foreach (var index in GeoIPCache.DB
                .Skip(GeoIPCache.DBHeader.Offset_cities)
                .Take(4 * GeoIPCache.DBHeader.Records)
                .Chunk(4))
            {
                var location = GetLocationFromDBByIndex(BitConverter.ToInt32(index));
                location.Index = i;
                locations.Add(location.GetInfo());
                i++;
            }
            return locations;
        }
    }
}
