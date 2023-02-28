using Dalamud.Game.Network;

namespace HypnotoadPlugin.Offsets;

public static unsafe class NetworkReader
{
    private static void NetworkMessage(nint dataPtr, ushort opCode, uint sourceActorId, uint targetActorId, NetworkMessageDirection direction)
    {
        if (direction == NetworkMessageDirection.ZoneDown && Pipe.Client is { IsConnected: true })
            Pipe.Client.WriteAsync(new PayloadMessage
            {
                MsgType = MessageType.NetworkPacket,
                Message = Environment.ProcessId + ":" + Convert.ToBase64String(GetPacket(dataPtr))
            });
    }

    internal static void Initialize()
    {
        if (Api.GameNetwork != null) Api.GameNetwork.NetworkMessage += NetworkMessage;
    }

    internal static void Dispose()
    {
        if (Api.GameNetwork != null) Api.GameNetwork.NetworkMessage -= NetworkMessage;
    }

    private static byte[] GetPacket(IntPtr dataPtr)
    {
        using var memoryStream = new MemoryStream();
        using var unmanagedMemoryStream = new UnmanagedMemoryStream((byte*)(dataPtr - 0x20).ToPointer(), 4096);
        unmanagedMemoryStream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}