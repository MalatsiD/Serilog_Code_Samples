using LoggingAPI.Models;
using MediatR;

namespace LoggingAPI.Behaviors
{
    public class LoggingPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly ILogger _logger;

        public LoggingPipelineBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //Structured logging
            _logger.LogInformation("Starting request {@RequestName}, {@DateTimeUtc}", typeof(TRequest).Name, DateTime.UtcNow);

            var result = await next();

           if(result.IsFailure)
            {
                _logger.LogError("Request failure {@RequestName}, {@error}, {@DateTimeUtc}", 
                    typeof(TRequest).Name, result.Error, DateTime.UtcNow);
            }

            _logger.LogInformation("Completed request {@RequestName}, {@DateTimeUtc}", typeof(TRequest).Name, DateTime.UtcNow);

            return result;
        }
    }
}
