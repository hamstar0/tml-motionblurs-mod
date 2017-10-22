using HamstarHelpers.NPCHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;


namespace MotionBlurs.NpcExtensions {
	class NpcFxHandler {
		public static int GetDefaultNpcIntensity( MotionBlursConfigData data, NPC npc ) {
			return (int)Math.Min( data.NpcMaxIntensity, (float)data.NpcBaseIntensity * npc.velocity.Length() );
		}


		////////////////

		private Vector2[] TrailPositions;
		private float[] TrailRotations;

		private int CurrentTrailLength = 0;

		public Func<Entity, int> IntensityGetter { get; private set; }


		////////////////

		public NpcFxHandler( MotionBlursMod mymod ) {
			int len = mymod.Config.Data.NpcTrailLength;

			this.TrailPositions = new Vector2[len];
			this.TrailRotations = new float[len];

			this.IntensityGetter = null;
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

		public void GetRenderColors( NPC npc, Color draw_color, int intensity, out Color main_color, out Color? overlay_color ) {
			Color base_color_a, base_color_b = default( Color );
			bool has_added_color = npc.color != default( Color );

			base_color_a = npc.GetAlpha( draw_color );
			if( has_added_color ) {
				base_color_b = npc.GetColor( draw_color );
			}

			float avg = (float)(base_color_a.R + base_color_a.G + base_color_a.B + base_color_a.A) / 4f;
			float scale = intensity / avg;

			main_color = Color.Multiply( base_color_a, scale );
			overlay_color = Color.Multiply( base_color_b, scale );
		}

		public int GetNpcIntensity( MotionBlursMod mymod, NPC npc ) {
			if( this.IntensityGetter != null ) {
				return this.IntensityGetter( npc );
			}
			return NpcFxHandler.GetDefaultNpcIntensity( mymod.Config.Data, npc );
		}

		////////////////

		public void SetCustomIntensity( Func<Entity, int> intensity_func ) {
			this.IntensityGetter = intensity_func;
		}


		////////////////

		public void RenderTrail( MotionBlursMod mymod, SpriteBatch sb, NPC npc, Color draw_color ) {
			int intensity = this.GetNpcIntensity( mymod, npc );
			if( intensity <= 8 ) { return; }

			Color main_color;
			Color? overlay_color;

			this.GetRenderColors( npc, draw_color, intensity, out main_color, out overlay_color );

			float re_avg = (float)(main_color.R + main_color.G + main_color.B + main_color.A) / 4f;
			float inc = mymod.Config.Data.NpcTrailFadeIncrements / re_avg;

			this.RenderTrailWithSettings( sb, npc, main_color, overlay_color, inc );
		}


		public void RenderTrailWithSettings( SpriteBatch sb, NPC npc, Color main_color, Color? overlay_color, float fade_amount ) {
			Texture2D tex = Main.npcTexture[npc.type];
			int frame_height = tex.Height / Main.npcFrameCount[npc.type];
			int frame = npc.frame.Y / frame_height;

//DebugHelpers.SetDisplay( npc.TypeName + "_info", "lit:"+lit+", avg: "+avg.ToString("N2")+", inc:"+inc.ToString("N2"), 30 );
//DebugHelpers.SetDisplay( npc.TypeName + "_colors", "a:" + color_a + ", b:" + color_b, 30 );
//var list = new List<string>();
			for( int i = 0; i <= this.CurrentTrailLength; i++ ) {
				float rot = this.TrailRotations[i];
				Vector2 pos = this.TrailPositions[i];
				
				float lerp = i * fade_amount;

				Color lerped_color_a = Color.Lerp( main_color, Color.Transparent, lerp );
				if( lerped_color_a.A <= 8 ) { break; }

				NPCHelpers.DrawSimple( sb, npc, frame, pos, rot, npc.scale, lerped_color_a );

				if( overlay_color.HasValue ) {
					Color lerped_color_b = Color.Lerp( overlay_color.Value, Color.Transparent, lerp );

					NPCHelpers.DrawSimple( sb, npc, frame, pos, rot, npc.scale, lerped_color_b );
				}
//list.Add( "R:" + lerped_color_a.R + "+G:" + lerped_color_a.G + "+B:" +lerped_color_a.B+"+A:" +lerped_color_a.A );
			}
//DebugHelpers.SetDisplay( npc.TypeName + "_trail", string.Join(", ",list.ToArray()), 30 );
		}
	}
}
