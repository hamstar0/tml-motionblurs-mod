using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.TModLoader.Mods;
using System;
using Terraria.ModLoader;


namespace MotionBlurs {
    partial class MotionBlursMod : Mod {
		public static MotionBlursMod Instance { get; private set; }



		////////////////

		public MotionBlursConfig Config => ModContent.GetInstance<MotionBlursConfig>();



		////////////////

		public MotionBlursMod() {
			MotionBlursMod.Instance = this;
		}

		////

		public override void Load() {
		}

		public override void Unload() {
			MotionBlursMod.Instance = null;
		}


		////////////////

		public override object Call( params object[] args ) {
			return ModBoilerplateHelpers.HandleModCall( typeof( MotionBlursAPI ), args );
		}
	}
}
