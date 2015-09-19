using UnityEngine;

namespace gk
{
	public static class Vector2DUtility
	{
		public static Vector2 RandomDirection()
		{
			float randomAngleRad = Random.Range(0.0f, Mathf.PI);
			return DirectionFromAngleRad(randomAngleRad);
		}

		public static Vector2 DirectionFromAngleRad(float angleRad)
		{
			return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
		}
	}
}
