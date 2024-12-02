using MediatR;
using Serilog;

namespace CafeEmployeeManagement.Application.Common.Behaviours
{
    internal class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger logger;

        public LoggingBehaviour(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Logging the Request and Response for testing purposes only. We must avoid doing this for production applications.
            logger.Information("Handling {RequestName} with data: {@RequestData}", typeof(TRequest).Name, request);


            var response = await next();

            logger.Information("Handled {RequestName}, response: {@ResponseData}", typeof(TRequest).Name, response);

            return response;
        }
    }
}
