using MassaK.Plugin.Abstractions.Enums;
using MassaK.Plugin.Abstractions.Events;
using MassaK.Plugin.Abstractions.Exceptions;

namespace MassaK.Plugin;

public interface IMassaK : IDisposable
{
    #region Connect

    /// <summary>
    /// Establishes a serial port connection to the MassaK.
    /// </summary>
    /// <exception cref="MassaKConnectionException">Thrown when the connection to the printer is lost.</exception>
    void Connect();
    
    /// <summary>
    /// Disconnects the serial port connection from the MassaK.
    /// </summary>
    void Disconnect();

    #endregion

    #region Polling Weight

    void StartWeightPolling();
    void StopWeightPolling();

    #endregion

    #region Commands

    void Calibrate();

    #endregion

    #region Events

    /// <summary>
    /// Subscribe to this event to receive notifications about weight changes from the MassaK adapter.
    /// </summary>
    public event EventHandler<WeightEventArg> WeightChanged;
    
    /// <summary>
    /// Subscribe to this event to receive notifications about the connection status of the MassaK adapter.
    /// </summary>
    public event EventHandler<MassaKStatus> StatusChanged;
    
    #endregion
}