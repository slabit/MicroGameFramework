using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace gk
{
	public static class gkTextMeshUtility
	{
		public static string ComputeHtmlColoredText(string a_oText, Color a_oColor)
		{
			return "<color=" + ConvertColorToHexadecimal(a_oColor) + ">" + a_oText + "</color>";
		}
		
		public static string ConvertColorToHexadecimal(Color a_oColor)
		{
			return "#"
					+ ConvertColorComponentToHexadecimal(a_oColor.r)
					+ ConvertColorComponentToHexadecimal(a_oColor.g)
					+ ConvertColorComponentToHexadecimal(a_oColor.b)
					+ ConvertColorComponentToHexadecimal(a_oColor.a);
		}
		
		private static string ConvertColorComponentToHexadecimal(float a_fColorComposanteToHexadecimal)
		{
			string oHexa = Mathf.FloorToInt(a_fColorComposanteToHexadecimal * 255).ToString("X").ToLower();
			int iLength = oHexa.Length;
			if(iLength == 0)
			{
				return "00";
			}
			else if(iLength == 1)
			{
				return "0" + oHexa;
			}
			else
			{
				return oHexa;
			}
		}
	}
}