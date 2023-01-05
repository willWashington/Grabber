using FASTER.core;
using Grabber.Utilities;
using System.Diagnostics;

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
                // first recover with no checkpoint spits the dummy
            }

            _session = _store.NewSession(new SimpleFunctions<string, string>());
        }

        public void WriteToDisk(string payload)
        {
            using var store = new FasterKeyValueStore();
            store.Clear();
            var sw = new Stopwatch();

            int count = 0;
            sw.Start();

            store.Begin();
            store.Put($"{count}", payload);
            store.End();

            sw.Stop();
            sw.Restart();
            var g = store.Get($"{count}");

            sw.Stop();
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

        public void Begin()
        {
        }

        public void End()
        {
            var x = _store.TakeFullCheckpointAsync(CheckpointType.Snapshot);
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}


