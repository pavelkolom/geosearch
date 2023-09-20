#define VARIANT2

using Api.Cache;
using Api.Models;
using Api.Util;

namespace Api.BusinessLogic
{
    /// <summary>
    /// ISearcher interface
    /// </summary>
    public interface ISearcher
    {
        /// <summary>
        /// SearchLocationByIp
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        LocationInfo? SearchLocationByIp(string ip);

        /// <summary>
        /// SearchLocationsByCityName
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        IEnumerable<LocationInfo>? SearchLocationsByCityName(string city);
    }

    /// <summary>
    /// Бинарный поиск (binary search) 
    /// </summary>
    public class BinarySearchLogic : ISearcher
    {
        private readonly int _locatonSize;
        private readonly int _iPRangeSize;
        private readonly int _intSize = 4;

        /// <summary>
        /// BinarySearchLogic
        /// </summary>
        /// <param name="configuration"></param>
        public BinarySearchLogic(IConfiguration configuration)
        {
            int.TryParse(configuration["LocationSize"], out _locatonSize);
            int.TryParse(configuration["IPRangeSize"], out _iPRangeSize);
        }

        #region IP search
        /// <summary>
        /// SearchLocationByIp
        /// </summary>
        /// <param name="ipString">IP address</param>
        /// <returns></returns>
        public LocationInfo? SearchLocationByIp(string ipString)
        {
            // переводим строчный айпи адрес в  
            uint ip = IPUtility.ConvertIPToUint(ipString);
            
            // выполняем бинарный поиск по таблице интервалов адресов
            IPRange? range = BinaryRangeSearch(
                GeoIPCache.DB,
                ip, 0,
                GeoIPCache.DBHeader.Records - 1);

            // если не находим, то возвращаем нулл
            if (range == null) return null;

            // индекс записи о местоположении
            var indx = range.Location_index;

#if VARIANT1
            // 1-ый вариант - ищем запись о местоположении исходя из того что индекс равен номеру строки в таблице местоположений
            var loc_record = GeoIPCache.DB
                .Skip(GeoIPCache.DBHeader.Offset_locations + _locatonSize * indx)
                .Take(_locatonSize)
                .ToArray();
#endif

#if VARIANT2
            // 2-ой вариант - ищем запись о местоположении исходя из того что индекс indx равен номеру строки в таблице индексов,
            // а по адресу, хранящемуся в строке мы извлекаем местоположение
            // получаем содержимое из таблицы индексов
            var index_record = GeoIPCache.DB
                .Skip(GeoIPCache.DBHeader.Offset_cities + _intSize * indx)
                .Take(_intSize)
                .ToArray();

            // получаем адрес местоположения
            var addressOfLocation = BitConverter.ToInt32(index_record, 0);

            // получаем местоположение по адресу
            var loc_record = GeoIPCache.DB
                .Skip(GeoIPCache.DBHeader.Offset_locations + addressOfLocation)
                .Take(_locatonSize)
                .ToArray();
#endif

            var location = new Location
            {
                Country = loc_record.AsSpan(0, 8).ToArray(),
                Region = loc_record.AsSpan(8, 12).ToArray(),
                Postal = loc_record.AsSpan(20, 12).ToArray(),
                City = loc_record.AsSpan(32, 24).ToArray(),
                Organization = loc_record.AsSpan(56, 32).ToArray(),
                Latitude = loc_record.AsSpan(88, 4).ToArray(),
                Longitude = loc_record.AsSpan(92, 4).ToArray()
            };

            LocationInfo info = location.GetInfo();
            info.Range = range.GetInfo();
            return info;
        }

        /// <summary>
        /// метод для рекурсивного бинарного поиска интервала IP адресов
        /// </summary>
        /// <param name="array"></param>
        /// <param name="searchedValue"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        private IPRange? BinaryRangeSearch(byte[] array, uint searchedValue, int first, int last)
        {
            //границы сошлись
            if (first > last)
            {
                //элемент не найден
                return null;
            }

            //средний индекс подмассива
            var middle = (first + last) / 2;

            // получим значение интервала по номеру
            var range = array.Skip(GeoIPCache.DBHeader.Offset_ranges + _iPRangeSize * middle).Take(_iPRangeSize);

            //значение в средине подмассива
            var middleValueFrom = BitConverter.ToUInt32(range.Take(_intSize).ToArray(), 0);
            var middleValueTo = BitConverter.ToUInt32(range.Skip(_intSize).Take(_intSize).ToArray(), 0);

            // проверяем попадает ли в интервал
            if (middleValueFrom <= searchedValue && middleValueTo >= searchedValue)
                // если да, то возвращаем интервал
                return new IPRange()
                {
                    Ip_from = middleValueFrom,
                    Ip_to = middleValueTo,
                    Location_index = middle
                };
            //если нет, то ищем дальше
            else
            {
                if (middleValueFrom > searchedValue)
                {
                    //рекурсивный вызов поиска для левого подмассива
                    return BinaryRangeSearch(array, searchedValue, first, middle - 1);
                }
                else
                {
                    //рекурсивный вызов поиска для правого подмассива
                    return BinaryRangeSearch(array, searchedValue, middle + 1, last);
                }
            }
        }
#endregion

