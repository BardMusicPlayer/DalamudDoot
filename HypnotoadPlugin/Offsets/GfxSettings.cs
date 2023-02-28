/*
 * Copyright(c) 2022 Ori @MidiBard2
 */

using System.Runtime.InteropServices;
using Dalamud.Game.Gui.Toast;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace HypnotoadPlugin.Offsets;

internal static class GfxSettings
{

    internal class AgentConfigSystem : AgentInterface
    {
        private static readonly unsafe ConfigModule* ConfigModule = Framework.Instance()->UIModule->GetConfigModule();
        public AgentConfigSystem(AgentInterface agentInterface) : base(agentInterface.Pointer, agentInterface.Id) { }

        public unsafe void ApplyGraphicSettings()
        {
            void OnToast(ref SeString message, ref ToastOptions options, ref bool handled)
            {
                if (Api.ToastGui != null) Api.ToastGui.Toast -= OnToast;
            }

            var refreshConfigGraphicState = (delegate* unmanaged<nint, long>)Offsets.ApplyGraphicConfigsFunc;
            var _ = refreshConfigGraphicState(Pointer);
            if (Api.ToastGui != null) Api.ToastGui.Toast += OnToast;
        }

        private static unsafe AtkValue* GetOptionValue(ConfigOption option) => ConfigModule->GetValue(option);
        public static unsafe void SetOptionValue(ConfigOption option, int value) => ConfigModule->SetOption(option, value);
        public static unsafe void ToggleBoolOptionValue(ConfigOption option) => ConfigModule->SetOption(option, GetOptionValue(option)->Byte == 0 ? 1 : 0);

        private static int _fpsInActive;
        private static int _originalObjQuantity;
        private static int _waterWetDx11;
        private static int _occlusionCullingDx11;
        private static int _lodTypeDx11;
        private static int _reflectionTypeDx11;
        private static int _antiAliasingDx11;
        private static int _translucentQualityDx11;
        private static int _grassQualityDx11;
        private static int _parallaxOcclusionDx11;
        private static int _tessellationDx11;
        private static int _glareRepresentationDx11;
        private static int _mapResolutionDx11;
        private static int _shadowVisibilityTypeSelfDx11;
        private static int _shadowVisibilityTypePartyDx11;
        private static int _shadowVisibilityTypeOtherDx11;
        private static int _shadowVisibilityTypeEnemyDx11;
        private static int _shadowLodDx11;
        private static int _shadowTextureSizeTypeDx11;
        private static int _shadowCascadeCountTypeDx11;
        private static int _shadowSoftShadowTypeDx11;
        private static int _textureFilterQualityDx11;
        private static int _textureAnisotropicQualityDx11;
        private static int _physicsTypeSelfDx11;
        private static int _physicsTypePartyDx11;
        private static int _physicsTypeOtherDx11;
        private static int _physicsTypeEnemyDx11;
        private static int _radialBlurDx11;
        private static int _ssaoDx11;
        private static int _glareDx11;
        private static int _distortionWaterDx11;

        public static bool CheckLowSettings()
        {
            return _originalObjQuantity == 4      &&
                   _waterWetDx11 == 0            &&
                   _occlusionCullingDx11 == 1    &&
                   _reflectionTypeDx11 == 3      &&
                   _grassQualityDx11 == 3        &&
                   _ssaoDx11 == 4;
        }

