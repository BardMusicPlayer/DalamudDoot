/*
 * Copyright(c) 2023 GiR-Zippo, Ori@MidiBard2
 * Licensed under the GPL v3 license. See https://github.com/GiR-Zippo/LightAmp/blob/main/LICENSE for full license information.
 */

using System.Runtime.InteropServices;
using Dalamud.Game.Config;
using Dalamud.Game.Gui.Toast;
using Dalamud.Game.Text.SeStringHandling;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;

namespace DalamudDoot.Offsets;

internal static class GameSettings
{
    /// <summary>
    /// The agent for the client config
    /// </summary>
    internal class AgentConfigSystem
    {
        #region Get/Restore config
        private static uint _fpsInActive;
        private static uint _originalObjQuantity;
        private static uint _waterWetDx11;
        private static uint _occlusionCullingDx11;
        private static uint _lodTypeDx11;
        private static uint _reflectionTypeDx11;
        private static uint _antiAliasingDx11;
        private static uint _translucentQualityDx11;
        private static uint _grassQualityDx11;
        private static uint _parallaxOcclusionDx11;
        private static uint _tessellationDx11;
        private static uint _glareRepresentationDx11;
        private static uint _mapResolutionDx11;
        private static uint _shadowVisibilityTypeSelfDx11;
        private static uint _shadowVisibilityTypePartyDx11;
        private static uint _shadowVisibilityTypeOtherDx11;
        private static uint _shadowVisibilityTypeEnemyDx11;
        private static uint _shadowLodDx11;
        private static uint _shadowTextureSizeTypeDx11;
        private static uint _shadowCascadeCountTypeDx11;
        private static uint _shadowSoftShadowTypeDx11;
        private static uint _textureFilterQualityDx11;
        private static uint _textureAnisotropicQualityDx11;
        private static uint _physicsTypeSelfDx11;
        private static uint _physicsTypePartyDx11;
        private static uint _physicsTypeOtherDx11;
        private static uint _physicsTypeEnemyDx11;
        private static uint _radialBlurDx11;
        private static uint _ssaoDx11;
        private static uint _glareDx11;
        private static uint _distortionWaterDx11;
        private static uint _soundEnabled;

        /// <summary>
        /// Get the gfx settings and save them
        /// </summary>
        public static unsafe void GetSettings()
        {
            var configEntry = Framework.Instance()->SystemConfig.SystemConfigBase.ConfigBase.ConfigEntry;

            _fpsInActive = configEntry[(int)ConfigOption.FPSInActive].Value.UInt;
            _originalObjQuantity = configEntry[(int)ConfigOption.DisplayObjectLimitType].Value.UInt;
            _waterWetDx11 = configEntry[(int)ConfigOption.WaterWet_DX11].Value.UInt;
            _occlusionCullingDx11 = configEntry[(int)ConfigOption.OcclusionCulling_DX11].Value.UInt;
            _lodTypeDx11 = configEntry[(int)ConfigOption.LodType_DX11].Value.UInt;
            _reflectionTypeDx11 = configEntry[(int)ConfigOption.ReflectionType_DX11].Value.UInt;
            _antiAliasingDx11 = configEntry[(int)ConfigOption.AntiAliasing_DX11].Value.UInt;
            _translucentQualityDx11 = configEntry[(int)ConfigOption.TranslucentQuality_DX11].Value.UInt;
            _grassQualityDx11 = configEntry[(int)ConfigOption.GrassQuality_DX11].Value.UInt;
            _parallaxOcclusionDx11 = configEntry[(int)ConfigOption.ParallaxOcclusion_DX11].Value.UInt;
            _tessellationDx11 = configEntry[(int)ConfigOption.Tessellation_DX11].Value.UInt;
            _glareRepresentationDx11 = configEntry[(int)ConfigOption.GlareRepresentation_DX11].Value.UInt;
            _mapResolutionDx11 = configEntry[(int)ConfigOption.MapResolution_DX11].Value.UInt;
            _shadowVisibilityTypeSelfDx11 = configEntry[(int)ConfigOption.ShadowVisibilityTypeSelf_DX11].Value.UInt;
            _shadowVisibilityTypePartyDx11 = configEntry[(int)ConfigOption.ShadowVisibilityTypeParty_DX11].Value.UInt;
            _shadowVisibilityTypeOtherDx11 = configEntry[(int)ConfigOption.ShadowVisibilityTypeOther_DX11].Value.UInt;
            _shadowVisibilityTypeEnemyDx11 = configEntry[(int)ConfigOption.ShadowVisibilityTypeEnemy_DX11].Value.UInt;
            _shadowLodDx11 = configEntry[(int)ConfigOption.ShadowLOD_DX11].Value.UInt;
            _shadowTextureSizeTypeDx11 = configEntry[(int)ConfigOption.ShadowTextureSizeType_DX11].Value.UInt;
            _shadowCascadeCountTypeDx11 = configEntry[(int)ConfigOption.ShadowCascadeCountType_DX11].Value.UInt;
            _shadowSoftShadowTypeDx11 = configEntry[(int)ConfigOption.ShadowSoftShadowType_DX11].Value.UInt;
            _textureFilterQualityDx11 = configEntry[(int)ConfigOption.TextureFilterQuality_DX11].Value.UInt;
            _textureAnisotropicQualityDx11 = configEntry[(int)ConfigOption.TextureAnisotropicQuality_DX11].Value.UInt;
            _physicsTypeSelfDx11 = configEntry[(int)ConfigOption.PhysicsTypeSelf_DX11].Value.UInt;
            _physicsTypePartyDx11 = configEntry[(int)ConfigOption.PhysicsTypeParty_DX11].Value.UInt;
            _physicsTypeOtherDx11 = configEntry[(int)ConfigOption.PhysicsTypeOther_DX11].Value.UInt;
            _physicsTypeEnemyDx11 = configEntry[(int)ConfigOption.PhysicsTypeEnemy_DX11].Value.UInt;
            _radialBlurDx11 = configEntry[(int)ConfigOption.RadialBlur_DX11].Value.UInt;
            _ssaoDx11 = configEntry[(int)ConfigOption.SSAO_DX11].Value.UInt;
            _glareDx11 = configEntry[(int)ConfigOption.Glare_DX11].Value.UInt;
            _distortionWaterDx11 = configEntry[(int)ConfigOption.DistortionWater_DX11].Value.UInt;
            _soundEnabled = configEntry[(int)ConfigOption.IsSndMaster].Value.UInt;
        }

