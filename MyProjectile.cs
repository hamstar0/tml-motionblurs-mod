using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MotionBlurs.ProjectileExtensions;
using System;
using Terraria;
using Terraria.ModLoader;


namespace MotionBlurs {
	class MyProjectile : GlobalProjectile {
		public ProjectileFxHandler Fx { get; private set; }

		////////////////

		public override bool InstancePerEntity => true;
		//public override bool CloneNewInstances => true;



		////////////////

		public override void SetDefaults( Projectile npc ) {
			this.Fx = new ProjectileFxHandler();
		}


		////////////////

		public override bool PreDraw( Projectile projectile, SpriteBatch sb, Color lightColor ) {
			this.Fx.Update( projectile );
			this.Fx.RenderTrail( sb, projectile, lightColor );

			return base.PreDraw( projectile, sb, lightColor );
		}
	}
}