        public static unsafe void GetObjQuantity()
        {
            _fpsInActive                   = ConfigModule->GetIntValue(ConfigOption.FPSInActive);
            _originalObjQuantity           = ConfigModule->GetIntValue(ConfigOption.DisplayObjectLimitType);
            _waterWetDx11                  = ConfigModule->GetIntValue(ConfigOption.WaterWet_DX11);
            _occlusionCullingDx11          = ConfigModule->GetIntValue(ConfigOption.OcclusionCulling_DX11);
            _lodTypeDx11                   = ConfigModule->GetIntValue(ConfigOption.LodType_DX11);
            _reflectionTypeDx11            = ConfigModule->GetIntValue(ConfigOption.ReflectionType_DX11);
            _antiAliasingDx11              = ConfigModule->GetIntValue(ConfigOption.AntiAliasing_DX11);
            _translucentQualityDx11        = ConfigModule->GetIntValue(ConfigOption.TranslucentQuality_DX11);
            _grassQualityDx11              = ConfigModule->GetIntValue(ConfigOption.GrassQuality_DX11);
            _parallaxOcclusionDx11         = ConfigModule->GetIntValue(ConfigOption.ParallaxOcclusion_DX11);
            _tessellationDx11              = ConfigModule->GetIntValue(ConfigOption.Tessellation_DX11);
            _glareRepresentationDx11       = ConfigModule->GetIntValue(ConfigOption.GlareRepresentation_DX11);
            _mapResolutionDx11             = ConfigModule->GetIntValue(ConfigOption.MapResolution_DX11);
            _shadowVisibilityTypeSelfDx11  = ConfigModule->GetIntValue(ConfigOption.ShadowVisibilityTypeSelf_DX11);
            _shadowVisibilityTypePartyDx11 = ConfigModule->GetIntValue(ConfigOption.ShadowVisibilityTypeParty_DX11);
            _shadowVisibilityTypeOtherDx11 = ConfigModule->GetIntValue(ConfigOption.ShadowVisibilityTypeOther_DX11);
            _shadowVisibilityTypeEnemyDx11 = ConfigModule->GetIntValue(ConfigOption.ShadowVisibilityTypeEnemy_DX11);
            _shadowLodDx11                 = ConfigModule->GetIntValue(ConfigOption.ShadowLOD_DX11);
            _shadowTextureSizeTypeDx11     = ConfigModule->GetIntValue(ConfigOption.ShadowTextureSizeType_DX11);
            _shadowCascadeCountTypeDx11    = ConfigModule->GetIntValue(ConfigOption.ShadowCascadeCountType_DX11);
            _shadowSoftShadowTypeDx11      = ConfigModule->GetIntValue(ConfigOption.ShadowSoftShadowType_DX11);
            _textureFilterQualityDx11      = ConfigModule->GetIntValue(ConfigOption.TextureFilterQuality_DX11);
            _textureAnisotropicQualityDx11 = ConfigModule->GetIntValue(ConfigOption.TextureAnisotropicQuality_DX11);
            _physicsTypeSelfDx11           = ConfigModule->GetIntValue(ConfigOption.PhysicsTypeSelf_DX11);
            _physicsTypePartyDx11          = ConfigModule->GetIntValue(ConfigOption.PhysicsTypeParty_DX11);
            _physicsTypeOtherDx11          = ConfigModule->GetIntValue(ConfigOption.PhysicsTypeOther_DX11);
            _physicsTypeEnemyDx11          = ConfigModule->GetIntValue(ConfigOption.PhysicsTypeEnemy_DX11);
            _radialBlurDx11                = ConfigModule->GetIntValue(ConfigOption.RadialBlur_DX11);
            _ssaoDx11                      = ConfigModule->GetIntValue(ConfigOption.SSAO_DX11);
            _glareDx11                     = ConfigModule->GetIntValue(ConfigOption.Glare_DX11);
            _distortionWaterDx11           = ConfigModule->GetIntValue(ConfigOption.DistortionWater_DX11);
        }

