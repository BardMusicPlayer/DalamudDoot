using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using HypnotoadPlugin.Offsets;
using static HypnotoadPlugin.Offsets.GfxSettings;

namespace HypnotoadPlugin;

public class Hypnotoad : IDalamudPlugin
{
    //public static XivCommonBase CBase;
    public string Name => "Hypnotoad";

    private const string CommandName = "/hypnotoad";

    private DalamudPluginInterface PluginInterface { get; }
    private CommandManager CommandManager { get; }
    private Configuration Configuration { get; }
    private PluginUi PluginUi { get; }
    internal static AgentConfigSystem? AgentConfigSystem { get; private set; }
    internal static AgentPerformance? AgentPerformance { get; private set; }
    internal static EnsembleManager? EnsembleManager { get; set; }

    [PluginService] 
    private static SigScanner? SigScanner { get; set; }

    public Hypnotoad(DalamudPluginInterface pluginInterface, DataManager? data, CommandManager commandManager, ClientState? clientState, PartyList? partyList)
    {
        Api.Initialize(this, pluginInterface);
        PluginInterface = pluginInterface;
        CommandManager  = commandManager;

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(PluginInterface);
        OffsetManager.Setup(SigScanner);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Open the Hypnotoad settings menu."
        });

        AgentConfigSystem = new AgentConfigSystem(AgentManager.Instance.FindAgentInterfaceByVtable(Offsets.Offsets.AgentConfigSystem));
        AgentPerformance  = new AgentPerformance(AgentManager.Instance.FindAgentInterfaceByVtable(Offsets.Offsets.AgentPerformance));
        EnsembleManager   = new EnsembleManager();

        Collector.Instance.Initialize(data, clientState, partyList);

        AgentConfigSystem.GetObjQuantity();

        //NetworkReader.Initialize();

        // you might normally want to embed resources and load them from the manifest stream
        PluginUi = new PluginUi(Configuration);

        PluginInterface.UiBuilder.Draw         += DrawUi;
        PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUi;
    }

    public void Dispose()
    {
        //NetworkReader.Dispose();
        AgentConfigSystem.RestoreObjQuantity();
        AgentConfigSystem?.ApplyGraphicSettings();
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