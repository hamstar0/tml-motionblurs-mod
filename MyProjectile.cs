using HamstarHelpers.Helpers.DebugHelpers;
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

		public override bool PreDraw( Projectile projectile, SpriteBatch sb, Color light_color ) {
			this.Fx.Update( projectile );
			this.Fx.RenderTrail( sb, projectile, light_color );

			return base.PreDraw( projectile, sb, light_color );
		}
	}
}
