using System.Management;
using MassaK.Plugin.Impl.Enums;

namespace MassaK.Plugin.Impl.Misc;

internal sealed class DeviceEventWatcher : IDisposable
{
    private readonly ManagementEventWatcher _watcher;
    public event EventHandler? DeviceEventArrived;

    public DeviceEventWatcher(DeviceEventType eventType)
    {
        int eventIndex = eventType == DeviceEventType.Connect ? 2 : 3;
        string query = $"SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = {eventIndex}";

        _watcher = new(new WqlEventQuery(query));
        _watcher.Start();
        
        _watcher.EventArrived += OnEventArrived;
    }
    
    private void OnEventArrived(object sender, EventArrivedEventArgs args)
    {
        DeviceEventArrived?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        _watcher.EventArrived -= OnEventArrived;
        _watcher.Stop();
        _watcher.Dispose();
    }
}