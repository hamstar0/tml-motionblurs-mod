using HamstarHelpers.DebugHelpers;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.XnaHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace MotionBlurs.NpcExtensions {
	class NpcFxHandler {
		private Vector2[] TrailPositions;
		private float[] TrailRotations;

		private int CurrentTrailLength = 0;


		////////////////

		public NpcFxHandler( MotionBlursMod mymod ) {
			int len = mymod.Config.Data.NpcTrailLength;

			this.TrailPositions = new Vector2[len];
			this.TrailRotations = new float[len];
		}

		////////////////

		public void Update( NPC npc ) {
			for( int i = this.CurrentTrailLength; i >= 1; i-- ) {
				this.TrailPositions[i] = this.TrailPositions[i - 1];
				this.TrailRotations[i] = this.TrailRotations[i - 1];
			}

			this.TrailPositions[0] = npc.position;
			this.TrailRotations[0] = npc.rotation;

			if( this.CurrentTrailLength < (this.TrailPositions.Length - 1) ) {
				this.CurrentTrailLength++;
			}
		}


		////////////////

		public void RenderTrail( MotionBlursMod mymod, SpriteBatch sb, NPC npc, Color draw_color ) {
			Texture2D tex = Main.npcTexture[ npc.type ];
			int frame = npc.frame.Y / tex.Height;

			float motion_intensity_scale = npc.velocity.Length() * mymod.Config.Data.NpcMotionScale;
			int lit = (int)((float)mymod.Config.Data.NpcHighestIntensity * motion_intensity_scale);
			int darken = lit / mymod.Config.Data.NpcTrailLength;

			if( lit <= 8 || darken == 0 ) { return; }
			
			Color base_color = npc.GetAlpha( draw_color );
			Color color = XnaColorHelpers.BlendInto( base_color, npc.color );
			while( color.A > lit ) {
				color = XnaColorHelpers.Add( color, -darken, true );
			}
//DebugHelpers.SetDisplay( npc.TypeName, "draw "+draw_color+"alpha'd "+npc.GetAlpha( draw_color )+" color "+ color, 30 );
//DebugHelpers.SetDisplay( npc.TypeName, "draw "+draw_color+" npc.color "+npc.color+" color "+color, 30 );
//DebugHelpers.SetDisplay( npc.TypeName, "draw "+draw_color+" npc.color "+npc.color+ " color " + color, 30 );
//DebugHelpers.SetDisplay( npc.TypeName, "draw "+draw_color+ " base_color " + base_color + " color " + color, 30 );
			
			for( int i = 0; i <= this.CurrentTrailLength; i++ ) {
				float rot = this.TrailRotations[i];
				Vector2 pos = this.TrailPositions[i];
				
				NPCHelpers.DrawSimple( sb, npc, frame, pos, rot, npc.scale, color );

				color = XnaColorHelpers.Add( color, -darken, true );
				if( color.A <= 8 ) { break; }
			}
		}
	}
}
