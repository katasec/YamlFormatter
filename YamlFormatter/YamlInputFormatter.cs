using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;


namespace Katasec.AspNet.YamlFormatter;

public class YamlInputFormatter : TextInputFormatter
{
    private readonly IDeserializer _deserializer;

    public YamlInputFormatter()
    {

        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/yaml"));
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/x-yaml"));
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/yaml"));
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/x-yaml"));

        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);

        _deserializer = new DeserializerBuilder().Build();
    }

    protected override bool CanReadType(Type type)
    {
        return true;
    }


    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
    {
        using var streamReader = context.ReaderFactory(context.HttpContext.Request.Body, encoding);
        var yaml = await streamReader.ReadToEndAsync();

        try
        {
            var result = _deserializer.Deserialize(yaml, context.ModelType);
            return await InputFormatterResult.SuccessAsync(result);
        }
        catch (Exception)
        {
            return await InputFormatterResult.FailureAsync();
        }
    }
}
