using Polly;
using Polly.Extensions.Http;
using System.Net;

namespace PlatformService.Core
{
    public static class PollyPolicies
    {
        /// <summary>
        /// The retry policy configured to occur 6 times exponentially
        /// </summary>
        /// <returns></returns>
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                //.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        /// <summary>
        ///  The circuit breaker policy configured so it breaks or opens the circuit when there have been 3 consecutive faults when retrying the Http requests.
        /// </summary>
        /// <returns></returns>
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));
        }
    }
}
