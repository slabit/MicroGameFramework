using UnityEngine;
using System.Collections;

using MicroGameFramework;

namespace MicroGameFrameworkSample
{
	[AddComponentMenu("MicroGameFrameworkSample/ButtonPoint")]
	public class ButtonPoint : MonoBehaviour
	{
		public void OnClick()
		{
			ScoreManager.Instance.IncrementScore();
		}
	}
}