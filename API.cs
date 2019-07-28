using System;
using Terraria;


namespace MotionBlurs {
	public static class MotionBlursAPI {
		public static void BeginCustomEntityBlur( Entity ent, Func<Entity, int> intensityFunc ) {
			if( ent is NPC ) {
				var npcInfo = ((NPC)ent).GetGlobalNPC<MotionBlursNpc>();
				if( npcInfo == null ) { return; }

				npcInfo.Fx.SetCustomIntensity( intensityFunc );
			} else if( ent is Projectile ) {
				var projInfo = ((Projectile)ent).GetGlobalProjectile<MyProjectile>();
				if( projInfo == null ) { return; }

				projInfo.Fx.SetCustomIntensity( intensityFunc );
			} else {
				throw new Exception( "Invalid entity type." );
			}
		}


		public static void EndCustomEntityBlur( Entity ent ) {
			if( ent is NPC ) {
				var npcInfo = ((NPC)ent).GetGlobalNPC<MotionBlursNpc>();
				if( npcInfo == null ) { return; }

				npcInfo.Fx.SetCustomIntensity( null );
			} else if( ent is Projectile ) {
				var projInfo = ((Projectile)ent).GetGlobalProjectile<MyProjectile>();
				if( projInfo == null ) { return; }

				projInfo.Fx.SetCustomIntensity( null );
			} else {
				throw new Exception( "Invalid entity type." );
			}
		}
	}
}
