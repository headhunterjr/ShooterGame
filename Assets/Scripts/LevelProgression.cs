using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
	public static class LevelProgression
	{
		private static int totalLevels = SceneManager.sceneCountInBuildSettings - 1;
		public static void CompleteLevel(int levelIndex)
		{
			int unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);
			if (levelIndex == unlockedLevels && levelIndex < totalLevels)
			{
				PlayerPrefs.SetInt("UnlockedLevels", unlockedLevels + 1);
			}
		}
	}
}
