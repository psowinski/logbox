using System.Collections;

public enum Codec
{
   Simple,
   Json,
   Systemd,
   OpenTelemetry
}

public class Options
{
   public Options(string[] args)
   {
      DefaultValues();
      ApplyEnv();
      ApplyArgs(args);
   }

   public void DefaultValues()
   {
      TimeSpan = new Random().Next(1, 10) * 1000;
      Codec = Codec.Simple;
      IncludeScope = true;

      var rnd = new Random();
      Name = $"{LogData.Names[rnd.Next(0, LogData.Names.Length)]} #{rnd.Next(100000, 1000000)}";
   }

   public void ApplyEnv()
   {
      LoadLoggerType(Environment.GetEnvironmentVariable("logbox.codec"));
      LoadTimeSpan(Environment.GetEnvironmentVariable("logbox.timespan"));
      LoadName(Environment.GetEnvironmentVariable("logbox.name"));
      LoadIncludeScope(Environment.GetEnvironmentVariable("logbox.scope"));
   }

   public void ApplyArgs(string[] args)
   {
      for (var idx = 0; idx < args.Length; ++idx)
      {
         var arg = args[idx];
         var argAttr = idx + 1 < args.Length ? args[idx + 1] : null;
         if (arg == "-c")
         {
            LoadLoggerType(argAttr);
            ++idx;
         }
         else if (arg == "-t")
         {
            LoadTimeSpan(argAttr);
            ++idx;
         }
         else if (arg == "-n")
         {
            LoadName(argAttr);
            ++idx;
         }
         else if (arg == "-nos")
            IncludeScope = true;
      }
   }

   private void LoadLoggerType(string? val)
   {
      if (val is null) return;
      var codec = RecognizeLoggerType(val);
      if (codec.HasValue)
         Codec = codec.Value;
   }

   private void LoadTimeSpan(string? val)
   {
      if (val is null) return;
      var timespan = ParseTimeSpan(val);
      if (timespan.HasValue)
         TimeSpan = timespan.Value;
   }

   private void LoadName(string? val)
   {
      if (val is null) return;
      if (!string.IsNullOrWhiteSpace(val))
         Name = val;
   }

   private void LoadIncludeScope(string? val)
   {
      if (val is null) return;
      var boolVal = ParseBool(val);
      if (boolVal.HasValue)
         IncludeScope = boolVal.Value;
   }

   private static bool? ParseBool(string val)
   {
      if (bool.TryParse(val, out var boolVal))
         return boolVal;
      return null;
   }

   private static int? ParseTimeSpan(string val)
   {
      if (int.TryParse(val, out var time) && time > 0)
         return time;
      return null;
   }

   private static Codec? RecognizeLoggerType(string name)
      => name switch
      {
         "json" => Codec.Json,
         "simple" => Codec.Simple,
         "systemd" => Codec.Systemd,
         "opent" => Codec.OpenTelemetry,
         _ => null
      };

   public Codec Codec { get; private set; }
   public int TimeSpan { get; private set; }
   public string Name { get; private set; }
   public bool IncludeScope { get; private set; }
}
