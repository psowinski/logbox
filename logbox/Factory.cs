using System.Text.Json;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

public static class Factory
{
   private const string IsoTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK";
   public static ILogger CreateLogger(Options options)
   {
      using var factory = LoggerFactory.Create(builder =>
      {
         if (options.Codec == Codec.Simple)
            builder.AddSimpleConsole(opt =>
            {
               opt.IncludeScopes = options.IncludeScope;
               opt.TimestampFormat = $"{IsoTimeFormat} ";
               opt.SingleLine = true;
            });
         else if (options.Codec == Codec.Systemd)
            builder.AddSystemdConsole(opt =>
            {
               opt.IncludeScopes = options.IncludeScope;
               opt.TimestampFormat = $"{IsoTimeFormat} ";
            });
         else if (options.Codec == Codec.Json)
            builder.AddJsonConsole(opt =>
            {
               opt.IncludeScopes = options.IncludeScope;
               opt.TimestampFormat = IsoTimeFormat;
               opt.JsonWriterOptions = new JsonWriterOptions
               {
                  Indented = true
               };
            });
         else if (options.Codec == Codec.OpenTelemetry)
            builder.AddOpenTelemetry(opt =>
            {
               opt.IncludeScopes = options.IncludeScope;
               opt.AddConsoleExporter();
               opt.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("logbox"));
            });
         else 
            throw new Exception("Unknown codec");
      });
      return factory.CreateLogger($"<{options.Name}>");
   }
}
