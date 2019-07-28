using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;


namespace MotionBlurs.ProjectileExtensions {
	class ProjectileFxHandler {
		public static int GetDefaultProjectileIntensity( MotionBlursConfig data, Projectile proj ) {
			return (int)Math.Min( data.ProjMaxIntensity, (float)data.ProjBaseIntensity * proj.velocity.Length() );
		}



		////////////////

		private Vector2[] TrailPositions;
		private float[] TrailRotations;

		private int CurrentTrailLength = 0;

		public Func<Entity, int> IntensityGetter { get; private set; }



		////////////////

		public ProjectileFxHandler() {
			var mymod = MotionBlursMod.Instance;
			int len = mymod.Config.ProjTrailLength;

			this.TrailPositions = new Vector2[len];
			this.TrailRotations = new float[len];
			this.IntensityGetter = null;
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

		public void GetRenderColors( Projectile proj, Color drawColor, int intensity, out Color mainColor ) {
			Color baseColor = proj.GetAlpha( drawColor );

			float avg = (float)(baseColor.R + baseColor.G + baseColor.B + baseColor.A) / 4f;
			float scale = intensity / avg;

			mainColor = Color.Multiply( baseColor, scale );
		}

		public int GetProjectileIntensity( Projectile proj ) {
			var mymod = MotionBlursMod.Instance;

			if( this.IntensityGetter != null ) {
				return this.IntensityGetter( proj );
			}
			return ProjectileFxHandler.GetDefaultProjectileIntensity( mymod.Config, proj );
		}

		////////////////

		public void SetCustomIntensity( Func<Entity, int> intensityFunc ) {
			this.IntensityGetter = intensityFunc;
		}


		////////////////

		public void RenderTrail( SpriteBatch sb, Projectile proj, Color drawColor ) {
			var mymod = MotionBlursMod.Instance;
			int intensity = this.GetProjectileIntensity( proj );
			if( intensity <= 8 ) { return; }
			
			Color mainColor;

			this.GetRenderColors( proj, drawColor, intensity, out mainColor );

			float reAvg = (float)(mainColor.R + mainColor.G + mainColor.B + mainColor.A) / 4f;
			float fade_amount = mymod.Config.ProjTrailFadeIncrements / reAvg;

			this.RenderTrailWithSettings( sb, proj, mainColor, fade_amount );
		}


		public void RenderTrailWithSettings( SpriteBatch sb, Projectile proj, Color mainColor, float fadeAmount ) {
//DebugHelpers.SetDisplay( proj.Name + "_info", "lit:"+lit+", avg: "+ re_avg.ToString("N2")+", inc:"+inc.ToString("N2")+ ", scale:"+ scale.ToString("N2"), 30 );
//DebugHelpers.SetDisplay( proj.Name + "_colors", "base:" + base_color + ", color:" + color, 30 );
//var list = new List<string>();
			for( int i = 0; i <= this.CurrentTrailLength; i++ ) {
				float rot = this.TrailRotations[i];
				Vector2 pos = this.TrailPositions[i];

				float lerp = i * fadeAmount;

				Color lerpedColor = Color.Lerp( mainColor, Color.Transparent, lerp );
				if( lerpedColor.A <= 8 ) { break; }

//DebugHelpers.Print( proj.Name+proj.whoAmI+"_"+i, "pos:"+(int)pos.X+":"+(int)pos.Y+", rot:"+rot.ToString("N2")+",color:"+lerpedColor, 20 );
				ProjectileHelpers.DrawSimple( sb, proj, pos, rot, lerpedColor, proj.scale );
//list.Add( "R:" + lerped_color.R + "+G:" + lerped_color.G + "+B:" + lerped_color.B+"+A:" + lerped_color.A );
			}
//DebugHelpers.SetDisplay( proj.Name + "_trail", string.Join(", ",list.ToArray()), 30 );
		}
	}
}
