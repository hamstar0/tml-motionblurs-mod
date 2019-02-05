using HamstarHelpers.Components.Config;
using HamstarHelpers.Helpers.DebugHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace MotionBlurs {
	partial class MotionBlursMod : Mod {
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
	}
}
