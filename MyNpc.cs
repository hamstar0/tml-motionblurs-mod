using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MotionBlurs.NpcExtensions;
using Terraria;
using Terraria.ModLoader;


namespace MotionBlurs {
	class MotionBlursNpc : GlobalNPC {
		public NpcFxHandler Fx { get; private set; }

		////////////////

		public override bool InstancePerEntity => true;
		//public override bool CloneNewInstances => true;



		////////////////

		public override void SetDefaults( NPC npc ) {
			this.Fx = new NpcFxHandler();
		}

		////////////////

		public override bool PreDraw( NPC npc, SpriteBatch sb, Color drawColor ) {
			this.Fx.Update( npc );
			this.Fx.RenderTrail( sb, npc, drawColor );
			
			return base.PreDraw( npc, sb, drawColor );
		}
	}
}
