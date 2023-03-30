﻿using Dalamud.Data;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Party;

namespace HypnotoadPlugin.Offsets;

public readonly struct PlayerInfo
{
    // Struct for holding the playerInfo relevant to our purposes. These values
    // are fetched using the various methods throughout the PartyHandler class
    public PlayerInfo(string? name, string world, string region)
    {
        PlayerName   = name;
        PlayerWorld  = world;
        PlayerRegion = region;
    }

    private string? PlayerName { get; }
    private string PlayerWorld { get; }
    private string PlayerRegion { get; }
    public override string ToString() => $"{PlayerName} [{PlayerWorld}, {PlayerRegion}]";
}


public class Collector
{
    #region Const/Dest
    private static readonly Lazy<Collector> LazyInstance = new(() => new Collector());

    private Collector()
    {
    }

    public static Collector Instance => LazyInstance.Value;

       
    public void Initialize(DataManager? data, ClientState? clientState, PartyList? partyList)
    {
        Data        = data;
        ClientState = clientState;
        PartyList   = partyList;
        if (ClientState != null)
        {
            ClientState.Login  += ClientState_Login;
            ClientState.Logout += ClientState_Logout;
        }
    }

    ~Collector() => Dispose();
    public void Dispose()
    {
        if (ClientState != null)
        {
            ClientState.Login  -= ClientState_Login;
            ClientState.Logout -= ClientState_Logout;
        }

        GC.SuppressFinalize(this);
    }
    #endregion

    internal DataManager? Data;
    internal ClientState? ClientState;
    internal PartyList? PartyList;

    /// <summary>
    /// Only called when the plugin is started
    /// </summary>
    public void UpdateClientStats()
    {
        ClientState_Login(null, null);
    }

    /// <summary>
    /// Triggered by ClientState_Login
    /// Send the Name and WorldId to the LA
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ClientState_Login(object? sender, EventArgs? e)
    {
        if (ClientState?.LocalPlayer != null)
        {
            var name = ClientState.LocalPlayer.Name.TextValue;
            if (ClientState.LocalPlayer.HomeWorld.GameData != null)
            {
                var homeWorld = ClientState.LocalPlayer.HomeWorld.GameData.RowId;

                if (Pipe.Client != null && Pipe.Client.IsConnected)
                {
                    Pipe.Client.WriteAsync(new PayloadMessage
                    {
                        MsgType = MessageType.NameAndHomeWorld,
                        Message = Environment.ProcessId + ":" + name + ":" + homeWorld
                    });
                }
            }
        }
    }

    private static void ClientState_Logout(object? sender, EventArgs e)
    {
    }


    private List<PlayerInfo> _getInfoFromNormalParty()
    {
        // Generates a list of playerInfo objects from the game's memory
        // assuming the party is a normal party (light/full/etc.)
        var output = new List<PlayerInfo>();
        if (PartyList != null)
        {
            var pCount = PartyList.Length;

            for (var i = 0; i < pCount; i++)
            {
                var memberPtr = PartyList.GetPartyMemberAddress(i);
                var member = PartyList.CreatePartyMemberReference(memberPtr);
                var tempName = member?.Name.ToString();
                const string tempWorld = "";
                const string tempRegion = "";
                output.Add(new PlayerInfo(tempName, tempWorld, tempRegion));
            }
        }

        return output;
    }
}