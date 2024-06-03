using System.Management;
using MassaK.Plugin.Impl.Enums;

namespace MassaK.Plugin.Impl.Misc;

internal class DeviceEventWatcher : IDisposable
{
    private readonly ManagementEventWatcher _watcher;
    public event EventHandler? DeviceEventArrived;
    
    public DeviceEventWatcher(DeviceEventType eventType)
    {
        string query =
            $"SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = {(eventType == DeviceEventType.Connect ? 2 : 3)}";

        _watcher = new(new WqlEventQuery(query));
        _watcher.Start();
        
        _watcher.EventArrived += (sender, args) =>
        {
            DeviceEventArrived?.Invoke(this, args);
        };
    }
    
    public void Dispose()
    {
        _watcher.Stop();
        _watcher.Dispose();
    }
}