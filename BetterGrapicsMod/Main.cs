using ModLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterGrapicsMod
{
    [MainModClass]
    public class Main : Mod
    {
        public const string ANTI_ALIASING_OPTION_ID = "AntiAliasing";
        public const string SHADOW_DISTANCE_OPTION_ID = "ShadowDistance";
        public const string SHADOW_RESOLUTION_OPTION_ID = "ShadowRes";
        public const string TARGET_FPS_OPTION_ID = "fpsTarget";
        public const string SOFT_PARTICLES_OPTION_ID = "SoftParticles";
        public const string ANISOTROPIC_FILTERING_OPTION_ID = "AnisotropicFiltering";


        protected override void OnModRefreshed()
        {
            /*Application.targetFrameRate = 1000;
            QualitySettings.vSyncCount = 0;

            QualitySettings.antiAliasing = 8;
            QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
            QualitySettings.softParticles = true;
            QualitySettings.shadowDistance = 1000f;
            QualitySettings.softParticles = true;*/

            SetAntiAliasingEnabled(ModdedSettings.GetModdedSettingsIntValue(this, ANTI_ALIASING_OPTION_ID, 2));
            SetShadowDistance(ModdedSettings.GetModdedSettingsFloatValue(this, SHADOW_DISTANCE_OPTION_ID, 500f));
            SetShadowResolution((ShadowResolution)ModdedSettings.GetModdedSettingsIntValue(this, SHADOW_RESOLUTION_OPTION_ID, 2));
            SetFPSTarget(ModdedSettings.GetModdedSettingsIntValue(this, TARGET_FPS_OPTION_ID, 300));
            SetSoftParticles(ModdedSettings.GetModdedSettingsBoolValue(this, SOFT_PARTICLES_OPTION_ID, true));
            SetAnisotropicFiltering((AnisotropicFiltering)ModdedSettings.GetModdedSettingsIntValue(this, ANISOTROPIC_FILTERING_OPTION_ID, (int)AnisotropicFiltering.Enable));
        }

        protected override bool ImplementsSettingsWindow() => true;
        protected override void CreateSettingsWindow(ModOptionsWindowBuilder builder)
        {
            var page = builder.AddPage("Grapics");
            page.AddDropdown(new string[] { "Disabled", "2x Multi Sampling", "4x Multi Sampling", "8x Multi Sampling" }, 2, "AntiAliasing", ANTI_ALIASING_OPTION_ID, null, null, SetAntiAliasingEnabled);
            page.AddSlider(10, 1000, 500, "Shadow distance", SHADOW_DISTANCE_OPTION_ID, null, null, SetShadowDistance);
            page.AddDropdown<ShadowResolution>(ShadowResolution.High, "Shadow resolution", SHADOW_RESOLUTION_OPTION_ID, null, null, SetShadowResolution);
            page.AddIntSlider(5, 1000, 300, "Target FPS", TARGET_FPS_OPTION_ID, null, null, SetFPSTarget);
            page.AddCheckbox(true, "Soft Particles", SOFT_PARTICLES_OPTION_ID, null, null, SetSoftParticles);
            page.AddDropdown<AnisotropicFiltering>(AnisotropicFiltering.Enable, "AnisotropicFiltering", ANTI_ALIASING_OPTION_ID, null, null, SetAnisotropicFiltering);
        }

        void SetAntiAliasingEnabled(int index)
        {
            switch (index)
            {
                case 0:
                    QualitySettings.antiAliasing = 0;
                    break;
                case 1:
                    QualitySettings.antiAliasing = 2;
                    break;
                case 2:
                    QualitySettings.antiAliasing = 4;
                    break;
                case 3:
                    QualitySettings.antiAliasing = 8;
                    break;
            }
        }
        void SetShadowDistance(float distance)
        {
            QualitySettings.shadowDistance = distance;
        }
        void SetShadowResolution(ShadowResolution resolution)
        {
            QualitySettings.shadowResolution = resolution;
        }
        void SetFPSTarget(int fps)
        {
            Application.targetFrameRate = fps;
        }
        void SetSoftParticles(bool state)
        {
            QualitySettings.softParticles = state;
        }
        void SetAnisotropicFiltering(AnisotropicFiltering state)
        {
            QualitySettings.anisotropicFiltering = state;
        }
    }
}
