using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.BusinessLogic;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    /// <summary>
    /// IpController
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class IpController : ControllerBase
    {
        private readonly ISearcher _searcher;

        /// <summary>
        /// IpController constructor
        /// </summary>
        /// <param name="searcher"></param>
        public IpController(ISearcher searcher)
        {
            _searcher = searcher;
        }

        /// <summary>
        /// Получение местоположения по IP адресу (бинарный поиск)
        /// </summary>
        /// <param name="ip">IP адрес</param>
        /// <returns></returns>
        [HttpGet("location")]
        [SwaggerOperation(Tags = new[] { "Методы поиска по IP" })]
        public async Task<LocationInfo?> GetAsync(string ip = "157.6.252.96")
        {
            return _searcher.SearchLocationByIp(ip);
        }
    }
}