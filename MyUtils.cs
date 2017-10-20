using HamstarHelpers.XnaHelpers;
using Microsoft.Xna.Framework;


namespace MotionBlurs {
	public class MyUtils {
		public static float CustomBrightness( Color c ) {
			float b = XnaColorHelpers.Brightness( c );
			return ((3f * b) + ((float)c.A / 255f)) / 4f;
		}


		public static Color CustomBlend( Color c1, Color c2 ) {
			float scale2 = (float)c2.A / 255f;

			c2.R = (byte)((float)c2.R * scale2);
			c2.G = (byte)((float)c2.G * scale2);
			c2.B = (byte)((float)c2.B * scale2);

			//int high = c2.R > c2.G ? c2.R : c2.G > c2.B ? c2.G : c2.B;
			int high = ((int)c2.R + (int)c2.G + (int)c2.B) / 3;

			int r = (c1.R - high) + c2.R;
			int g = (c1.G - high) + c2.G;
			int b = (c1.B - high) + c2.B;

			c1.R = (byte)MathHelper.Clamp( r, 0, 255 );
			c1.G = (byte)MathHelper.Clamp( g, 0, 255 );
			c1.B = (byte)MathHelper.Clamp( b, 0, 255 );

			return c1;
		}
	}
}
