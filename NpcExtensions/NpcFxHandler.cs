using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;


namespace MotionBlurs.NpcExtensions {
	class NpcFxHandler {
		public static int GetDefaultNpcIntensity( NPC npc ) {
			var mymod = MotionBlursMod.Instance;

			return (int)Math.Min( mymod.Config.NpcMaxIntensity, (float)mymod.Config.NpcBaseIntensity * npc.velocity.Length() );
		}



		////////////////

		private Vector2[] TrailPositions;
		private float[] TrailRotations;

		private int CurrentTrailLength = 0;

		public Func<Entity, int> IntensityGetter { get; private set; }



		////////////////

		public NpcFxHandler() {
			int len = MotionBlursMod.Instance.Config.NpcTrailLength;

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

		public void GetRenderColors( NPC npc, Color drawColor, int intensity, out Color mainColor, out Color? overlayColor ) {
			Color baseColorA, baseColorB = default( Color );
			bool hasAddedColor = npc.color != default( Color );

			baseColorA = npc.GetAlpha( drawColor );
			if( hasAddedColor ) {
				baseColorB = npc.GetColor( drawColor );
			}

			float avg = (float)(baseColorA.R + baseColorA.G + baseColorA.B + baseColorA.A) / 4f;
			float scale = intensity / avg;

			mainColor = Color.Multiply( baseColorA, scale );
			overlayColor = Color.Multiply( baseColorB, scale );
		}

		public int GetNpcIntensity( NPC npc ) {
			var mymod = MotionBlursMod.Instance;

			if( this.IntensityGetter != null ) {
				return this.IntensityGetter( npc );
			}
			return NpcFxHandler.GetDefaultNpcIntensity( npc );
		}

		////////////////

		public void SetCustomIntensity( Func<Entity, int> intensityFunc ) {
			this.IntensityGetter = intensityFunc;
		}


		////////////////

		public void RenderTrail( SpriteBatch sb, NPC npc, Color drawColor ) {
			var mymod = MotionBlursMod.Instance;
			int intensity = this.GetNpcIntensity( npc );
			if( intensity <= 8 ) { return; }

			Color mainColor;
			Color? overlayColor;

			this.GetRenderColors( npc, drawColor, intensity, out mainColor, out overlayColor );

			float reAvg = (float)(mainColor.R + mainColor.G + mainColor.B + mainColor.A) / 4f;
			float inc = mymod.Config.NpcTrailFadeIncrements / reAvg;

			this.RenderTrailWithSettings( sb, npc, mainColor, overlayColor, inc );
		}


		public void RenderTrailWithSettings( SpriteBatch sb, NPC npc, Color mainColor, Color? overlayColor, float fadeAmount ) {
			Texture2D tex = Main.npcTexture[npc.type];
			int frameHeight = tex.Height / Main.npcFrameCount[npc.type];
			int frame = npc.frame.Y / frameHeight;

//DebugHelpers.SetDisplay( npc.TypeName + "_info", "lit:"+lit+", avg: "+avg.ToString("N2")+", inc:"+inc.ToString("N2"), 30 );
//DebugHelpers.SetDisplay( npc.TypeName + "_colors", "a:" + color_a + ", b:" + color_b, 30 );
//var list = new List<string>();
			for( int i = 0; i <= this.CurrentTrailLength; i++ ) {
				float rot = this.TrailRotations[i];
				Vector2 pos = this.TrailPositions[i];
				
				float lerp = i * fadeAmount;

				Color lerpedColorA = Color.Lerp( mainColor, Color.Transparent, lerp );
				if( lerpedColorA.A <= 8 ) { break; }

//DebugHelpers.Print( npc.TypeName+npc.whoAmI+"_"+i, "pos:"+(int)pos.X+":"+(int)pos.Y+", rot:"+rot.ToString("N2")+",color:"+lerpedColorA, 20 );
				NPCDrawHelpers.DrawSimple( sb, npc, frame, pos, rot, npc.scale, lerpedColorA );

				if( overlayColor.HasValue ) {
					Color lerpedColorB = Color.Lerp( overlayColor.Value, Color.Transparent, lerp );

					NPCDrawHelpers.DrawSimple( sb, npc, frame, pos, rot, npc.scale, lerpedColorB );
				}
//list.Add( "R:" + lerped_color_a.R + "+G:" + lerped_color_a.G + "+B:" +lerped_color_a.B+"+A:" +lerped_color_a.A );
			}
//DebugHelpers.SetDisplay( npc.TypeName + "_trail", string.Join(", ",list.ToArray()), 30 );
		}
	}
}
