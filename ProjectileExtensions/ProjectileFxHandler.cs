using HamstarHelpers.ProjectileHelpers;
using HamstarHelpers.XnaHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			Texture2D tex = Main.projectileTexture[ proj.type ];
			int tex_height = tex.Height / Main.projFrames[proj.type];
			Rectangle frame = new Rectangle( 0, proj.frame * tex_height, tex.Width, tex_height );

			float motion_intensity_scale = proj.velocity.Length() * mymod.Config.Data.ProjMotionScale;
			int lit = (int)((float)mymod.Config.Data.ProjHighestIntensity * motion_intensity_scale);
			int darken = lit / mymod.Config.Data.ProjTrailLength;

			if( lit <= 8 || darken == 0 ) { return; }

			Color color = draw_color;
			while( color.A > lit ) {
				color = XnaColorHelpers.Add( color, -darken, true );
			}
//DebugHelpers.SetDisplay( "proj", "intensity "+motion_intensity_scale.ToString("N1")+ ", draw "+draw_color+", alpha'd "+proj.GetAlpha( draw_color )+" color "+ color, 30 );
//DebugHelpers.SetDisplay( "proj", "base "+proj.GetAlpha( draw_color )+" color "+ color, 30 );
			
			for( int i = 0; i <= this.CurrentTrailLength; i++ ) {
				ProjectileHelpers.DrawSimple( sb, proj, this.TrailPositions[i], this.TrailRotations[i], color, proj.scale );

				color = XnaColorHelpers.Add( color, -darken, true );
				if( color.A <= 8 ) { break; }
			}
		}
	}
}
