using HamstarHelpers.DebugHelpers;
using HamstarHelpers.ProjectileHelpers;
using HamstarHelpers.XnaHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;


namespace MotionBlurs.ProjectileExtensions {
	class ProjectileFxHandler {
		private Vector2[] TrailPositions;
		private float[] TrailRotations;
		
		private int CurrentTrailLength = 0;


		////////////////

		public ProjectileFxHandler( MotionBlursMod mymod ) {
			int len = mymod.Config.Data.ProjTrailLength;

			this.TrailPositions = new Vector2[len];
			this.TrailRotations = new float[len];
		}

		////////////////

		public void Update( Projectile proj ) {
			for( int i = this.CurrentTrailLength; i >= 1; i-- ) {
				this.TrailPositions[i] = this.TrailPositions[i - 1];
				this.TrailRotations[i] = this.TrailRotations[i - 1];
			}

			this.TrailPositions[0] = proj.Center;
			this.TrailRotations[0] = proj.rotation;

			if( this.CurrentTrailLength < (this.TrailPositions.Length - 1) ) {
				this.CurrentTrailLength++;
			}
		}


		////////////////

		public void RenderTrail( MotionBlursMod mymod, SpriteBatch sb, Projectile proj, Color draw_color ) {
			int lit = (int)Math.Min( mymod.Config.Data.ProjMaxIntensity, (float)mymod.Config.Data.ProjBaseIntensity * proj.velocity.Length() );
			if( lit <= 8 ) { return; }

			Color base_color = proj.GetAlpha( draw_color );

			float avg = (float)(base_color.R + base_color.G + base_color.B + base_color.A) / 4f;
			float scale = lit / avg;

			Color color = Color.Multiply( base_color, scale );

			float re_avg = (float)(color.R + color.G + color.B + color.A) / 4f;
			float inc = mymod.Config.Data.ProjTrailFadeIncrements / re_avg;

//DebugHelpers.SetDisplay( proj.Name + "_info", "lit:"+lit+", avg: "+ re_avg.ToString("N2")+", inc:"+inc.ToString("N2")+ ", scale:"+ scale.ToString("N2"), 30 );
//DebugHelpers.SetDisplay( proj.Name + "_colors", "base:" + base_color + ", color:" + color, 30 );
//var list = new List<string>();
			for( int i = 0; i <= this.CurrentTrailLength; i++ ) {
				float rot = this.TrailRotations[i];
				Vector2 pos = this.TrailPositions[i];

				float lerp = i * inc;

				Color lerped_color = Color.Lerp( color, Color.Transparent, lerp );
				if( lerped_color.A <= 8 ) { break; }

				ProjectileHelpers.DrawSimple( sb, proj, pos, rot, lerped_color, proj.scale );
//list.Add( "R:" + lerped_color.R + "+G:" + lerped_color.G + "+B:" + lerped_color.B+"+A:" + lerped_color.A );
			}
//DebugHelpers.SetDisplay( proj.Name + "_trail", string.Join(", ",list.ToArray()), 30 );
		}
	}
}
