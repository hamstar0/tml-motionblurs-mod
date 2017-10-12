using System;


namespace MotionBlurs {
	public class MotionBlursConfigData {
		public readonly static Version ConfigVersion = new Version( 1, 0, 0 );


		public string VersionSinceUpdate = "";

		public bool Enabled = true;
		
		public int NpcTrailLength = 10;
		public int ProjTrailLength = 10;

		public int NpcHighestIntensity = 72;
		public int ProjHighestIntensity = 64;

		public float NpcMotionScale = 1f / 8f;
		public float ProjMotionScale = 1f / 10f;



		////////////////

		public bool UpdateToLatestVersion() {
			var new_config = new MotionBlursConfigData();
			var vers_since = this.VersionSinceUpdate != "" ?
				new Version( this.VersionSinceUpdate ) :
				new Version();

			if( vers_since >= MotionBlursConfigData.ConfigVersion ) {
				return false;
			}
			
			this.VersionSinceUpdate = MotionBlursConfigData.ConfigVersion.ToString();

			return true;
		}
	}
}
