using FASTER.core;
using Grabber.Execution;
using Grabber.Models;
using Grabber.Utilities;
using System.Diagnostics;
using System.Text.Json;

namespace Grabber
{
    /// <summary>
    /// This is a basic sample of FasterKV using value types
    /// </summary>

    public class FasterKeyValueStore : IValueStore, IDisposable
    {
        private ClientSession<string, string, string, string, Empty, IFunctions<string, string, string, string, Empty>>
            _session;

        private FasterKV<string, string> _store;

        private static int Count = 0;

        public FasterKeyValueStore()
        {
            var log = Devices.CreateLogDevice(@"hlog.log");
            var objlog = Devices.CreateLogDevice(@"hlog.obj.log");

            var logSettings = new LogSettings
            {
                LogDevice = log,
                ObjectLogDevice = objlog,
            };

            _store = new FasterKV<string, string>(
                size: 1L << 20,
                logSettings: logSettings,
                checkpointSettings: new CheckpointSettings()
                {
                    CheckpointDir = @".",
                }
            );

            try
            {
                _store.Recover();
            }
            catch (Exception e)
            {
                //ignore the error for some reason per the devs
                Console.WriteLine($"Error ignored: {e.Message} --> {e.InnerException}");
            }

            _session = _store.NewSession(new SimpleFunctions<string, string>());
        }

        public static int ConvertPayloadToJSONAndWriteToDisk<T>(T payload)
        {
            string? json;
            switch (payload)
            {
                case RedditPayload redditPayload:
                    GrabberLogger.ReportPayload(redditPayload);
                    json = JsonSerializer.Serialize(redditPayload);                    
                    return WriteToDisk(json);
                case PolygonPayload polygonPayload:
                    GrabberLogger.ReportPayload(polygonPayload);
                    json = JsonSerializer.Serialize(polygonPayload);
                    return WriteToDisk(json);
                default:
                    return 0;
            }
        }

        public static int WriteToDisk(string payload)
        {
            using var store = new FasterKeyValueStore();
            store.Clear();
            var sw = new Stopwatch();

            Count++;
            sw.Start();

            store.Put($"{Count}", payload);

            sw.Stop();
            sw.Restart();
            var g = store.Get($"{Count}");

            sw.Stop();
            return Count;
        }

        public void Put(string key, string value)
        {
            _session.Upsert(ref key, ref value);
        }

        public string Get(string key)
        {
            var value = "";
            var status = _session.Read(ref key, ref value);
            return value;
        }

        public void Clear()
        {
        }

        public void Begin(List<Payload> payloads)
        {
            foreach (var payload in payloads)
            {
                var key = ConvertPayloadToJSONAndWriteToDisk(payload);
                var jsonToConvert = Get($"{key}");
                if (jsonToConvert.ToLower().Contains("upvotes"))
                {
                    RedditPayload? redditPayload = JsonSerializer.Deserialize<RedditPayload>(jsonToConvert);
                    ValidateDiskSaveAndCleanUp(payload, redditPayload);
                }

                if (jsonToConvert.ToLower().Contains("description"))
                {
                    PolygonPayload? polygonPayload = JsonSerializer.Deserialize<PolygonPayload>(jsonToConvert);
                    ValidateDiskSaveAndCleanUp(payload, polygonPayload: polygonPayload);
                }
            }
            GrabberLogger.Quiet = true;
            End();
        }

        private void ValidateDiskSaveAndCleanUp(Payload payload, RedditPayload? redditPayload = null, PolygonPayload polygonPayload = null)
        {
            if (redditPayload == null && polygonPayload == null)
            {
                GrabberLogger.Log("No payload to validate!", ConsoleColor.Red);
                throw new Exception("No payload to validate!");
            }

            if (redditPayload != null)
            {
                if (payload.Title.ToLower() != redditPayload?.Title.ToLower())
                {
                    GrabberLogger.Log("Reddit save data does not match recovery data.", ConsoleColor.Red);
                    throw new Exception("Reddit save data does not match recovery data.");
                }
            }

            if (polygonPayload != null)
            {
                if (payload.Title.ToLower() != polygonPayload?.Title.ToLower())
                {
                    GrabberLogger.Log("Polygon save data does not match recovery data.", ConsoleColor.Red);
                    throw new Exception("Polygon save data does not match recovery data.");
                }
            }            
        }

        public void End()
        {
            Reacher.Cleanup();
            var x = _store.TakeFullCheckpointAsync(CheckpointType.Snapshot);
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}


