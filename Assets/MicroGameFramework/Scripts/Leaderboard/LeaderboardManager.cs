using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MicroGameFramework
{
	[AddComponentMenu("MicroGameFramework/LeaderboardManager")]
	public class LeaderboardManager : MonoBehaviour
	{
		public string leaderboard_HighScore = "HighScore";

		public string achievement_Score10 = "Score10";
		public string achievement_Score20 = "Score20";
		public string achievement_Score30 = "Score30";
		public string achievement_Score50 = "Score50";
		public string achievement_Score100 = "Score100";

		static LeaderboardManager instance;
		
		public static LeaderboardManager Instance
		{
			get
			{
				return instance;
			}
		}

		public void ShowLeaderboard()
		{
			gk.gkLeaderBoardManager.Instance.ShowLeaderBoards();
		}

		public void ShowAchievements()
		{
			gk.gkLeaderBoardManager.Instance.ShowAchievements();
		}

		public void PostScore(int score)
		{
			PostScore(leaderboard_HighScore, score);

			CheckForScoreAchievement(score);
		}

		void Awake()
		{
			if(instance != null)
			{
				Destroy(gameObject);
				return;
			}
			
			instance = this;
		}

		void CheckForScoreAchievement(int score)
		{
			if(score >= 10)
			{
				ReportScoreAchievement(achievement_Score10);
			}

			if(score >= 20)
			{
				ReportScoreAchievement(achievement_Score20);
			}

			if(score >= 30)
			{
				ReportScoreAchievement(achievement_Score30);
			}

			if(score >= 50)
			{
				ReportScoreAchievement(achievement_Score50);
			}

			if(score >= 100)
			{
				ReportScoreAchievement(achievement_Score100);
			}
		}

		void ReportScoreAchievement(string achievementID)
		{
			ReportAchievement(achievementID);
		}

		public void PostScore(string leaderboardID, int score)
		{
			gk.gkLeaderBoardManager.Instance.ReportScore(leaderboardID, score);
		}

		public void ReportAchievement(string achievementID)
		{
			gk.gkLeaderBoardManager.Instance.ReportAchievement(achievementID);
		}
	}
}
