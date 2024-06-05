namespace MassaK.Plugin.Impl.Utils;

internal static class MassaKCommands
{
    public static readonly Lazy<byte[]> CmdGetWeight = new(() => CrcUtils.Generate(0x23));
    public static readonly Lazy<byte[]> CmdSetZero = new(() => CrcUtils.Generate(0x72));
}