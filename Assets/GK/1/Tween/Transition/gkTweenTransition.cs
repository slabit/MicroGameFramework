using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	// Tween transition
	public enum ETweenTransition
	{
		MoveBy,
		MoveTo,
		ScaleBy,
		ScaleTo
	}
	
	[AddComponentMenu("GKTween/Transition/gkTweenTransition")]
	// gkTweenTransition
	public class gkTweenTransition : gkTransition
	{
		// Ease type
		public gkTween.EaseType easeType = gkTween.EaseType.linear;
		
		// The transition speed
		private float m_fTransitionSpeed = 0.0f;
		
		// The transition time
		private float m_fTransitionTime = 0.0f;
		
		// The Tween component
		private List<gkTween> m_oTweens = new List<gkTween>();
		
		// Create transition
		public static gkTweenTransition CreateTransition(GameObject a_rGameObject)
		{
			return gkTransition.CreateTransition<gkTweenTransition>(a_rGameObject);
		}
		
		// Move By
		public void InitializeTransition_MoveBy(float a_fStartPercent, Vector3 a_f3Movement, float a_fDuration)
		{
			InitializeTransition(ETweenTransition.MoveBy, a_fStartPercent, gkTween.Hash("amount", a_f3Movement, "time", a_fDuration));
		}
		
		// Move To
		public void InitializeTransition_MoveTo(float a_fStartPercent, Vector3 a_f3Position, float a_fDuration)
		{
			InitializeTransition(ETweenTransition.MoveTo, a_fStartPercent, gkTween.Hash("position", a_f3Position, "time", a_fDuration));
		}
		
		// Move Between
		public void InitializeTransition_MoveBetween(float a_fStartPercent, Vector3 a_f3PositionA, Vector3 a_f3PositionB, float a_fDuration)
		{
			InitializeTransition(ETweenTransition.MoveTo, a_fStartPercent, gkTween.Hash("path", new Vector3[]{a_f3PositionA, a_f3PositionB}, "movetopath", false, "time", a_fDuration));
		}
		
		// Scale By
		public void InitializeTransition_ScaleBy(float a_fStartPercent, float a_fScale, float a_fDuration)
		{
			InitializeTransition_ScaleBy(a_fStartPercent, Vector3.one * a_fScale, a_fDuration);
		}
		
		public void InitializeTransition_ScaleBy2D(float a_fStartPercent, float a_fScale, float a_fDuration)
		{
			InitializeTransition_ScaleBy(a_fStartPercent, new Vector3(a_fScale, a_fScale, 1.0f), a_fDuration);
		}
		
		public void InitializeTransition_ScaleBy2D(float a_fStartPercent, Vector2 a_f2Scale, float a_fDuration)
		{
			InitializeTransition_ScaleBy(a_fStartPercent, new Vector3(a_f2Scale.x, a_f2Scale.y, 1.0f), a_fDuration);
		}
		
		public void InitializeTransition_ScaleBy(float a_fStartPercent, Vector3 a_f3Scale, float a_fDuration)
		{
			InitializeTransition(ETweenTransition.ScaleBy, a_fStartPercent, gkTween.Hash("amount", a_f3Scale, "time", a_fDuration));
		}
		
		// Scale To
		public void InitializeTransition_ScaleTo(float a_fStartPercent, float a_fScale, float a_fDuration)
		{
			InitializeTransition_ScaleTo(a_fStartPercent, Vector3.one * a_fScale, a_fDuration);
		}
		
		public void InitializeTransition_ScaleTo2D(float a_fStartPercent, float a_fScale, float a_fDuration)
		{
			InitializeTransition_ScaleTo(a_fStartPercent, new Vector3(a_fScale, a_fScale, 1.0f), a_fDuration);
		}
		
		public void InitializeTransition_ScaleTo2D(float a_fStartPercent, Vector2 a_f2Scale, float a_fDuration)
		{
			InitializeTransition_ScaleTo(a_fStartPercent, new Vector3(a_f2Scale.x, a_f2Scale.y, 1.0f), a_fDuration);
		}
		
		public void InitializeTransition_ScaleTo(float a_fStartPercent, Vector3 a_f3Scale, float a_fDuration)
		{
			InitializeTransition(ETweenTransition.ScaleTo, a_fStartPercent, gkTween.Hash("scale", a_f3Scale, "time", a_fDuration));
		}
		
		// Initialize transition
		public void InitializeTransition(ETweenTransition a_eTweenTransition, float a_fStartPercent, Hashtable a_rTweenArguments)
		{
			InitializeTransition(a_eTweenTransition, a_fStartPercent, a_rTweenArguments, gameObject);
		}
		
		// Initialize transition
		public void InitializeTransition(ETweenTransition a_eTweenTransition, float a_fStartPercent, Hashtable a_rTweenArguments, GameObject a_rTweenTarget)
		{
			m_oTweens.Clear();
			LaunchTween(a_eTweenTransition, a_rTweenTarget, a_rTweenArguments);
			
			InitializeTweenTransitionBase(a_fStartPercent);
		}
		
		// Initialize transition
		public void InitializeTransition(ETweenTransition a_eTweenTransition, float a_fStartPercent, Hashtable a_rTweenArguments, List<GameObject> a_rTweenTargets)
		{
			m_oTweens.Clear();
			foreach(GameObject rTweenTarget in a_rTweenTargets)
			{
				LaunchTween(a_eTweenTransition, rTweenTarget, a_rTweenArguments);
			}
			
			InitializeTweenTransitionBase(a_fStartPercent);
		}
	
		// Initialize transition
		private void InitializeTweenTransitionBase(float a_fStartPercent)
		{
			gkTween rTween = m_oTweens[0];
			float fDuration = rTween.time;
			InitializeTransitionBase(fDuration);
			
			InitializeAtStartPercent(a_fStartPercent);
		}
		
		// Launch Tween
		private void LaunchTween(ETweenTransition a_eTweenTransition, GameObject a_rTweenTarget, Hashtable a_rTweenArguments)
		{
			a_rTweenArguments.Add("easetype", easeType);
			gkTween oTween = LaunchTween(a_rTweenTarget, a_rTweenArguments, a_eTweenTransition);
			oTween.Initialize();
			m_oTweens.Add(oTween);
		}
		
		// Launch Tween
		private gkTween LaunchTween(GameObject a_rTweenTarget, Hashtable a_rTweenArguments, ETweenTransition a_eTweenTransition)
		{
			switch(a_eTweenTransition)
			{		
				case ETweenTransition.MoveBy:
				{
					return gkTween.MoveBy(a_rTweenTarget, a_rTweenArguments);
				}
				
				case ETweenTransition.MoveTo:
				{
					return gkTween.MoveTo(a_rTweenTarget, a_rTweenArguments);
				}
				
				case ETweenTransition.ScaleBy:
				{
					return gkTween.ScaleBy(a_rTweenTarget, a_rTweenArguments);
				}
				
				case ETweenTransition.ScaleTo:
				{
					return gkTween.ScaleTo(a_rTweenTarget, a_rTweenArguments);
				}
			}
			return null;
		}
		
		// Get transition time
		protected override float GetTransitionTime()
		{
			return m_fTransitionTime;
		}
		
		// Set transition time
		protected override void SetTransitionTime(float a_fTime)
		{
			m_fTransitionTime = a_fTime;
			
			float fNormalizedTime = 0.0f;
			if(transitionDuration == 0.0f)
			{
				if(a_fTime <= 0.0f)
				{
					fNormalizedTime = 0.0f;
				}
				else
				{
					fNormalizedTime = 1.0f;
				}
			}
			else
			{
				fNormalizedTime = a_fTime/transitionDuration;
			}
			
			foreach(gkTween rTween in m_oTweens)
			{
				rTween.SetTweenPercent(fNormalizedTime);
			}
		}
		
		// Get transition speed
		protected override float GetTransitionSpeed()
		{
			return m_fTransitionSpeed;
		}
		
		// Set transition speed
		protected override void SetTransitionSpeed(float a_fSpeed)
		{
			m_fTransitionSpeed = a_fSpeed;
		}
		
		// Update transition time
		protected override void UpdateTransitionTime(float a_fDeltaTime)
		{
			m_fTransitionTime += a_fDeltaTime;
			SetTransitionTime(m_fTransitionTime);
		}
		
		// On Destroy
		private void OnDestroy()
		{
			foreach(gkTween rTween in m_oTweens)
			{
				Destroy(rTween);
			}
		}
		
		// Initialize at start percent
		private void InitializeAtStartPercent(float a_fStartPercent)
		{
			TransitionPercent = a_fStartPercent;
		}
	}
}