using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace MotionBlurs {
    class MotionBlursMod : Mod {
		public static MotionBlursMod Instance { get; private set; }

		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-motionblurs-mod"; } }

		public static string ConfigFileRelativePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + MotionBlursConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( MotionBlursMod.Instance != null ) {
				if( !MotionBlursMod.Instance.ConfigJson.LoadFile() ) {
					MotionBlursMod.Instance.ConfigJson.SaveFile();
				}
			}
		}

		public static void ResetConfigFromDefaults() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reset to default configs outside of single player." );
			}

			var new_config = new MotionBlursConfigData();
			//new_config.SetDefaults();

			MotionBlursMod.Instance.ConfigJson.SetData( new_config );
			MotionBlursMod.Instance.ConfigJson.SaveFile();
		}


		////////////////

		public JsonConfig<MotionBlursConfigData> ConfigJson { get; private set; }
		public MotionBlursConfigData Config { get { return this.ConfigJson.Data; } }


		////////////////

		public MotionBlursMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			this.ConfigJson = new JsonConfig<MotionBlursConfigData>( MotionBlursConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new MotionBlursConfigData() );
		}

		////////////////

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