        /// <summary>
        /// Restore the GFX settings
        /// </summary>
        public static unsafe void RestoreSettings()
        {
            var configEntry = Framework.Instance()->SystemConfig.SystemConfigBase.ConfigBase.ConfigEntry;

            configEntry[(int)ConfigOption.FPSInActive].SetValueUInt(_fpsInActive);
            configEntry[(int)ConfigOption.DisplayObjectLimitType].SetValueUInt(_originalObjQuantity);
            configEntry[(int)ConfigOption.WaterWet_DX11].SetValueUInt(_waterWetDx11);
            configEntry[(int)ConfigOption.OcclusionCulling_DX11].SetValueUInt(_occlusionCullingDx11);
            configEntry[(int)ConfigOption.LodType_DX11].SetValueUInt(_lodTypeDx11);
            configEntry[(int)ConfigOption.ReflectionType_DX11].SetValueUInt(_reflectionTypeDx11);
            configEntry[(int)ConfigOption.AntiAliasing_DX11].SetValueUInt(_antiAliasingDx11);
            configEntry[(int)ConfigOption.TranslucentQuality_DX11].SetValueUInt(_translucentQualityDx11);
            configEntry[(int)ConfigOption.GrassQuality_DX11].SetValueUInt(_grassQualityDx11);
            configEntry[(int)ConfigOption.ParallaxOcclusion_DX11].SetValueUInt(_parallaxOcclusionDx11);
            configEntry[(int)ConfigOption.Tessellation_DX11].SetValueUInt(_tessellationDx11);
            configEntry[(int)ConfigOption.GlareRepresentation_DX11].SetValueUInt(_glareRepresentationDx11);
            configEntry[(int)ConfigOption.MapResolution_DX11].SetValueUInt(_mapResolutionDx11);
            configEntry[(int)ConfigOption.ShadowVisibilityTypeSelf_DX11].SetValueUInt(_shadowVisibilityTypeSelfDx11);
            configEntry[(int)ConfigOption.ShadowVisibilityTypeParty_DX11].SetValueUInt(_shadowVisibilityTypePartyDx11);
            configEntry[(int)ConfigOption.ShadowVisibilityTypeOther_DX11].SetValueUInt(_shadowVisibilityTypeOtherDx11);
            configEntry[(int)ConfigOption.ShadowVisibilityTypeEnemy_DX11].SetValueUInt(_shadowVisibilityTypeEnemyDx11);
            configEntry[(int)ConfigOption.ShadowLOD_DX11].SetValueUInt(_shadowLodDx11);
            configEntry[(int)ConfigOption.ShadowTextureSizeType_DX11].SetValueUInt(_shadowTextureSizeTypeDx11);
            configEntry[(int)ConfigOption.ShadowCascadeCountType_DX11].SetValueUInt(_shadowCascadeCountTypeDx11);
            configEntry[(int)ConfigOption.ShadowSoftShadowType_DX11].SetValueUInt(_shadowSoftShadowTypeDx11);
            configEntry[(int)ConfigOption.TextureFilterQuality_DX11].SetValueUInt(_textureFilterQualityDx11);
            configEntry[(int)ConfigOption.TextureAnisotropicQuality_DX11].SetValueUInt(_textureAnisotropicQualityDx11);
            configEntry[(int)ConfigOption.PhysicsTypeSelf_DX11].SetValueUInt(_physicsTypeSelfDx11);
            configEntry[(int)ConfigOption.PhysicsTypeParty_DX11].SetValueUInt(_physicsTypePartyDx11);
            configEntry[(int)ConfigOption.PhysicsTypeOther_DX11].SetValueUInt(_physicsTypeOtherDx11);
            configEntry[(int)ConfigOption.PhysicsTypeEnemy_DX11].SetValueUInt(_physicsTypeEnemyDx11);
            configEntry[(int)ConfigOption.RadialBlur_DX11].SetValueUInt(_radialBlurDx11);
            configEntry[(int)ConfigOption.SSAO_DX11].SetValueUInt(_ssaoDx11);
            configEntry[(int)ConfigOption.Glare_DX11].SetValueUInt(_glareDx11);
            configEntry[(int)ConfigOption.DistortionWater_DX11].SetValueUInt(_distortionWaterDx11);
            configEntry[(int)ConfigOption.IsSndMaster].SetValueUInt(_soundEnabled);
        }
        #endregion

