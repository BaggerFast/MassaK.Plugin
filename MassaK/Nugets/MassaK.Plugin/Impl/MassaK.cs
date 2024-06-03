using MassaK.Plugin.Abstractions.Enums;
using MassaK.Plugin.Abstractions.Events;
using MassaK.Plugin.Impl.Commands;

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

    public void StartWeightPolling()
    {
        lock (_pollingLock)
        {
            if (!_cancelPollingToken.IsCancellationRequested)
                return;

            _cancelPollingToken = new();
            CancellationToken cancellationToken = _cancelPollingToken.Token;


            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    ExecuteCommand(new GetMassaCommand(Port));
                    await Task.Delay(200, cancellationToken);
                }
            }, cancellationToken);
        }
    }
    
    public void StopWeightPolling()
    {
        lock (_pollingLock)
        {
            if (_cancelPollingToken.IsCancellationRequested)
                return;
            _cancelPollingToken.Cancel();
        }
    }
    
    #endregion

    public void Calibrate() => ExecuteCommand(new CalibrateCommand(Port));
}