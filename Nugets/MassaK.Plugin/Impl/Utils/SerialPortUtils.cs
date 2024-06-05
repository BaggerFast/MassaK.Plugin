using System.IO.Ports;

namespace MassaK.Plugin.Impl.Utils;

internal static class SerialPortUtils
{
    public static SerialPort Generate(string comPort)
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
}