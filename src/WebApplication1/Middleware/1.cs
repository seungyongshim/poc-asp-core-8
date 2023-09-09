using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using System.Diagnostics;

namespace WebApplication1.Middleware;

public class ResponseCompressionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IResponseCompressionProvider _provider;

    public ResponseCompressionMiddleware(RequestDelegate next, IResponseCompressionProvider provider)
    {
        ArgumentNullException.ThrowIfNull(next);
        ArgumentNullException.ThrowIfNull(provider);

        _next = next;
        _provider = provider;
    }

    public Task Invoke(HttpContext context)
    {
        if (!_provider.CheckRequestAcceptsCompression(context))
        {
            return _next(context);
        }
        return InvokeCore(context);
    }

    private async Task InvokeCore(HttpContext context)
    {
        var originalBodyFeature = context.Features.Get<IHttpResponseBodyFeature>();
        var originalCompressionFeature = context.Features.Get<IHttpsCompressionFeature>();

        Debug.Assert(originalBodyFeature != null);

        var compressionBody = new ResponseCompressionBody(context, _provider, originalBodyFeature);
        context.Features.Set<IHttpResponseBodyFeature>(compressionBody);
        context.Features.Set<IHttpsCompressionFeature>(compressionBody);

        try
        {
            await _next(context);
            await compressionBody.FinishCompressionAsync();
        }
        finally
        {
            context.Features.Set(originalBodyFeature);
            context.Features.Set(originalCompressionFeature);
        }
    }
}
