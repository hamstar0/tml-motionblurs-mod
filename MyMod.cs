using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using HamstarHelpers.Helpers.TmlHelpers;
using System;
using Terraria.ModLoader;


namespace MotionBlurs {
    partial class MotionBlursMod : Mod {
		public JsonConfig<MotionBlursConfigData> ConfigJson { get; private set; }
		public MotionBlursConfigData Config => this.ConfigJson.Data;



		////////////////

		public MotionBlursMod() {
			this.ConfigJson = new JsonConfig<MotionBlursConfigData>( MotionBlursConfigData.ConfigFileName,
				ConfigurationDataBase.RelativePath, new MotionBlursConfigData() );
		}

		////

		public override void Load() {
			string depErr = TmlHelpers.ReportBadDependencyMods( this );
			if( depErr != null ) { throw new HamstarException( depErr ); }

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
				ErrorLogger.Log( "Motion Blurs updated to " + this.Version.ToString() );
				this.ConfigJson.SaveFile();
			}
		}


		////////////////

		public override object Call( params object[] args ) {
			if( args == null || args.Length == 0 ) { throw new HamstarException( "Undefined call type." ); }

			string callType = args[0] as string;
			if( callType == null ) { throw new HamstarException( "Invalid call type." ); }

			var methodInfo = typeof( MotionBlursAPI ).GetMethod( callType );
			if( methodInfo == null ) { throw new HamstarException( "Invalid call type " + callType ); }

			var newArgs = new object[args.Length - 1];
			Array.Copy( args, 1, newArgs, 0, args.Length - 1 );

			try {
				return ReflectionHelpers.SafeCall( methodInfo, null, newArgs );
			} catch( Exception e ) {
				throw new HamstarException( "Barriers.BarrierMod.Call - Bad API call.", e );
			}
		}
	}
}
