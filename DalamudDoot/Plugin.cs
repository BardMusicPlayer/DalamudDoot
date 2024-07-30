/*
 * Copyright(c) 2023 GiR-Zippo, Meowchestra
 * Licensed under the GPL v3 license. See https://github.com/GiR-Zippo/LightAmp/blob/main/LICENSE for full license information.
 */

using Dalamud.Game;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using DalamudDoot.Offsets;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using static DalamudDoot.Offsets.GameSettings;

namespace DalamudDoot;

public class DalamudDoot : IDalamudPlugin
{
    //public static XivCommonBase CBase;
    public string Name => "DalamudDoot";

    private const string CommandName = "/doot";

    private IDalamudPluginInterface PluginInterface { get; }
    private ICommandManager CommandManager { get; }
    private Configuration Configuration { get; }
    private PluginUi PluginUi { get; }
    internal static AgentPerformance AgentPerformance { get; private set; }
    internal static EnsembleManager EnsembleManager { get; set; }
    public Api api { get; set; }

    [PluginService]
    private static ISigScanner SigScanner { get; set; }

    public DalamudDoot(IDalamudPluginInterface pluginInterface, IDataManager data, ICommandManager commandManager, IClientState clientState, IPartyList partyList)
    {
        api = pluginInterface.Create<Api>();
        PluginInterface = pluginInterface;
        CommandManager = commandManager;

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(PluginInterface);
        OffsetManager.Setup(SigScanner);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Open the DalamudDoot settings menu."
        });

        AgentPerformance = new AgentPerformance(AgentId.PerformanceMode);
        EnsembleManager = new EnsembleManager();

        Collector.Instance.Initialize(data, clientState, partyList);

        AgentConfigSystem.GetSettings();

        //NetworkReader.Initialize();

        // you might normally want to embed resources and load them from the manifest stream
        PluginUi = new PluginUi(Configuration);

        PluginInterface.UiBuilder.Draw += DrawUi;
        PluginInterface.UiBuilder.OpenMainUi += DrawConfigUi;
        PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUi;
    }

    public void Dispose()
    {
        PluginInterface.UiBuilder.Draw -= DrawUi;
        PluginInterface.UiBuilder.OpenMainUi -= DrawConfigUi;
        PluginInterface.UiBuilder.OpenConfigUi -= DrawConfigUi;

        //NetworkReader.Dispose();
        AgentConfigSystem.RestoreSettings();
        EnsembleManager?.Dispose();
        Collector.Instance.Dispose();

        PluginUi.Dispose();
        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        // in response to the slash command, just display our main ui
        PluginUi.Visible = true;
    }

    private void DrawUi()
    {
        PluginUi.Draw();
    }

    private void DrawConfigUi()
    {
        PluginUi.Visible = true;
    }
}