using System.IO.Ports;
using MassaK.Plugin.Abstractions.Events;
using MassaK.Plugin.Impl.Common;
using MassaK.Plugin.Impl.Exceptions;
using MassaK.Plugin.Impl.Utils;

namespace MassaK.Plugin.Impl.Commands;

internal class GetMassaCommand(SerialPort port): ScaleCommandBase<WeightEventArg>(port, MassaKCommands.CmdGetWeight.Value)
{
    protected override WeightEventArg Response()
    {
        byte[] buffer = new byte[20];
        int bytesRead = Port.Read(buffer, 0, buffer.Length);
        
        if (buffer[5] != 0x24 || bytesRead < buffer.Length) throw new MassaKResponseException();

        int packetLen = BitConverter.ToUInt16(buffer.Skip(3).Take(2).ToArray(), 0);
        byte[] packet = buffer.Skip(5).Take(packetLen).ToArray();

        ushort getCrc = BitConverter.ToUInt16(buffer.Skip(18).Take(2).ToArray(), 0);
        ushort generatedCrc = CrcUtils.CalculateCrc16(packet);

        if (getCrc != generatedCrc) throw new MassaKResponseException();

        int weight = BitConverter.ToInt32(buffer.Skip(6).Take(4).ToArray(), 0);
        bool isStable = buffer[11] == 0x01;
        
        return new(weight, isStable);
    }
}