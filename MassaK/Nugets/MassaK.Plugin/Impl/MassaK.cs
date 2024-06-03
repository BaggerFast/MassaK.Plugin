using MassaK.Plugin.Abstractions.Enums;
using MassaK.Plugin.Abstractions.Events;
using MassaK.Plugin.Impl.Commands;
using MassaK.Plugin.Impl.Enums;

namespace MassaK.Plugin.Impl;

public partial class MassaK : IMassaK
{
    public event EventHandler<WeightEventArg>? WeightChanged;
    public event EventHandler<MassaKStatus>? StatusChanged;

    public MassaK(string comPort)
    {
        Port = GenerateSerialPort(comPort);
        _attachWatcher.DeviceEventArrived += DeviceAttached;
        _detachWatcher.DeviceEventArrived += DeviceDetached;
    }
    
    #region Connect
    
    public void Connect()
    {
        try
        {
            Port.Dispose();
            Port.Open();
            SetStatus(MassaKStatus.IsReady);
        }
        catch
        {
            SetStatus(MassaKStatus.IsDetached);
        }
    }

    public void Disconnect()
    {
        SetStatus(MassaKStatus.IsDisabled);
        StopWeightPolling();
        Port.Dispose();
    }

    #endregion
    
    #region Polling Weight

    public void StartWeightPolling(ushort msc = 100)
    {
        if (msc < 100)
            throw new ArgumentOutOfRangeException(nameof(msc), "The polling interval must be at least 100 milliseconds.");
        
        TimeSpan interval = TimeSpan.FromMilliseconds(msc);
        WeightTimer = new(Callback, null, TimeSpan.Zero, interval);
        return;

        void Callback(object? _)
        {
            WeightChanged?.Invoke(this, ExecuteCommand(new GetMassaCommand(Port), new(0, false)));
        }
    }
    
    public void StopWeightPolling()
    {
        WeightTimer?.Dispose();
        WeightTimer = null;
    }
    
    #endregion

    public void Calibrate() => ExecuteCommand(new CalibrateCommand(Port), VoidType.None);
}