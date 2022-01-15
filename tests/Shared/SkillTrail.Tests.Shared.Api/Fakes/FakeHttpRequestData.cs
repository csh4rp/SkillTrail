using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using Microsoft.Azure.Functions.Worker.Http;

namespace SkillTrail.Tests.Shared.Api.Fakes;

public class FakeHttpRequestData : HttpRequestData
{
    private Stream _body = new MemoryStream();
    private HttpHeadersCollection? _httpHeaders = new();
    private string _method;
    private Uri _url;
    
    public FakeHttpRequestData(Uri url, string method) : base(new FakeFunctionContext())
    {
        _url = url;
        _method = method;
    }

    public override HttpResponseData CreateResponse()
    {
        throw new NotImplementedException();
    }

    public override Stream Body => _body;
    public override HttpHeadersCollection Headers => _httpHeaders;
    public override IReadOnlyCollection<IHttpCookie> Cookies { get; }
    public override Uri Url => _url;
    public override IEnumerable<ClaimsIdentity> Identities { get; }
    public override string Method => _method;

    public void SetBody(Stream stream, string contentType)
    {
        _body = stream;
        _httpHeaders ??= new HttpHeadersCollection();
        _httpHeaders.Add("ContentType", contentType);
    }

}