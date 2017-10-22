using HamstarHelpers.DebugHelpers;
using HamstarHelpers.Utilities.Config;
using System;
using Terraria;
using Terraria.ModLoader;


namespace MotionBlurs {
	public static class MotionBlursModSettings {
		public static MotionBlursConfigData Get() {
			return MotionBlursMod.instance.Config.Data;
		}
	}


	public static class MotionBlursAPI {
		public static void BeginCustomEntityBlur( Entity ent, int intensity ) {
			if( ent is NPC ) {
				var npc_info = MyNpcInfo.GetNpcInfo<MyNpcInfo>( ent.whoAmI );
				if( npc_info == null ) { return; }

				npc_info.Fx.SetCustomIntensity( intensity );
			} else if( ent is Projectile ) {
				var proj_info = MyProjectileInfo.GetProjInfo<MyProjectileInfo>( ent.whoAmI );
				if( proj_info == null ) { return; }

				proj_info.Fx.SetCustomIntensity( intensity );
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
		public static MotionBlursMod instance { get; private set; }

		
		////////////////

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
			MotionBlursMod.instance = this;

			var hamhelpmod = ModLoader.GetMod( "HamstarHelpers" );
			var min_ver = new Version( 1, 1, 4 );
			if( hamhelpmod.Version < min_ver ) {
				throw new Exception( "Hamstar's Helpers must be version " + min_ver.ToString() + " or greater." );
			}

			this.LoadConfig();
		}

		public override void Unload() {
			MotionBlursMod.instance = null;
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
