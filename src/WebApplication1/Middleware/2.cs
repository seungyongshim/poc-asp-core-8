using WebApplication1.Middleware;

namespace Microsoft.AspNetCore.Builder;

public static class ResponseCompressionBuilderExtensions1
{
    public static IApplicationBuilder UseResponseCompression(this IApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder.UseMiddleware<ResponseCustomMiddleware>();
    }
}
