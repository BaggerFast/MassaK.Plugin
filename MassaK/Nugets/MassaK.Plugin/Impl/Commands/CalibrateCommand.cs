using System.IO.Ports;
using MassaK.Plugin.Common;
using MassaK.Plugin.Utils;

namespace MassaK.Plugin.Impl.Commands;

internal class CalibrateCommand(SerialPort port) : ScaleCommandBase(port, MassaKCommands.CmdSetZero.Value)
{
    protected override void Response()
    {
        byte[] buffer = new byte[9];
        Port.Read(buffer, 0, buffer.Length);
    }
}