using System.IO.Ports;
using MassaK.Plugin.Impl.Exceptions;

namespace MassaK.Plugin.Impl.Common;

internal abstract class ScaleCommandBase<T> (SerialPort port, byte[] command)
{
    protected readonly SerialPort Port = port;

    public T Request()
    {
        try
        {
            if (!Port.IsOpen) throw new MassaKConnectionException();
            Port.Write(command, 0, command.Length);
        }
        catch
        {
            throw new MassaKConnectionException();
        }
        return Response();
    }

    protected abstract T Response();
}