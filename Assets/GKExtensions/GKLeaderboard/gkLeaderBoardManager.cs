using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using gk;
using UnityEngine.SocialPlatforms;
using System;

#if UNITY_ANDROID
using GooglePlayGames;
#endif

namespace gk
{
	[AddComponentMenu("GK/LeaderBoard/gkLeaderBoardManager")]
	public class gkLeaderBoardManager : MonoBehaviour
	{
		public class ScoreReport
		{
			public string leaderboardUnityID;
			public int score;
		}
		
		public class AchievementReport
		{
			public string achievementUnityID;
		}
		
		public List<LeaderBoardID> leaderBoards;

		public List<AchievementID> achievements;
			
		static private gkLeaderBoardManager ms_oInstance;
		
		private Dictionary<string, LeaderBoardID> m_oLeaderBoardByID = new Dictionary<string, LeaderBoardID>();
		
		private Dictionary<string, ScoreReport> m_oScoreReportByLeaderBoardUnityID = new Dictionary<string, ScoreReport>();
		
		private Dictionary<string, AchievementID> m_oAchievementByID = new Dictionary<string, AchievementID>();
		
		private Dictionary<string, AchievementReport> m_oAchievementReportByLeaderboardUnityID = new Dictionary<string, AchievementReport>();
		
		private bool m_bCanPostAchievements;
		
		private bool m_bAuthenticationInProgress;
		
		static public gkLeaderBoardManager Instance
		{
			get
			{
				return ms_oInstance;
			}
		}
		
		public void ShowLeaderBoards()
		{
			Debug.Log("Authenticated ? : " + Social.localUser.authenticated);
			if(Social.localUser.authenticated == false)
			{
				StartAuthentication(OnAuthenticationToShowLeaderboard);
				return;
			}
			
			Social.ShowLeaderboardUI();
		}
		
		public void ShowAchievements()
		{
			if(Social.localUser.authenticated == false)
			{
				StartAuthentication(OnAuthenticationToShowAchievements);
				return;
			}
			
			Social.ShowAchievementsUI();
		}
		
		public void ReportScore(string leaderboardUnityID, int a_iScore)
		{
			m_oScoreReportByLeaderBoardUnityID.Remove(leaderboardUnityID);
			
			ScoreReport oScoreReport = new ScoreReport();
			oScoreReport.leaderboardUnityID = leaderboardUnityID;
			oScoreReport.score = a_iScore;
			
			if(TryToReport(oScoreReport) == false)
			{	
				m_oScoreReportByLeaderBoardUnityID.Add(leaderboardUnityID, oScoreReport);
			}
		}
		
		public void ReportAchievement(string achievementUnityID)
		{
			m_oAchievementReportByLeaderboardUnityID.Remove(achievementUnityID);
			
			AchievementReport oReport = new AchievementReport();
			oReport.achievementUnityID = achievementUnityID;
			
			if(TryToReport(oReport) == false)
			{	
				m_oAchievementReportByLeaderboardUnityID.Add(achievementUnityID, oReport);
			}
		}
		
		private void Awake()
		{
			if(ms_oInstance == null)
			{
				ms_oInstance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
				return;
			}
			
			#if UNITY_ANDROID && !UNITY_EDITOR
			//Social.Active = new UnityEngine.SocialPlatforms.GPGSocial();
			
			// recommended for debugging
			PlayGamesPlatform.DebugLogEnabled = true;
			// Activate the Google Play Games platform
			PlayGamesPlatform.Activate();
			#endif
			FillLeaderBoardByID();
			FillAchievementID();
			StartAuthentication(OnAuthentication);
		}
		
		private void FillLeaderBoardByID()
		{
			foreach(LeaderBoardID rLeaderBoardID in leaderBoards)
			{
				m_oLeaderBoardByID.Add(rLeaderBoardID.unityID, rLeaderBoardID);
			}
		}
		
