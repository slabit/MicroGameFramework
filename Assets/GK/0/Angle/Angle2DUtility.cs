using UnityEngine;

namespace gk
{
	public static class Angle2DUtility
	{
		public static float DistanceBetweenAngleSigned(float angleFrom, float angleTo)
		{
			return ExpressAngleBetween180AndMinus180(angleTo) - ExpressAngleBetween180AndMinus180(angleFrom);
		}

		public static float ExpressAngleBetween180AndMinus180(float angle)
		{
			float angleValue = Mathf.Abs(angle);
			angleValue %= 360.0f; 
			if(angleValue > 180.0f)
			{
				angleValue = angleValue - 360.0f;
			}
			
			return angleValue * Mathf.Sign(angle);
		}

		public static float ExpressAngleBetween0AndMinus360(float angle)
		{
			float angleValue = Mathf.Abs(angle);
			angleValue %= 360.0f; 
			if(angleValue > 180.0f)
			{
				angleValue = angleValue - 360.0f;
			}
			if(angle < 0.0f)
			{
				angleValue = 360.0f - angleValue;
			}
			
			return angleValue;
		}
	}
}
