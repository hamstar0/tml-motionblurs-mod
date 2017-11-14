using System;
using Terraria;


namespace MotionBlurs {
	public static class MotionBlursAPI {
		public static MotionBlursConfigData GetModSettings() {
			return MotionBlursMod.Instance.Config.Data;
		}


		public static void BeginCustomEntityBlur( Entity ent, Func<Entity, int> intensity_func ) {
			if( ent is NPC ) {
				var npc_info = MyNpcInfo.GetNpcInfo<MyNpcInfo>( ent.whoAmI );
				if( npc_info == null ) { return; }

				npc_info.Fx.SetCustomIntensity( intensity_func );
			} else if( ent is Projectile ) {
				var proj_info = MyProjectileInfo.GetProjInfo<MyProjectileInfo>( ent.whoAmI );
				if( proj_info == null ) { return; }

				proj_info.Fx.SetCustomIntensity( intensity_func );
			} else {
				throw new Exception( "Invalid entity type." );
			}
		}


		public static void EndCustomEntityBlur( Entity ent ) {
			if( ent is NPC ) {
				var npc_info = MyNpcInfo.GetNpcInfo<MyNpcInfo>( ent.whoAmI );
				if( npc_info == null ) { return; }

				npc_info.Fx.SetCustomIntensity( null );
			} else if( ent is Projectile ) {
				var proj_info = MyProjectileInfo.GetProjInfo<MyProjectileInfo>( ent.whoAmI );
				if( proj_info == null ) { return; }

				proj_info.Fx.SetCustomIntensity( null );
			} else {
				throw new Exception( "Invalid entity type." );
			}
		}
	}
}
