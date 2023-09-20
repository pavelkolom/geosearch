using Api.BusinessLogic;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    /// <summary>
    /// CityController
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CityController : ControllerBase
    {
        private readonly ISearcher _searcher;

        /// <summary>
        /// CityController constructor
        /// </summary>
        /// <param name="searcher"></param>
        public CityController(ISearcher searcher)
        {
            _searcher = searcher;
        }

        /// <summary>
        /// Получение 100 местоположений, отсортированных по городу
        /// </summary>
        /// <param name="fromIndex">Стартовый индекс</param>
        /// <returns></returns>
        [HttpGet("get100locationssortedbycity")]
        [SwaggerOperation(Tags = new[] { "Вспомогательные методы" })]
        public IEnumerable<LocationInfo> Get(int fromIndex = 0)
        {
            return LocationLogic.GetSortedByCity().Skip(fromIndex).Take(100);
        }

        /// <summary>
        /// Получение местоположений по названию города (бинарный поиск)
        /// </summary>
        /// <param name="city">Название города</param>
        /// <returns></returns>
        [HttpGet("locations")]
        [SwaggerOperation(Tags = new[] { "Методы поиска по городy" })]
        public IEnumerable<LocationInfo>? Get(string city = "cit_Ixiluza")
        {
            return _searcher.SearchLocationsByCityName(city);
        }
    }
}