		private void FillAchievementID()
		{
			foreach(AchievementID rAchievementID in achievements)
			{
				m_oAchievementByID.Add(rAchievementID.unityID, rAchievementID);
			}
		}
		
		private void StartAuthentication(Action<bool> a_rOnAuthentication)
		{
			if(m_bAuthenticationInProgress)
			{
				return;
			}
			m_bAuthenticationInProgress = true;
			Debug.Log("StartAuthentication");
			Social.localUser.Authenticate(a_rOnAuthentication);
		}
		
		private void OnAuthenticationToShowLeaderboard(bool a_bSuccess)
		{
			OnAuthenticationBeforeUsingLeaderboard(a_bSuccess, "Leaderboards unavailable");
		}
		
		private void OnAuthenticationToShowAchievements(bool a_bSuccess)
		{
			OnAuthenticationBeforeUsingLeaderboard(a_bSuccess, "Achievements unavailable");
		}
		
		private void OnAuthenticationBeforeUsingLeaderboard(bool a_bSuccess, string a_rErrorMessage)
		{
			bool bAutoReCall = m_bAuthenticationInProgress == false;
			
			Debug.Log("OnAuthenticationBeforeUsingLeaderboard : " + a_bSuccess + ". If error message : " + a_rErrorMessage);
			Social.LoadAchievements(OnLoadAchievements);
			OnAuthentication(a_bSuccess);
			
			if(bAutoReCall)
			{
				return;
			}

			#if nativePopUp
			if(a_bSuccess == false)
			{
				gkNativePopUpManager.Instance.ShowPopUp(a_rErrorMessage, "");
			}
			#endif
		}
		
		private void OnAuthentication(bool a_bSuccess)
		{
			Debug.Log("Authentication : " + a_bSuccess);
			m_bAuthenticationInProgress = false;
			Social.LoadAchievements(OnLoadAchievements);
			if(a_bSuccess)
			{
				foreach(ScoreReport rScoreReport in m_oScoreReportByLeaderBoardUnityID.Values)
				{
					TryToReport(rScoreReport);
				}
				m_oScoreReportByLeaderBoardUnityID.Clear();
			}
		}
		
		private void OnLoadAchievements(IAchievement[] a_rAchievements)
		{
			m_bCanPostAchievements = true;
			foreach(IAchievement rAchievement in a_rAchievements)
			{
				Debug.Log(rAchievement);
			}
			
			foreach(AchievementReport rAchievementReport in m_oAchievementReportByLeaderboardUnityID.Values)
			{
				TryToReport(rAchievementReport);
			}
			m_oAchievementReportByLeaderboardUnityID.Clear();
		}
		
		private bool TryToReport(ScoreReport a_rScoreReport)
		{
			if(Social.localUser.authenticated == false)
			{
				return false;
			}

			string oLeaderBoardID = m_oLeaderBoardByID[a_rScoreReport.leaderboardUnityID].CurrentPlatformID;
			
			Debug.Log("Report Score : " + a_rScoreReport.score + " on " + oLeaderBoardID);
			#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
			Social.ReportScore(a_rScoreReport.score, oLeaderBoardID, OnReportScoreResult);
			#endif		
			return true;
		}
		
		private bool TryToReport(AchievementReport a_rAchievementReport)
		{
			if(Social.localUser.authenticated == false || m_bCanPostAchievements == false)
			{
				return false;
			}

			string oID = m_oAchievementByID[a_rAchievementReport.achievementUnityID].CurrentPlatformID;
			
			Debug.Log("Report Achievement : " + oID);
			#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
			Social.ReportProgress(oID, 100.0, OnReportProgressResult);
			#endif		
			return true;
		}
		
		private void OnReportProgressResult(bool a_bSuccess)
		{
			Debug.Log("Report Progress : " + a_bSuccess);
		}
		
		private void OnReportScoreResult(bool a_bSuccess)
		{
			Debug.Log("Report Score : " + a_bSuccess);
		}
	}
}