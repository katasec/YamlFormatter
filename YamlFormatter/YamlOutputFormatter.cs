using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using Microsoft.Net.Http.Headers;
using System.Runtime;

namespace Katasec.AspNet.YamlFormatter;

public class YamlOutputFormatter : TextOutputFormatter
{
    private readonly ISerializer _serializer;

    public YamlOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/yaml"));
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/x-yaml"));
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/yaml"));
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/x-yaml"));

        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);

        _serializer = new SerializerBuilder().Build();
    }

    protected override bool CanWriteType(Type? type)
    {
        return true;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        using var writer = new StreamWriter(context.HttpContext.Response.Body, selectedEncoding);
        _serializer.Serialize(writer, context.Object);
        await writer.FlushAsync();
        await writer.DisposeAsync();
    }
}

