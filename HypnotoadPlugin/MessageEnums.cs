using System.Collections.ObjectModel;

namespace HypnotoadPlugin;

public enum MessageType
{
    None = 0,
    Handshake = 1,
    Version = 2,

    SetGfx = 10,

    Instrument = 20,
    NoteOn = 21,
    NoteOff = 22,
    ProgramChange = 23,

    StartEnsemble = 30,
    AcceptReply = 31,

    Chat = 40,

    NetworkPacket = 50
}

public readonly struct ChatMessageChannelType
{
    public static readonly  ChatMessageChannelType None        = new("None",         0x0000, "");
    private static readonly ChatMessageChannelType Say         = new("Say",          0x000A, "/s");
    private static readonly ChatMessageChannelType Yell        = new("Yell",         0x001E, "/y");
    private static readonly ChatMessageChannelType Shout       = new("Shout",        0x000B, "/sh");
    private static readonly ChatMessageChannelType Party       = new("Party",        0x000E, "/p");
    private static readonly ChatMessageChannelType FreeCompany = new("Free Company", 0x0018, "/fc");

    private static readonly IReadOnlyList<ChatMessageChannelType> All = new ReadOnlyCollection<ChatMessageChannelType>(new List<ChatMessageChannelType>
    {
        None,
        Say,
        Yell,
        Shout,
        Party,
        FreeCompany
    });

    private string Name { get; }

    private int ChannelCode { get; }
    public string ChannelShortCut { get; }

    private ChatMessageChannelType(string name, int channelCode, string channelShortCut)
    {
        Name            = name;
        ChannelCode     = channelCode;
        ChannelShortCut = channelShortCut;
    }

    public static ChatMessageChannelType ParseByChannelCode(int channelCode)
    {
        var _ = TryParseByChannelCode(channelCode, out var result);
        return result;
    }

    private static bool TryParseByChannelCode(int channelCode, out ChatMessageChannelType result)
    {
        if (All.Any(x => x.ChannelCode.Equals(channelCode)))
        {
            result = All.First(x => x.ChannelCode.Equals(channelCode));
            return true;
        }
        result = None;
        return false;
    }
}