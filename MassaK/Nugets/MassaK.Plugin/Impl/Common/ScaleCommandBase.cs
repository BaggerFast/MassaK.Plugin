using System.IO.Ports;

namespace MassaK.Plugin.Common;

internal abstract class ScaleCommandBase(SerialPort port, byte[] command)
{
    protected readonly SerialPort Port = port;

    public void Activate()
    {
        Port.Write(command, 0, command.Length);
        Response();
    }

    protected abstract void Response();
}