using HamstarHelpers.ProjectileHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;


namespace MotionBlurs.ProjectileExtensions {
	class ProjectileFxHandler {
		public static int GetDefaultProjectileIntensity( MotionBlursConfigData data, Projectile proj ) {
			return (int)Math.Min( data.ProjMaxIntensity, (float)data.ProjBaseIntensity * proj.velocity.Length() );
		}


		////////////////

		private Vector2[] TrailPositions;
		private float[] TrailRotations;

		private int CurrentTrailLength = 0;

		public Func<Entity, int> IntensityGetter { get; private set; }


		////////////////

		public ProjectileFxHandler( MotionBlursMod mymod ) {
			int len = mymod.Config.Data.ProjTrailLength;

			this.TrailPositions = new Vector2[len];
			this.TrailRotations = new float[len];
			this.IntensityGetter = null;
			;
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

		public void GetRenderColors( Projectile proj, Color draw_color, int intensity, out Color main_color ) {
			Color base_color = proj.GetAlpha( draw_color );

			float avg = (float)(base_color.R + base_color.G + base_color.B + base_color.A) / 4f;
			float scale = intensity / avg;

			main_color = Color.Multiply( base_color, scale );
		}

		public int GetProjectileIntensity( MotionBlursMod mymod, Projectile proj ) {
			if( this.IntensityGetter != null ) {
				return this.IntensityGetter( proj );
			}
			return ProjectileFxHandler.GetDefaultProjectileIntensity( mymod.Config.Data, proj );
		}

		////////////////

		public void SetCustomIntensity( Func<Entity, int> intensity_func ) {
			this.IntensityGetter = intensity_func;
		}


		////////////////

		public void RenderTrail( MotionBlursMod mymod, SpriteBatch sb, Projectile proj, Color draw_color ) {
			int intensity = this.GetProjectileIntensity( mymod, proj );
			if( intensity <= 8 ) { return; }

			Color main_color;

			this.GetRenderColors( proj, draw_color, intensity, out main_color );

			float re_avg = (float)(main_color.R + main_color.G + main_color.B + main_color.A) / 4f;
			float fade_amount = mymod.Config.Data.ProjTrailFadeIncrements / re_avg;

			this.RenderTrailWithSettings( sb, proj, main_color, fade_amount );
		}


		public void RenderTrailWithSettings( SpriteBatch sb, Projectile proj, Color main_color, float fade_amount ) {
//DebugHelpers.SetDisplay( proj.Name + "_info", "lit:"+lit+", avg: "+ re_avg.ToString("N2")+", inc:"+inc.ToString("N2")+ ", scale:"+ scale.ToString("N2"), 30 );
//DebugHelpers.SetDisplay( proj.Name + "_colors", "base:" + base_color + ", color:" + color, 30 );
//var list = new List<string>();
			for( int i = 0; i <= this.CurrentTrailLength; i++ ) {
				float rot = this.TrailRotations[i];
				Vector2 pos = this.TrailPositions[i];

				float lerp = i * fade_amount;

				Color lerped_color = Color.Lerp( main_color, Color.Transparent, lerp );
				if( lerped_color.A <= 8 ) { break; }

				ProjectileHelpers.DrawSimple( sb, proj, pos, rot, lerped_color, proj.scale );
//list.Add( "R:" + lerped_color.R + "+G:" + lerped_color.G + "+B:" + lerped_color.B+"+A:" + lerped_color.A );
			}
//DebugHelpers.SetDisplay( proj.Name + "_trail", string.Join(", ",list.ToArray()), 30 );
		}
	}
}
