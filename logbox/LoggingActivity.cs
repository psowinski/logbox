using System.Diagnostics;
using Microsoft.Extensions.Logging;

public class LoggingActivity : IDisposable
{
   private readonly Activity activity;
   private readonly Random random = new ();

   public LoggingActivity()
   {
      this.activity = new Activity("LogMe").Start();
      Activity.Current = activity;
   }

   public void Log(ILogger logger, int counter)
   {
      using (logger.BeginScope("TraceID: {traceId} SpanId: {spanId} ScopeId: {scopeId}", activity.TraceId, activity.SpanId, Guid.NewGuid()))
      {
         foreach (var idx in Enumerable.Range(0, random.Next(1, 6)))
         {
            LogSentence(logger, counter, idx + 1);
         }
      }
   }

   private void LogSentence(ILogger logger, int counter, int idx)
   {
      var id = random.Next(0, LogData.Sentences.Length);
      var sentence = LogData.Sentences[id];
      var logLevel = (LogLevel) random.Next(2, 6);
      logger.Log(logLevel, new EventId(id), "[{counter}/{idx}: {sentence}]", counter, idx, sentence);
   }

   public void Dispose()
   {
      this.activity.Stop();
   }
}