using HamstarHelpers.Classes.UI.ModConfig;
using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace MotionBlurs {
	public class MotionBlursConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ClientSide;


		////

		[Range( 0, 1000 )]
		[DefaultValue( 17 )]
		public int NpcTrailLength = 17;

		[Range( 0, 1000 )]
		[DefaultValue( 12 )]
		public int ProjTrailLength = 12;


		[Range( 0f, 1000f )]
		[DefaultValue( 8f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float NpcTrailFadeIncrements = 8f;

		[Range( 0f, 1000f )]
		[DefaultValue( 10f )]
		[CustomModConfigItem( typeof( FloatInputElement ) )]
		public float ProjTrailFadeIncrements = 10f;


		[Range( 0, 1000 )]
		[DefaultValue( 11 )]
		public int NpcBaseIntensity = 11;

		[Range( 0, 1000 )]
		[DefaultValue( 8 )]
		public int ProjBaseIntensity = 8;


		[Range( 0, 1000 )]
		[DefaultValue( 128 )]
		public int NpcMaxIntensity = 128;

		[Range( 0, 1000 )]
		[DefaultValue( 96 )]
		public int ProjMaxIntensity = 96;
	}
}