        #region GfxConfig
        /// <summary>
        /// Basic check if GFX settings are low
        /// </summary>
        /// <returns></returns>
        public static bool CheckLowSettings()
        {
            return _originalObjQuantity == 4 &&
                   _waterWetDx11 == 0 &&
                   _occlusionCullingDx11 == 1 &&
                   _reflectionTypeDx11 == 3 &&
                   _grassQualityDx11 == 3 &&
                   _ssaoDx11 == 4;
        }
        /// <summary>
        /// Set the GFX to minimal
        /// </summary>
        public static unsafe void SetMinimalGfx()
        {
            var configEntry = Framework.Instance()->SystemConfig.SystemConfigBase.ConfigBase.ConfigEntry;

            configEntry[(int)ConfigOption.FPSInActive].SetValueUInt(0);
            configEntry[(int)ConfigOption.DisplayObjectLimitType].SetValueUInt(4);
            configEntry[(int)ConfigOption.WaterWet_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.OcclusionCulling_DX11].SetValueUInt(1);
            configEntry[(int)ConfigOption.LodType_DX11].SetValueUInt(1);
            configEntry[(int)ConfigOption.ReflectionType_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.AntiAliasing_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.TranslucentQuality_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.GrassQuality_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.ParallaxOcclusion_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.Tessellation_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.GlareRepresentation_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.MapResolution_DX11].SetValueUInt(2);
            configEntry[(int)ConfigOption.ShadowVisibilityTypeSelf_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.ShadowVisibilityTypeParty_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.ShadowVisibilityTypeOther_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.ShadowVisibilityTypeEnemy_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.ShadowLOD_DX11].SetValueUInt(1);
            configEntry[(int)ConfigOption.ShadowTextureSizeType_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.ShadowCascadeCountType_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.ShadowSoftShadowType_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.TextureFilterQuality_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.TextureAnisotropicQuality_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.PhysicsTypeSelf_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.PhysicsTypeParty_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.PhysicsTypeOther_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.PhysicsTypeEnemy_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.RadialBlur_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.SSAO_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.Glare_DX11].SetValueUInt(0);
            configEntry[(int)ConfigOption.DistortionWater_DX11].SetValueUInt(0);
        }
        #endregion

        #region Mute/Unmute MasterSound

        /// <summary>
        /// Mutes/Unmutes the sound
        /// </summary>
        /// <param name="enabled"></param>
        public static void SetMasterSoundEnable(bool enabled)
        {
            Api.GameConfig?.Set(SystemConfigOption.IsSndMaster, !enabled);
        }

        public static bool GetMasterSoundEnable()
        {
            return Api.GameConfig != null && Api.GameConfig.TryGet(SystemConfigOption.IsSndMaster, out bool isSndMaster) && isSndMaster;
        }
        #endregion
    }
}

public unsafe class AgentInterface
{
    public nint Pointer { get; }
    public nint VTable { get; }
    public int Id { get; }
    public FFXIVClientStructs.FFXIV.Client.UI.Agent.AgentInterface* Struct => (FFXIVClientStructs.FFXIV.Client.UI.Agent.AgentInterface*)Pointer;

    public AgentInterface(nint pointer, int id)
    {
        Pointer = pointer;
        Id = id;
        VTable = Marshal.ReadIntPtr(Pointer);
    }

    public unsafe AgentInterface(AgentId id)
    {
        Pointer = (nint)AgentModule.Instance()->GetAgentByInternalId(id);
        Id = (int)id;
        VTable = (nint)AgentModule.Instance()->GetAgentByInternalId(id)->VirtualTable;
    }

    public override string ToString()
    {
        return $"{Id} {(long)Pointer:X} {(long)VTable:X}";
    }
}
