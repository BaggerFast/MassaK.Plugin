using System.IO.Ports;
using MassaK.Plugin.Abstractions.Enums;
using MassaK.Plugin.Abstractions.Events;
using MassaK.Plugin.Impl.Common;
using MassaK.Plugin.Impl.Enums;
using MassaK.Plugin.Impl.Exceptions;
using MassaK.Plugin.Impl.Misc;

namespace MassaK.Plugin.Impl;

public partial class MassaUsb
{
    #region Variables

    private Timer? InitTimer { get; set; }
    private Timer? WeightTimer { get; set; }
    private SerialPort Port { get; set; }
    private MassaKStatus Status { get; set; } = MassaKStatus.Disabled;

    private WeightEventArg Weight { get; set; } = new(0, false);

    private readonly DeviceEventWatcher _attachWatcher = new(DeviceEventType.Connect);
    private readonly DeviceEventWatcher _detachWatcher = new(DeviceEventType.Disconnect);
    
    #endregion

    #region Inner

    private void SetStatus(MassaKStatus status)
    {
        if (status == Status) return;
        
        if (status is MassaKStatus.Detached or MassaKStatus.Disabled)
        {
            Port.Dispose();
            InitTimer?.Dispose();
            SetWeight(0, false);
        }
        Status = status;
        OnStatusChanged?.Invoke(this, status);
    }

    private void SetWeight(int weight, bool isStable)
    {
        WeightEventArg newWeight = new(weight, isStable);
        if (newWeight == Weight) return;
        Weight = newWeight;
        OnWeightChanged?.Invoke(this, Weight);
    }
    
    private T? ExecuteCommand<T>(ScaleCommandBase<T> command)
    {
        try
        {
            if (Status == MassaKStatus.Ready)
                return command.Request();
        }
        catch (MassaKConnectionException)
        {
            Connect();
        }
        catch (Exception)
        {
            // ignored
        }
        return default;
    }

    #endregion
    
    #region Monitoring com port
    
    private void DeviceAttached(object? sender, EventArgs e)
    {
        if (Status != MassaKStatus.Detached || Port.IsOpen) return;
        
        SetStatus(MassaKStatus.Initializing);
        Connect();
            
        InitTimer = new(DeviceInit, null, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(15));
    }
    
    private void DeviceDetached(object? sender, EventArgs e)
    {
        if (!Port.IsOpen && Status is MassaKStatus.Ready or MassaKStatus.Initializing)
            SetStatus(MassaKStatus.Detached);
    }

    private void DeviceInit(object? _)
    {
        if (Status == MassaKStatus.Initializing)
            SetStatus(MassaKStatus.Ready);
        
        InitTimer?.Dispose();
    }
    
    #endregion
}