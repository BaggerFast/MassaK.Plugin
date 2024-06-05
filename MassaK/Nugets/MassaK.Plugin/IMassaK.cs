using MassaK.Plugin.Abstractions.Enums;
using MassaK.Plugin.Abstractions.Events;

namespace MassaK.Plugin;

public interface IMassaK : IDisposable
{
    #region Connect

    /// <summary>
    /// Establishes a serial port connection to the MassaK. Use <see cref="OnStatusChanged"/>
    /// </summary>
    void Connect();
    
    /// <summary>
    /// Disconnects the serial port connection from the MassaK.
    /// </summary>
    void Disconnect();

    #endregion

    #region Polling Weight

    /// <summary>
    /// Starts polling the weight from the MassaK device at a specified interval. Use <see cref="OnWeightChanged"/>
    /// </summary>
    /// <param name="msc">The interval in milliseconds between weight requests. The minimum value is 100 milliseconds (use cautiously).</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the interval is less than 100 milliseconds.</exception>
    void StartWeightPolling(ushort msc = 100);
    
    /// <summary>
    /// Stops polling the weight from the MassaK device.
    /// </summary>
    void StopWeightPolling();

    #endregion

    #region Commands

    /// <summary>
    /// Calibrate MassaK device.
    /// </summary>
    void Calibrate();

    #endregion

    #region Events

    /// <summary>
    /// Subscribe to this event to receive notifications about weight changes from the MassaK adapter.
    /// </summary>
    public event EventHandler<WeightEventArg> OnWeightChanged;
    
    /// <summary>
    /// Subscribe to this event to receive notifications about the connection status of the MassaK adapter.
    /// </summary>
    public event EventHandler<MassaKStatus> OnStatusChanged;
    
    #endregion
}