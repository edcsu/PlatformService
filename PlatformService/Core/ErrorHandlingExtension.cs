namespace PlatformService.Core
{
    public static class ErrorHandlingExtension
    {
        /// <summary>
        /// Insert error handling middle-ware
        /// </summary>
        /// <param name="builder">IApplication Builder extension</param>    
        /// <returns></returns>
        public static IApplicationBuilder UseGlobalErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalErrorHandlerMiddleware>();
        }
    }
}
