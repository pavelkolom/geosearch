using Api.BusinessLogic;
using Api.Cache;
using Api.Models;
using Api.Util;

namespace Api.Services
{
    public class InitializeCacheService : IHostedService
    {
        private readonly string _DBPath;
        private readonly int _headerSize;
        private readonly int _locatonSize;
        private readonly int _iPRangeSize;
        private readonly IServiceProvider _serviceProvider;
        private readonly IParser _parser;

        public InitializeCacheService(IServiceProvider serviceProvider, IConfiguration configuration, IParser parser)
        {
            _serviceProvider = serviceProvider;
            _parser = parser;
            _DBPath = configuration["DBPath"];
            int.TryParse(configuration["HeaderSize"], out _headerSize);
            int.TryParse(configuration["LocationSize"], out _locatonSize);
            int.TryParse(configuration["IPRangeSize"], out _iPRangeSize);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                byte[] fileBytes = await DBUtility.ReadBytesFromFileAsync(_DBPath);
                _parser.ParseDB(fileBytes, _headerSize, _iPRangeSize, _locatonSize);
            }

            return;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
