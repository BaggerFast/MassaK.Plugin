using MassaK.Plugin.Abstractions.Enums;
using MassaK.Plugin.Abstractions.Events;
using MassaK.Plugin.Impl.Commands;
using MassaK.Plugin.Impl.Utils;

namespace MassaK.Plugin.Impl;

public partial class MassaUsb : IMassaK
{
    public event EventHandler<WeightEventArg>? OnWeightChanged;
    public event EventHandler<MassaKStatus>? OnStatusChanged;

    public MassaUsb(string comPort)
    {
        Port = SerialPortUtils.Generate(comPort);
        _attachWatcher.DeviceEventArrived += DeviceAttached;
        _detachWatcher.DeviceEventArrived += DeviceDetached;
    }
    
    #region Connect
    
    public void Connect()
    {
        try
        {
            if (Port.IsOpen && Status is MassaKStatus.Initializing or MassaKStatus.Ready)
                return;
            
            Port.Dispose();
            Port.Open();
            
            if (Status == MassaKStatus.Initializing) return;
            
            SetStatus(MassaKStatus.Ready);
        }
        catch
        {
            SetStatus(MassaKStatus.Detached);
        }
    }

    public void Disconnect()
    {
        SetStatus(MassaKStatus.Disabled);
        StopWeightPolling();
        Port.Dispose();
    } 
    
    public void Dispose()
    {
        Disconnect();
        
        _attachWatcher.DeviceEventArrived -= DeviceAttached;
        _attachWatcher.Dispose();
        
        _detachWatcher.DeviceEventArrived -= DeviceDetached;
        _detachWatcher.Dispose();

        GC.SuppressFinalize(this);
    }
    
    #endregion
    
    #region Polling Weight

    public void StartWeightPolling(ushort msc = 100)
    {
        if (msc < 100)
            throw new ArgumentOutOfRangeException(nameof(msc), "The polling interval must be at least 100 milliseconds.");

        StopWeightPolling();
        
        WeightTimer = new(Callback, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(msc));
        return;

        void Callback(object? _)
        {
            WeightEventArg newWeight = ExecuteCommand(new GetMassaCommand(Port)) ?? new(0, false);
            if (newWeight == Weight) return;
            Weight = newWeight;
            OnWeightChanged?.Invoke(this, Weight);
        }
    }
    
    public void StopWeightPolling()
    {
        WeightTimer?.Dispose();
        WeightTimer = null;
    }
    
    #endregion

    public void Calibrate() => ExecuteCommand(new CalibrateCommand(Port));
}