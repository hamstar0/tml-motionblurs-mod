using HamstarHelpers.TmlHelpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MotionBlurs.NpcExtensions;
using Terraria;
using Terraria.ModLoader;


namespace MotionBlurs {
	class MyNpcInfo : AltNPCInfo {
		public NpcFxHandler Fx { get; private set; }


		////////////////

		public override bool CanInitialize( NPC npc ) {
			return true;
		}

		public override void Initialize( NPC npc ) {
			this.Fx = new NpcFxHandler( MotionBlursMod.instance );
		}

		////////////////

		public override void Update() {
			this.Fx.Update( this.Npc );
		}
	}

	
	////////////////

	class MyGlobalNpc : GlobalNPC {
		public override bool PreDraw( NPC npc, SpriteBatch sb, Color draw_color ) {
			var mymod = (MotionBlursMod)this.mod;
			if( !mymod.IsEnabled() ) { return base.PreDraw( npc, sb, draw_color ); }

			var npc_info = MyNpcInfo.GetNpcInfo<MyNpcInfo>( npc.whoAmI );

			if( npc_info != null ) {
				npc_info.Fx.RenderTrail( mymod, sb, npc, draw_color );
			}
			
			return base.PreDraw( npc, sb, draw_color );
		}
	}
}
