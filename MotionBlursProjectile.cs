using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MotionBlurs.ProjectileExtensions;
using Terraria;
using Terraria.ModLoader;


namespace MotionBlurs {
	class MotionBlursProjectile : GlobalProjectile {
		public ProjectileFxHandler Fx { get; private set; }


		////////////////

		public override bool InstancePerEntity { get { return true; } }
		public override bool CloneNewInstances { get { return true; } }

		public override void SetDefaults( Projectile npc ) {
			this.Fx = new ProjectileFxHandler( (MotionBlursMod)this.mod );
		}


		////////////////

		public override bool PreDraw( Projectile projectile, SpriteBatch sb, Color light_color ) {
			var mymod = (MotionBlursMod)this.mod;
			if( !mymod.IsEnabled() ) { return base.PreDraw( projectile, sb, light_color ); }
			
			this.Fx.RenderTrail( mymod, sb, projectile, light_color );

			return base.PreDraw( projectile, sb, light_color );
		}
	}
}