        public static unsafe void SetMinimalObjQuantity()
        {
            ConfigModule->SetOption(ConfigOption.FPSInActive, 0);
            ConfigModule->SetOption(ConfigOption.DisplayObjectLimitType, 4);

            ConfigModule->SetOption(ConfigOption.WaterWet_DX11, 0);
            ConfigModule->SetOption(ConfigOption.OcclusionCulling_DX11, 1);
            ConfigModule->SetOption(ConfigOption.LodType_DX11, 1);
            ConfigModule->SetOption(ConfigOption.ReflectionType_DX11, 3);
            ConfigModule->SetOption(ConfigOption.AntiAliasing_DX11, 1);
            ConfigModule->SetOption(ConfigOption.TranslucentQuality_DX11, 1);
            ConfigModule->SetOption(ConfigOption.GrassQuality_DX11, 3);
            ConfigModule->SetOption(ConfigOption.ParallaxOcclusion_DX11, 1);
            ConfigModule->SetOption(ConfigOption.Tessellation_DX11, 1);
            ConfigModule->SetOption(ConfigOption.GlareRepresentation_DX11, 1);
            ConfigModule->SetOption(ConfigOption.MapResolution_DX11, 2);
            ConfigModule->SetOption(ConfigOption.ShadowVisibilityTypeSelf_DX11, 1);
            ConfigModule->SetOption(ConfigOption.ShadowVisibilityTypeParty_DX11, 1);
            ConfigModule->SetOption(ConfigOption.ShadowVisibilityTypeOther_DX11, 1);
            ConfigModule->SetOption(ConfigOption.ShadowVisibilityTypeEnemy_DX11, 1);
            ConfigModule->SetOption(ConfigOption.ShadowLOD_DX11, 1);
            ConfigModule->SetOption(ConfigOption.ShadowTextureSizeType_DX11, 2);
            ConfigModule->SetOption(ConfigOption.ShadowCascadeCountType_DX11, 2);
            ConfigModule->SetOption(ConfigOption.ShadowSoftShadowType_DX11, 1);
            ConfigModule->SetOption(ConfigOption.TextureFilterQuality_DX11, 2);
            ConfigModule->SetOption(ConfigOption.TextureAnisotropicQuality_DX11, 2);
            ConfigModule->SetOption(ConfigOption.PhysicsTypeSelf_DX11, 2);
            ConfigModule->SetOption(ConfigOption.PhysicsTypeParty_DX11, 2);
            ConfigModule->SetOption(ConfigOption.PhysicsTypeOther_DX11, 2);
            ConfigModule->SetOption(ConfigOption.PhysicsTypeEnemy_DX11, 2);
            ConfigModule->SetOption(ConfigOption.RadialBlur_DX11, 0);
            ConfigModule->SetOption(ConfigOption.SSAO_DX11, 4);
            ConfigModule->SetOption(ConfigOption.Glare_DX11, 2);
            ConfigModule->SetOption(ConfigOption.DistortionWater_DX11, 2);

        }

