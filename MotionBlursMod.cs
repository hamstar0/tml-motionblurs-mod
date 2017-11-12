using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Config;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace MotionBlurs {
	public static class MotionBlursAPI {
		public static MotionBlursConfigData GetModSettings() {
			return MotionBlursMod.Instance.Config.Data;
		}


		public static void BeginCustomEntityBlur( Entity ent, Func<Entity, int> intensity_func ) {
			if( ent is NPC ) {
				var npc_info = MyNpcInfo.GetNpcInfo<MyNpcInfo>( ent.whoAmI );
				if( npc_info == null ) { return; }

				npc_info.Fx.SetCustomIntensity( intensity_func );
			} else if( ent is Projectile ) {
				var proj_info = MyProjectileInfo.GetProjInfo<MyProjectileInfo>( ent.whoAmI );
				if( proj_info == null ) { return; }

				proj_info.Fx.SetCustomIntensity( intensity_func );
			} else {
				throw new Exception("Invalid entity type.");
			}
		}


		public static void EndCustomEntityBlur( Entity ent ) {
			if( ent is NPC ) {
				var npc_info = MyNpcInfo.GetNpcInfo<MyNpcInfo>( ent.whoAmI );
				if( npc_info == null ) { return; }

				npc_info.Fx.SetCustomIntensity( null );
			} else if( ent is Projectile ) {
				var proj_info = MyProjectileInfo.GetProjInfo<MyProjectileInfo>( ent.whoAmI );
				if( proj_info == null ) { return; }

				proj_info.Fx.SetCustomIntensity( null );
			} else {
				throw new Exception( "Invalid entity type." );
			}
		}
	}



    class MotionBlursMod : Mod {
		public static string GithubUserName { get { return "hamstar0"; } }
		public static string GithubProjectName { get { return "tml-motionblurs-mod"; } }

		public static string ConfigRelativeFilePath {
			get { return ConfigurationDataBase.RelativePath + Path.DirectorySeparatorChar + MotionBlursConfigData.ConfigFileName; }
		}
		public static void ReloadConfigFromFile() {
			if( Main.netMode != 0 ) {
				throw new Exception( "Cannot reload configs outside of single player." );
			}
			if( MotionBlursMod.Instance != null ) {
				MotionBlursMod.Instance.Config.LoadFile();
			}
		}
		
		public static MotionBlursMod Instance { get; private set; }

		
		////////////////

		public JsonConfig<MotionBlursConfigData> Config { get; private set; }


		////////////////

		public MotionBlursMod() {
			this.Properties = new ModProperties() {
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
			
			this.Config = new JsonConfig<MotionBlursConfigData>( MotionBlursConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new MotionBlursConfigData() );
		}

		////////////////

		public override void Load() {
			MotionBlursMod.Instance = this;

			var hamhelpmod = ModLoader.GetMod( "HamstarHelpers" );
			var min_ver = new Version( 1, 2, 0 );
			if( hamhelpmod.Version < min_ver ) {
				throw new Exception( "Hamstar's Helpers must be version " + min_ver.ToString() + " or greater." );
			}

			this.LoadConfig();
		}

		public override void Unload() {
			MotionBlursMod.Instance = null;
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
