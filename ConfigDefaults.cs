using HamstarHelpers.Components.Config;
using System;


namespace MotionBlurs {
	public class MotionBlursConfigData : ConfigurationDataBase {
		public readonly static string ConfigFileName = "Motion Blurs Config.json";


		////////////////

		public string VersionSinceUpdate = "";
		
		public int NpcTrailLength = 17;
		public int ProjTrailLength = 12;

		public float NpcTrailFadeIncrements = 8f;
		public float ProjTrailFadeIncrements = 10f;

		public int NpcBaseIntensity = 11;
		public int ProjBaseIntensity = 8;

		public int NpcMaxIntensity = 128;
		public int ProjMaxIntensity = 96;



		////////////////

		public void SetDefaults() {
		}

		////

		public bool UpdateToLatestVersion() {
			var mymod = MotionBlursMod.Instance;
			var newConfig = new MotionBlursConfigData();
			newConfig.SetDefaults();

			var versSince = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( versSince >= mymod.Version ) {
				return false;
			}

			if( this.VersionSinceUpdate == "" ) {
				this.SetDefaults();
			}

			this.VersionSinceUpdate = mymod.Version.ToString();

			return true;
		}
	}
}