        public static unsafe void RestoreObjQuantity()
        {
            ConfigModule->SetOption(ConfigOption.FPSInActive,                      _fpsInActive);
            ConfigModule->SetOption(ConfigOption.DisplayObjectLimitType,           _originalObjQuantity);
            ConfigModule->SetOption(ConfigOption.WaterWet_DX11,                    _waterWetDx11);
            ConfigModule->SetOption(ConfigOption.OcclusionCulling_DX11,            _occlusionCullingDx11);
            ConfigModule->SetOption(ConfigOption.LodType_DX11,                     _lodTypeDx11);
            ConfigModule->SetOption(ConfigOption.ReflectionType_DX11,              _reflectionTypeDx11);
            ConfigModule->SetOption(ConfigOption.AntiAliasing_DX11,                _antiAliasingDx11);
            ConfigModule->SetOption(ConfigOption.TranslucentQuality_DX11,          _translucentQualityDx11);
            ConfigModule->SetOption(ConfigOption.GrassQuality_DX11,                _grassQualityDx11);
            ConfigModule->SetOption(ConfigOption.ParallaxOcclusion_DX11,           _parallaxOcclusionDx11);
            ConfigModule->SetOption(ConfigOption.Tessellation_DX11,                _tessellationDx11);
            ConfigModule->SetOption(ConfigOption.GlareRepresentation_DX11,         _glareRepresentationDx11);
            ConfigModule->SetOption(ConfigOption.MapResolution_DX11,               _mapResolutionDx11);
            ConfigModule->SetOption(ConfigOption.ShadowVisibilityTypeSelf_DX11,    _shadowVisibilityTypeSelfDx11);
            ConfigModule->SetOption(ConfigOption.ShadowVisibilityTypeParty_DX11,   _shadowVisibilityTypePartyDx11);
            ConfigModule->SetOption(ConfigOption.ShadowVisibilityTypeOther_DX11,   _shadowVisibilityTypeOtherDx11);
            ConfigModule->SetOption(ConfigOption.ShadowVisibilityTypeEnemy_DX11,   _shadowVisibilityTypeEnemyDx11);
            ConfigModule->SetOption(ConfigOption.ShadowLOD_DX11,                   _shadowLodDx11);
            ConfigModule->SetOption(ConfigOption.ShadowTextureSizeType_DX11,       _shadowTextureSizeTypeDx11);
            ConfigModule->SetOption(ConfigOption.ShadowCascadeCountType_DX11,      _shadowCascadeCountTypeDx11);
            ConfigModule->SetOption(ConfigOption.ShadowSoftShadowType_DX11,        _shadowSoftShadowTypeDx11);
            ConfigModule->SetOption(ConfigOption.TextureFilterQuality_DX11,        _textureFilterQualityDx11);
            ConfigModule->SetOption(ConfigOption.TextureAnisotropicQuality_DX11,   _textureAnisotropicQualityDx11);
            ConfigModule->SetOption(ConfigOption.PhysicsTypeSelf_DX11,             _physicsTypeSelfDx11);
            ConfigModule->SetOption(ConfigOption.PhysicsTypeParty_DX11,            _physicsTypePartyDx11);
            ConfigModule->SetOption(ConfigOption.PhysicsTypeOther_DX11,            _physicsTypeOtherDx11);
            ConfigModule->SetOption(ConfigOption.PhysicsTypeEnemy_DX11,            _physicsTypeEnemyDx11);
            ConfigModule->SetOption(ConfigOption.RadialBlur_DX11,                  _radialBlurDx11);
            ConfigModule->SetOption(ConfigOption.SSAO_DX11,                        _ssaoDx11);
            ConfigModule->SetOption(ConfigOption.Glare_DX11,                       _glareDx11);
            ConfigModule->SetOption(ConfigOption.DistortionWater_DX11,             _distortionWaterDx11);


        }

    }
}

public unsafe class AgentInterface
{
    public nint Pointer { get; }
    public nint VTable { get; }
    public int Id { get; }
    public FFXIVClientStructs.FFXIV.Component.GUI.AgentInterface* Struct => (FFXIVClientStructs.FFXIV.Component.GUI.AgentInterface*)Pointer;

    public AgentInterface(nint pointer, int id)
    {
        Pointer = pointer;
        Id      = id;
        VTable  = Marshal.ReadIntPtr(Pointer);
    }

    public override string ToString()
    {
        return $"{Id} {(long)Pointer:X} {(long)VTable:X}";
    }
}

internal unsafe class AgentManager
{
    private List<AgentInterface> AgentTable { get; } = new(400);

    private AgentManager()
    {
        try
        {
            var instance = Framework.Instance();
            var agentModule = instance->UIModule->GetAgentModule();
            var i = 0;
            foreach (var pointer in agentModule->AgentsSpan)
                AgentTable.Add(new AgentInterface((nint)pointer.Value, i++));
        }
        catch (Exception e)
        {
            PluginLog.Error(e.ToString());
        }
    }

    public static AgentManager Instance { get; } = new();

    internal AgentInterface FindAgentInterfaceById(int id) => AgentTable[id];

    internal AgentInterface FindAgentInterfaceByVtable(nint vtbl) => AgentTable.First(i => i.VTable == vtbl);
}