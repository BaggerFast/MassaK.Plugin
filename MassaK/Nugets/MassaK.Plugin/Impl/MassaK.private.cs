using System.IO.Ports;
using MassaK.Plugin.Abstractions.Enums;
using MassaK.Plugin.Common;
using MassaK.Plugin.Impl.Enums;
using MassaK.Plugin.Impl.Misc;

namespace MassaK.Plugin.Impl;

public partial class MassaK
{
    private SerialPort Port { get; set; }
    private MassaKStatus Status { get; set; } = MassaKStatus.IsDisabled;

    private readonly DeviceEventWatcher _attachWatcher = new(DeviceEventType.Connect);
    private readonly DeviceEventWatcher _detachWatcher = new(DeviceEventType.Disconnect);
    
    
    private readonly object _pollingLock = new();
    private CancellationTokenSource _cancelPollingToken = new();
    
    private static SerialPort GenerateSerialPort(string comPort)
    {
        return new()
        {
            PortName = comPort.ToUpper(),
            ReadTimeout = 0_100,
            WriteTimeout = 0_100,
            BaudRate = 57600,
            Parity = Parity.None,
            StopBits = StopBits.One,
            DataBits = 8,
            Handshake = Handshake.RequestToSend
        };
    }

    private void SetStatus(MassaKStatus status)
    {
        Status = status;
        StatusChanged?.Invoke(this, Status);
    }

    private void ExecuteCommand(ScaleCommandBase command)
    {
        try
        {
            switch (Status)
            {
                case MassaKStatus.IsReady:
                    command.Activate();
                    return;
                case MassaKStatus.IsDisabled:
                    Connect();
                    break;
                default:
                    Connect();
                    break;
            }
        }
        catch (TimeoutException)
        {
            
        }
        catch (Exception)
        {
            Connect();
        }
    }

    #region Monitoring com port
    
    private void DeviceAttached(object? sender, EventArgs e)
    {
        if (Status == MassaKStatus.IsDetached && !Port.IsOpen)
            Connect();
    }
    
    private void DeviceDetached(object? sender, EventArgs e)
    {
        if (Status == MassaKStatus.IsReady && !Port.IsOpen)
            SetStatus(MassaKStatus.IsDetached);
    }

    #endregion

    public void Dispose()
    {
        Disconnect();
        
        _attachWatcher.DeviceEventArrived -= DeviceAttached;
        _detachWatcher.DeviceEventArrived -= DeviceDetached;
        
        _attachWatcher.Dispose();
        _detachWatcher.Dispose();
    }
}