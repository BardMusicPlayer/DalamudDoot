﻿/*
 * Copyright(c) 2023 GiR-Zippo, akira0245 @MidiBard, Ori @MidiBard2
 * Licensed under the GPL v3 license. See https://github.com/GiR-Zippo/LightAmp/blob/main/LICENSE for full license information.
 */

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;

namespace DalamudDoot.Offsets;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]

public static partial class Chat
{
    private static class Signatures
    {
        internal const string SendChat = "48 89 5C 24 ?? 57 48 83 EC 20 48 8B FA 48 8B D9 45 84 C9";
        internal const string SanitiseString = "E8 ?? ?? ?? ?? 48 8D 4C 24 ?? 0F B6 F0 E8 ?? ?? ?? ?? 48 8D 4D C0";
    }
}

public static class Offsets
{
    [StaticAddress("48 8B C2 0F B6 15 ?? ?? ?? ?? F6 C2 01")]
    public static nint PerformanceStructPtr { get; private set; }

    [Function("48 89 6C 24 10 48 89 74 24 18 57 48 83 EC ?? 48 83 3D ?? ?? ?? ?? ?? 41 8B E8")]
    public static nint DoPerformAction { get; private set; }

    [Function("40 53 48 83 EC 20 48 8B D9 48 83 C1 78 E8 ? ? ? ? 48 8D 8B ? ? ? ? E8 ? ? ? ? 48 8D 53 20")]
    public static IntPtr NetworkEnsembleStart { get; private set; }
}

public sealed unsafe class AgentPerformance : AgentInterface
{
    public AgentPerformance(AgentId id) : base(id) { }

    public static AgentPerformance Instance => DalamudDoot.AgentPerformance;
    public new AgentPerformanceStruct* Struct => (AgentPerformanceStruct*)Pointer;

    [StructLayout(LayoutKind.Explicit)]
    public struct AgentPerformanceStruct
    {
        [FieldOffset(0)] public FFXIVClientStructs.FFXIV.Client.UI.Agent.AgentInterface AgentInterface;
        [FieldOffset(0x20)] public byte InPerformanceMode;
        [FieldOffset(0x1F)] public byte Instrument;
        [FieldOffset(0x38)] public long PerformanceTimer1;
        [FieldOffset(0x40)] public long PerformanceTimer2;
        [FieldOffset(0x5C)] public int NoteOffset;
        [FieldOffset(0x60)] public int CurrentPressingNote;
        [FieldOffset(0xFC)] public int OctaveOffset;
        [FieldOffset(0x1B0)] public int GroupTone;
    }

    internal int CurrentGroupTone => Struct->GroupTone;
    internal bool InPerformanceMode => Struct->InPerformanceMode != 0;
    internal bool NotePressed => Struct->CurrentPressingNote != -100;
    internal int NoteNumber => Struct->CurrentPressingNote;
    internal long PerformanceTimer1 => Struct->PerformanceTimer1;
    internal long PerformanceTimer2 => Struct->PerformanceTimer2;
    internal byte Instrument => Struct->Instrument;
}

internal class EnsembleManager : IDisposable
{
    private delegate long SubNetworkEnsemble(IntPtr a1, IntPtr a2);
    private readonly Hook<SubNetworkEnsemble> _networkEnsembleHook;
    internal EnsembleManager()
    {
        //Get the ensemble start
        _networkEnsembleHook = Api.GameInteropProvider?.HookFromAddress<SubNetworkEnsemble>(Offsets.NetworkEnsembleStart, (a1, a2) =>
        {
            //and pipe it
            if (Pipe.Client != null && Pipe.Client.IsConnected)
            {
                Pipe.Client.WriteAsync(new PayloadMessage
                {
                    MsgType = MessageType.StartEnsemble,
                    Message = Environment.ProcessId + ":1"
                });
            }

            return _networkEnsembleHook != null ? _networkEnsembleHook.Original(a1, a2) : 0;
        });
        _networkEnsembleHook?.Enable();
    }

    public void Dispose()
    {
        _networkEnsembleHook?.Dispose();
    }
}