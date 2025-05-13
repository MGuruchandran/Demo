using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DemoLLD.Service.Services.Handler;

public class MessageHandlerStepOne : DelegatingHandler
{
    public MessageHandlerStepOne() : base(new HttpClientHandler())
    {
        
    }
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Clear();
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        request.Headers.Add("User-Agent", ".NET Foundation Repository Reporter");
        return base.SendAsync(request, cancellationToken);
    }
}
