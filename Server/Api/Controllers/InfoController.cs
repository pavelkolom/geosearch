using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    /// <summary>
    /// InfoController
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class InfoController : ControllerBase
    {
        /// <summary>
        /// InfoController constructor
        /// </summary>
        public InfoController()
        {
        }

        /// <summary>
        /// Получение информации о БД
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new[] { "Вспомогательные методы" })]
        public string Get()
        {
            return new Header()
            {
                Version = Cache.GeoIPCache.DB.Take(4).ToArray(),
                Name = Cache.GeoIPCache.DB.Skip(4).Take(32).ToArray(),
                Timestamp = Cache.GeoIPCache.DB.Skip(36).Take(8).ToArray(),
                Records = Cache.GeoIPCache.DBHeader.Records,
                Offset_ranges = Cache.GeoIPCache.DBHeader.Offset_ranges,
                Offset_cities = Cache.GeoIPCache.DBHeader.Offset_cities,
                Offset_locations = Cache.GeoIPCache.DBHeader.Offset_locations,
                ReadInmilliseconds = Cache.GeoIPCache.DBHeader.ReadInmilliseconds
            }.ToString();
        }
    }
}