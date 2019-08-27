using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace MotionBlurs {
	public class MotionBlursConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ClientSide;


		////

		[DefaultValue( 17 )]
		public int NpcTrailLength = 17;
		[DefaultValue( 12 )]
		public int ProjTrailLength = 12;

		[DefaultValue( 8f )]
		public float NpcTrailFadeIncrements = 8f;
		[DefaultValue( 10f )]
		public float ProjTrailFadeIncrements = 10f;

		[DefaultValue( 11 )]
		public int NpcBaseIntensity = 11;
		[DefaultValue( 8 )]
		public int ProjBaseIntensity = 8;

		[DefaultValue( 128 )]
		public int NpcMaxIntensity = 128;
		[DefaultValue( 96 )]
		public int ProjMaxIntensity = 96;
	}
}
