using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	public static class gkColorUtility
	{
		public static Color SetAlpha(Color a_oColor, float a_fAlpha)
		{
			a_oColor.a = a_fAlpha;
			return a_oColor;
		}
		
		public static void SetAlpha(Renderer a_rRenderer, float a_fAlpha)
		{
			SetAlpha(a_rRenderer.material, a_fAlpha);
		}
		
		public static void SetAlpha(Material a_rMaterial, float a_fAlpha)
		{
			a_rMaterial.color = ChangeAlpha(a_rMaterial.color, a_fAlpha);
		}
		
		public static Color ChangeAlpha(Color a_oColor, float a_fAlpha)
		{
			a_oColor.a = a_fAlpha;
			return a_oColor;
		}
	}
}
