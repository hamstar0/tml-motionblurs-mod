using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MotionBlurs.NpcExtensions;
using Terraria;
using Terraria.ModLoader;


namespace MotionBlurs {
	class MyNpcInfo : AltNPCInfo {
		public NpcFxHandler Fx { get; private set; }
		public Color PrevColor = Color.White;


		////////////////

		public override bool CanInitialize( NPC npc ) {
			return true;
		}

		public override void Initialize( NPC npc ) {
			var mymod = (MotionBlursMod)ModLoader.GetMod( "MotionBlurs" );
			this.Fx = new NpcFxHandler( mymod );
		}

		////////////////

		public override void Update() {
			this.Fx.Update( this.Npc );
		}
	}



	class MyGlobalNpc : GlobalNPC {
		public override bool PreDraw( NPC npc, SpriteBatch sb, Color _ ) {
			var mymod = (MotionBlursMod)this.mod;
			if( !mymod.IsEnabled() ) { return base.PreDraw( npc, sb, _ ); }

			var npc_info = MyNpcInfo.GetNpcInfo<MyNpcInfo>( npc.whoAmI );

			if( npc_info != null ) {
				npc_info.Fx.RenderTrail( mymod, sb, npc, _ );
			}
			
			return base.PreDraw( npc, sb, _ );
		}

		public override void PostDraw( NPC npc, SpriteBatch sb, Color draw_color ) {
			var npc_info = MyNpcInfo.GetNpcInfo<MyNpcInfo>( npc.whoAmI );

			if( npc_info != null ) {
				npc_info.PrevColor = draw_color;
			}
		}
	}
}