        #region City name search
        /// <summary>
        /// метод для рекурсивного бинарного поиска местоположений по названию города
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public IEnumerable<LocationInfo>? SearchLocationsByCityName(string city)
        {
            // преврашаем в массив
            byte[] searchedValue = ByteUtility.ConvertStringToArray(city);

            // ищем соответствие
            var res = BinaryLocationsSearch(searchedValue, 0,
                GeoIPCache.DBHeader.Records - 1);
            if (res != null)
            {
                var list = new List<LocationInfo>();

                // добавляем в список остальные местоположения рядом с найденным
                List<LocationInfo> otherLocations = FindOtherLocations(res);
                if(otherLocations != null && otherLocations.Any())
                    list.AddRange(otherLocations);

                return list;
            }
            return null;
        }

        /// <summary>
        /// метод для рекурсивного бинарного поиска местоположений
        /// </summary>
        /// <param name="searchedValue">искомое значение - город</param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        private Location? BinaryLocationsSearch(byte[] searchedValue, int first, int last)
        {
            //границы сошлись
            if (first > last)
            {
                //элемент не найден
                return null;
            }

            //средний индекс подмассива
            var middle = (first + last) / 2;

            // получим значение города по среднему индексу 
            var indexInTheMiddle = GeoIPCache.DB.Skip(GeoIPCache.DBHeader.Offset_cities + 4 * middle).Take(4).ToArray();
            var locationInTheMiddle = GeoIPCache.DB.Skip(GeoIPCache.DBHeader.Offset_locations + BitConverter.ToInt32(indexInTheMiddle, 0)).Take(96).ToArray();
            var cityInTheMiddle = locationInTheMiddle.Skip(32).Take(24).ToArray();

            // убираем пробелы в конце массива
            ByteUtility.Trim(cityInTheMiddle);

            //значение в средине подмассива
            var comparison = ByteUtility.CompareByteArrays(cityInTheMiddle, searchedValue);
            if (comparison == 0)
            {
                var loc = LocationLogic.GetLocationFromByteArray(locationInTheMiddle);
                loc.Index = middle;
                return loc;
            }

            else
            {
                if (comparison == -1)//cityInTheMiddle > searchedValue
                {
                    //рекурсивный вызов поиска для левого подмассива
                    return BinaryLocationsSearch(searchedValue, first, middle - 1);
                }
                else
                {
                    //рекурсивный вызов поиска для правого подмассива
                    return BinaryLocationsSearch(searchedValue, middle + 1, last);
                }
            }
        }

        /// <summary>
        /// Поиск местоположений влево и вправо от найденного значения
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        private List<LocationInfo> FindOtherLocations(Location loc)
        {
            var list = new List<LocationInfo>() { loc.GetInfo()};
            var index = GeoIPCache.DB.Skip(GeoIPCache.DBHeader.Offset_cities + 4 * loc.Index).Take(4).ToArray();
            int intIndex = BitConverter.ToInt32(index, 0);

            var locationOfTheIndex= GeoIPCache.DB.Skip(GeoIPCache.DBHeader.Offset_locations + intIndex).Take(96).ToArray();
            var cityOfTheIndex = locationOfTheIndex.Skip(32).Take(24).ToArray();
            
            // вправо
            for(int i = 1; ; i++)
            {
                var nextIndex = GeoIPCache.DB.Skip(GeoIPCache.DBHeader.Offset_cities + 4 * (loc.Index + i)).Take(4).ToArray();
                int intNextIndex = BitConverter.ToInt32(nextIndex, 0);

                var locationOfTheNextIndex = GeoIPCache.DB.Skip(GeoIPCache.DBHeader.Offset_locations + intNextIndex).Take(96).ToArray();
                var cityOfTheNextIndex = locationOfTheNextIndex.Skip(32).Take(24).ToArray();
                if (ByteUtility.ByteArraysEqual(cityOfTheIndex, cityOfTheNextIndex))
                {
                    var lc = LocationLogic.GetLocationFromByteArray(locationOfTheNextIndex);
                    lc.Index = loc.Index + i;
                    list.Add(lc.GetInfo());
                }
                else break;
            }

            // влево
            for (int i = 1; ; i++)
            {
                var nextIndex = GeoIPCache.DB.Skip(GeoIPCache.DBHeader.Offset_cities + 4 * (loc.Index - i)).Take(4).ToArray();
                int intNextIndex = BitConverter.ToInt32(nextIndex, 0);

                var locationOfTheNextIndex = GeoIPCache.DB.Skip(GeoIPCache.DBHeader.Offset_locations + intNextIndex).Take(96).ToArray();
                var cityOfTheNextIndex = locationOfTheNextIndex.Skip(32).Take(24).ToArray();
                if (ByteUtility.ByteArraysEqual(cityOfTheIndex, cityOfTheNextIndex))
                {
                    var lc = LocationLogic.GetLocationFromByteArray(locationOfTheNextIndex);
                    lc.Index = loc.Index - i;
                    list.Insert(0, lc.GetInfo());
                }
                else break;
            }

            return list;
        }

        #endregion
    }
}