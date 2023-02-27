using Dalamud.Game;
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

    [PluginService] 
    private static SigScanner? SigScanner { get; set; }

    public Hypnotoad(DalamudPluginInterface pluginInterface, CommandManager commandManager)
    {
        Api.Initialize(this, pluginInterface);
        PluginInterface = pluginInterface;
        CommandManager  = commandManager;

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(PluginInterface);
        OffsetManager.Setup(SigScanner);

        // you might normally want to embed resources and load them from the manifest stream
        PluginUi = new PluginUi(Configuration);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Open the Hypnotoad settings menu."
        });

        AgentConfigSystem = new AgentConfigSystem(AgentManager.Instance.FindAgentInterfaceByVtable(Offsets.Offsets.AgentConfigSystem));
        AgentPerformance  = new AgentPerformance(AgentManager.Instance.FindAgentInterfaceByVtable(Offsets.Offsets.AgentPerformance));
        AgentConfigSystem.GetObjQuantity();

        PluginInterface.UiBuilder.Draw         += DrawUi;
        PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUi;
        NetworkReader.Initialize();
    }

    public void Dispose()
    {
        NetworkReader.Dispose();
        AgentConfigSystem.RestoreObjQuantity();
        AgentConfigSystem?.ApplyGraphicSettings();

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