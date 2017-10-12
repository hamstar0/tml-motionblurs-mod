using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Config;
using System;
using Terraria.ModLoader;


namespace MotionBlurs {
	public static class MotionBlursModSettings {
		public static MotionBlursConfigData Get() {
			var mymod = (MotionBlursMod)ModLoader.GetMod( "MotionBlurs" );
			return mymod.Config.Data;
		}
	}



    class MotionBlursMod : Mod {
		public JsonConfig<MotionBlursConfigData> Config { get; private set; }


		////////////////

		public MotionBlursMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};

			string filename = "Motion Blurs Config.json";
			this.Config = new JsonConfig<MotionBlursConfigData>( filename, "Mod Configs", new MotionBlursConfigData() );
		}

		////////////////

		public override void Load() {
			var hamhelpmod = ModLoader.GetMod( "HamstarHelpers" );
			var min_ver = new Version( 1, 1, 3 );
			if( hamhelpmod.Version < min_ver ) {
				throw new Exception( "Hamstar's Helpers must be version " + min_ver.ToString() + " or greater." );
			}

			this.LoadConfig();
		}


		private void LoadConfig() {
			try {
				if( !this.Config.LoadFile() ) {
					this.Config.SaveFile();
				}
			} catch( Exception e ) {
				DebugHelpers.Log( e.Message );
				this.Config.SaveFile();
			}

			if( this.Config.Data.UpdateToLatestVersion() ) {
				ErrorLogger.Log( "Motion Blurs updated to " + MotionBlursConfigData.ConfigVersion.ToString() );
				this.Config.SaveFile();
			}
		}


		public override void PostSetupContent() {
			MyNpcInfo.RegisterInfoType<MyNpcInfo>();
			MyProjectileInfo.RegisterInfoType<MyProjectileInfo>();
		}


		////////////////

		public bool IsEnabled() {
			return this.Config.Data.Enabled;
		}
	}
}
