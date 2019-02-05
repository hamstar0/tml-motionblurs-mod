using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using Terraria.ModLoader;


namespace MotionBlurs {
    partial class MotionBlursMod : Mod {
		public JsonConfig<MotionBlursConfigData> ConfigJson { get; private set; }
		public MotionBlursConfigData Config { get { return this.ConfigJson.Data; } }



		////////////////

		public MotionBlursMod() {
			this.ConfigJson = new JsonConfig<MotionBlursConfigData>( MotionBlursConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new MotionBlursConfigData() );
		}

		////

		public override void Load() {
			MotionBlursMod.Instance = this;
			
			this.LoadConfig();
		}

		public override void Unload() {
			MotionBlursMod.Instance = null;
		}


		private void LoadConfig() {
			try {
				if( !this.ConfigJson.LoadFile() ) {
					this.ConfigJson.SaveFile();
				}
			} catch( Exception e ) {
				LogHelpers.Log( e.Message );
				this.ConfigJson.SaveFile();
			}

			if( this.ConfigJson.Data.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Motion Blurs updated to " + MotionBlursConfigData.ConfigVersion.ToString() );
				this.ConfigJson.SaveFile();
			}
		}


		////////////////

		public bool IsEnabled() {
			return this.ConfigJson.Data.Enabled;
		}
	}
}
