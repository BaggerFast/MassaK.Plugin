using System.IO.Ports;
using MassaK.Plugin.Impl.Common;
using MassaK.Plugin.Impl.Misc;
using MassaK.Plugin.Impl.Utils;

namespace MassaK.Plugin.Impl.Commands;

internal sealed class CalibrateCommand(SerialPort port) : ScaleCommandBase<VoidType>(port, MassaKCommands.CmdSetZero.Value)
{
    protected override VoidType Response()
    {
        byte[] buffer = new byte[9];
        Port.Read(buffer, 0, buffer.Length);
        return new();
    }
}