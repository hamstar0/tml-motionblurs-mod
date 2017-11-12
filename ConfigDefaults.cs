using HamstarHelpers.Utilities.Config;
using System;


namespace MotionBlurs {
	public class MotionBlursConfigData : ConfigurationDataBase {
		public readonly static Version ConfigVersion = new Version( 1, 1, 0 );
		public readonly static string ConfigFileName = "Motion Blurs Config.json";


		////////////////

		public string VersionSinceUpdate = "";

		public bool Enabled = true;
		
		public int NpcTrailLength = 10;
		public int ProjTrailLength = 10;

		public float NpcTrailFadeIncrements = 8f;
		public float ProjTrailFadeIncrements = 10f;

		public int NpcBaseIntensity = 10;
		public int ProjBaseIntensity = 8;

		public int NpcMaxIntensity = 128;
		public int ProjMaxIntensity = 96;



		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new MotionBlursConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= MotionBlursConfigData.ConfigVersion ) {
				return false;
			}

			if( vers_since < new Version( 1, 1, 0 ) ) {
				if( MotionBlursConfigData._1_0_0_NpcHighestIntensity == this.NpcBaseIntensity ) {
					this.NpcBaseIntensity = new_config.NpcBaseIntensity;
				}
				if( MotionBlursConfigData._1_0_0_ProjHighestIntensity == this.ProjBaseIntensity ) {
					this.ProjBaseIntensity = new_config.ProjBaseIntensity;
				}
			}

			this.VersionSinceUpdate = MotionBlursConfigData.ConfigVersion.ToString();

			return true;
		}


		////////////////

		public string _OLD_SETTINGS_BELOW = "";

		public readonly static float _1_0_0_NpcHighestIntensity = 72;
		public readonly static float _1_0_0_ProjHighestIntensity = 64;
	}
}
