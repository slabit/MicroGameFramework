using UnityEngine;
using System.Collections.Generic;

using UnityEditor;

namespace AnimationClipFixer
{
	public static class AnimationClipFixerMenuItem
	{
		[MenuItem("AnimationClipFixer/Create Animation Clip Visualisation")]
		static void CreateAnimationClipVisualisation()
		{
			AnimationClipFixerUtility.CreateAnimationClipVisualisation(GetFirstSelectedAnimationClip());
		}

		[MenuItem("AnimationClipFixer/Create Animation Clip Visualisation", true)]
		static bool CanCreateAnimationClipVisualisation()
		{
			return GetFirstSelectedAnimationClip() != null;
		}

		static AnimationClip GetFirstSelectedAnimationClip()
		{
			Object[] clips = Selection.GetFiltered(typeof(AnimationClip), SelectionMode.Unfiltered);

			if(clips.Length <= 0)
				return null;

			return clips[0] as AnimationClip;
		}
	}
}
