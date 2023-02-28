/*
 * Copyright(c) 2022 Ori @MidiBard2
 */

using System.Diagnostics;
using System.Runtime.InteropServices;
using Dalamud.Logging;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace HypnotoadPlugin.Offsets;

public class PerformActions
{
    private delegate void DoPerformActionDelegate(nint performInfoPtr, uint instrumentId, int a3 = 0);
    private static DoPerformActionDelegate DoPerformAction { get; } = Marshal.GetDelegateForFunctionPointer<DoPerformActionDelegate>(Offsets.DoPerformAction);
    public static void PerformAction(uint instrumentId)
    {
        PluginLog.Information($"[PerformAction] instrumentId: {instrumentId}");
        DoPerformAction(Offsets.PerformanceStructPtr, instrumentId);
    }

    private PerformActions() { }
    private static unsafe nint GetWindowByName(string s) => (nint)AtkStage.GetSingleton()->RaptureAtkUnitManager->GetAddonByName(s);
    public static void Init() => SignatureHelper.Initialise(new PerformActions());

    private static void SendAction(nint ptr, params ulong[] param)
    {
        if (param.Length % 2 != 0) 
            throw new ArgumentException("The parameter length must be an integer multiple of 2.");
        if (ptr == nint.Zero) 
            throw new ArgumentException("input pointer is null");

        var pairCount = param.Length / 2;
        unsafe
        {
            fixed (ulong* u = param)
            {
                AtkUnitBase.MemberFunctionPointers.FireCallback((AtkUnitBase*)ptr, pairCount, (AtkValue*)u, (void*)1);
            }
        }
    }

    private static bool SendAction(string name, params ulong[] param)
    {
        var ptr = GetWindowByName(name);
        if (ptr == nint.Zero) return false;
        SendAction(ptr, param);
        return true;
    }

    private static bool PressKey(int keyNumber, ref int offset, ref int octave)
    {
        if (!TargetWindowPtr(out var miniMode, out var targetWindowPtr)) return false;
        offset = 0;
        octave = 0;

        if (miniMode)
        {
            keyNumber = ConvertMiniKeyNumber(keyNumber, ref offset, ref octave);
        }

        SendAction(targetWindowPtr, 3, 1, 4, (ulong)keyNumber);

        return true;

    }

    private static bool ReleaseKey(int keyNumber)
    {
        if (!TargetWindowPtr(out var miniMode, out var targetWindowPtr)) return false;
        if (miniMode) keyNumber = ConvertMiniKeyNumber(keyNumber);

        SendAction(targetWindowPtr, 3, 2, 4, (ulong)keyNumber);

        return true;

    }

    private static int ConvertMiniKeyNumber(int keyNumber)
    {
        keyNumber -= 12;
        switch (keyNumber)
        {
            case < 0:
                keyNumber += 12;
                break;
            case > 12:
                keyNumber -= 12;
                break;
        }

        return keyNumber;
    }

    private static int ConvertMiniKeyNumber(int keyNumber, ref int offset, ref int octave)
    {
        keyNumber -= 12;
        switch (keyNumber)
        {
            case < 0:
                keyNumber += 12;
                offset    =  -12;
                octave    =  -1;
                break;
            case > 12:
                keyNumber -= 12;
                offset    =  12;
                octave    =  1;
                break;
        }

        return keyNumber;
    }

    private static bool TargetWindowPtr(out bool miniMode, out nint targetWindowPtr)
    {
        targetWindowPtr = GetWindowByName("PerformanceMode");
        if (targetWindowPtr != nint.Zero)
        {
            miniMode = true;
            return true;
        }

        targetWindowPtr = GetWindowByName("PerformanceModeWide");
        if (targetWindowPtr != nint.Zero)
        {
            miniMode = false;
            return true;
        }

        miniMode = false;
        return false;
    }

    public static bool GuitarSwitchTone(int tone)
    {
        var ptr = GetWindowByName("PerformanceToneChange");
        if (ptr == nint.Zero) return false;

        SendAction(ptr, 3, 0, 3, (ulong)tone);
        return true;
    }

    public static bool BeginReadyCheck() => SendAction("PerformanceMetronome", 3, 2, 2, 0);
    public static bool ConfirmBeginReadyCheck() => SendAction("PerformanceReadyCheck", 3, 2);
    public static bool ConfirmReceiveReadyCheck() => SendAction("PerformanceReadyCheckReceive", 3, 2);

    public static string MainModuleRva(nint ptr)
    {
        var modules = Process.GetCurrentProcess().Modules;
        List<ProcessModule> mh = new();
        for (var i = 0; i < modules.Count; i++)
            mh.Add(modules[i]);

        mh.Sort((x, y) => x.BaseAddress > (long)y.BaseAddress ? -1 : 1);
        foreach (var module in mh.Where(module => module.BaseAddress <= (long)ptr))
        {
            return $"[{module.ModuleName}+0x{ptr - (long)module.BaseAddress:X}]";
        }
        return $"[0x{(long)ptr:X}]";
    }

    public static unsafe void PlayNote(int noteNum, bool on)
    {
        if (on)
        {
            if (Hypnotoad.AgentPerformance != null && Hypnotoad.AgentPerformance.NoteNumber - 39 == noteNum)
                if (ReleaseKey(noteNum))
                    Hypnotoad.AgentPerformance.Struct->CurrentPressingNote = -100;

            if (Hypnotoad.AgentPerformance != null && PressKey(noteNum, ref Hypnotoad.AgentPerformance.Struct->NoteOffset, ref Hypnotoad.AgentPerformance.Struct->OctaveOffset))
            {
                Hypnotoad.AgentPerformance.Struct->CurrentPressingNote = noteNum + 39;
            }

        }
        else
        {
            if (Hypnotoad.AgentPerformance != null && Hypnotoad.AgentPerformance.Struct->CurrentPressingNote - 39 != noteNum)
                return;

            if (ReleaseKey(noteNum))
            {
                if (Hypnotoad.AgentPerformance != null) Hypnotoad.AgentPerformance.Struct->CurrentPressingNote = -100;
            }
        }
    }

}