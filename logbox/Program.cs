using Microsoft.Extensions.Logging;

const string version = "1.2.0";

var options = new Options(args);
var logger = Factory.CreateLogger(options);
var counter = 0;

logger.LogInformation($"Logbox {version} '{options.Name}' started, codec: '{options.Codec}', time span ms: '{options.TimeSpan}', include scope: '{options.IncludeScope}'");
while (true)
{
   using (var act = new LoggingActivity())
      act.Log(logger, ++counter);
   Thread.Sleep(options.TimeSpan);
}