using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MotionBlurs.NpcExtensions;
using Terraria;
using Terraria.ModLoader;


namespace MotionBlurs {
	class MotionBlursNpc : GlobalNPC {
		public NpcFxHandler Fx;


		////////////////

		public override bool InstancePerEntity { get { return true; } }
		public override bool CloneNewInstances { get { return true; } }

		public override void SetDefaults( NPC npc ) {
			this.Fx = new NpcFxHandler( (MotionBlursMod)this.mod );
		}

		////////////////

		public override bool PreDraw( NPC npc, SpriteBatch sb, Color draw_color ) {
			var mymod = (MotionBlursMod)this.mod;
			if( !mymod.IsEnabled() ) { return base.PreDraw( npc, sb, draw_color ); }
			
			this.Fx.RenderTrail( mymod, sb, npc, draw_color );
			
			return base.PreDraw( npc, sb, draw_color );
		}
	}
}
