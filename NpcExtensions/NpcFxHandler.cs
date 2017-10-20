using HamstarHelpers.DebugHelpers;
using HamstarHelpers.NPCHelpers;
using HamstarHelpers.XnaHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
			int frame_height = tex.Height / Main.npcFrameCount[npc.type];
			int frame = npc.frame.Y / frame_height;

			int lit = (int)Math.Min( mymod.Config.Data.NpcMaxIntensity, (float)mymod.Config.Data.NpcBaseIntensity * npc.velocity.Length() );
			if( lit <= 8 ) { return; }
			
			Color base_color_a, base_color_b = default(Color);
			bool has_added_color = npc.color != default( Color );

			base_color_a = npc.GetAlpha( draw_color );
			if( has_added_color ) {
				base_color_b = npc.GetColor( draw_color );
			}

			float avg = (float)(base_color_a.R + base_color_a.G + base_color_a.B + base_color_a.A) / 4f;
			float scale = lit / avg;

			Color color_a = Color.Multiply( base_color_a, scale );
			Color color_b = Color.Multiply( base_color_b, scale );

			float re_avg = (float)(color_a.R + color_a.G + color_a.B + color_a.A) / 4f;
			float inc = mymod.Config.Data.NpcTrailFadeIncrements / re_avg;

//DebugHelpers.SetDisplay( npc.TypeName + "_info", "lit:"+lit+", avg: "+avg.ToString("N2")+", inc:"+inc.ToString("N2"), 30 );
//DebugHelpers.SetDisplay( npc.TypeName + "_colors", "a:" + color_a + ", b:" + color_b, 30 );
			var list = new List<string>();
			for( int i = 0; i <= this.CurrentTrailLength; i++ ) {
				float rot = this.TrailRotations[i];
				Vector2 pos = this.TrailPositions[i];
				
				float lerp = i * inc;

				Color lerped_color_a = Color.Lerp( color_a, Color.Transparent, lerp );
				if( lerped_color_a.A <= 8 ) { break; }

				NPCHelpers.DrawSimple( sb, npc, frame, pos, rot, npc.scale, lerped_color_a );

				if( has_added_color ) {
					Color lerped_color_b = Color.Lerp( color_b, Color.Transparent, lerp );

					NPCHelpers.DrawSimple( sb, npc, frame, pos, rot, npc.scale, lerped_color_b );
				}
//list.Add( "R:" + lerped_color_a.R + "+G:" + lerped_color_a.G + "+B:" +lerped_color_a.B+"+A:" +lerped_color_a.A );
			}
//DebugHelpers.SetDisplay( npc.TypeName + "_trail", string.Join(", ",list.ToArray()), 30 );
		}
	}
}
