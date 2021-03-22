using ModLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LowGravity
{
    [MainModClass]
    public class Main : Mod
    {
        public const string SETTINGS_SAVE_ID_GRAVITY = "Gravity";

        public float? NormalGravity = null;

        protected override void OnModRefreshed()
        {
            if (NormalGravity == null)
                NormalGravity = PhysicsManager.Instance.Gravity;

            PhysicsManager.Instance.Gravity = ModdedSettings.GetModdedSettingsIntValue(this, SETTINGS_SAVE_ID_GRAVITY, -10);
        }

        protected override void OnModDeactivated()
        {
            if (NormalGravity == null)
                return;

            PhysicsManager.Instance.Gravity = NormalGravity.Value;
        }

        protected override bool ImplementsSettingsWindow() => true;

        protected override void CreateSettingsWindow(ModOptionsWindowBuilder builder)
        {
            ModOptionsWindowBuilder.Page page = builder.AddPage("Settings");
            page.AddIntSlider(-100, 1, -10, "Gravity", SETTINGS_SAVE_ID_GRAVITY, null, null, delegate (int value)
            {
                PhysicsManager.Instance.Gravity = (float)value;
            });
        }
    }
}
