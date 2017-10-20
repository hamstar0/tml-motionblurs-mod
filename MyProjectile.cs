using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MotionBlurs.ProjectileExtensions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace MotionBlurs {
	class MyProjectileInfo : AltProjectileInfo {
		public ProjectileFxHandler Fx { get; private set; }

		
		////////////////

		public override bool CanInitialize( Projectile proj ) {
			if( proj.type == ProjectileID.SolarWhipSword ) { return false; }
			return true;
		}

		public override void Initialize( Projectile projectile ) {
			var mymod = (MotionBlursMod)ModLoader.GetMod( "MotionBlurs" );
			this.Fx = new ProjectileFxHandler( mymod );
		}


		////////////////

		public override void Update() {
			this.Fx.Update( this.Proj );
		}
	}


	
	class MyProjectile : GlobalProjectile {
		public override bool PreDraw( Projectile projectile, SpriteBatch sb, Color light_color ) {
			var mymod = (MotionBlursMod)this.mod;
			if( !mymod.IsEnabled() ) { return base.PreDraw( projectile, sb, light_color ); }

			var proj_info = MyProjectileInfo.GetProjInfo<MyProjectileInfo>( projectile.whoAmI );

			if( proj_info != null ) {
				proj_info.Fx.RenderTrail( mymod, sb, projectile, light_color );
			}

			return base.PreDraw( projectile, sb, light_color );
		}
	}
}
