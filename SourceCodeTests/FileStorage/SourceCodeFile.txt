using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExamplesApp.Method.Violation
{
    class Achievement
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public List<string> PrerequisiteAchievementNames { get; set; }
    }

    class AchievementService
    {
        private readonly string _achievementStorageLocation = "C:/MyGame/Storage/Achievements/";
        
        /// <summary>
        /// 1) Discover (e.g., using Google) the Extract Method command in your IDE.
        /// 2) Using the command, extract multiple methods from the AwardAchievement method.
        /// 3) For each method, define the most appropriate name.
        /// </summary>
        public void AwardAchievement(int userId, int newAchievementId)
        {
            //Load data for new achievement
            Achievement newAchievement = null;
            string[] allAchievements = File.ReadAllLines(_achievementStorageLocation + "allAchievements.csv");
            foreach (var achievement in allAchievements)
            {
                string[] achievementElements = achievement.Split(":");
                if(!achievementElements[0].Equals(newAchievementId.ToString())) continue;
                newAchievement = new Achievement();
                newAchievement.Name = achievementElements[0];
                newAchievement.ImagePath = achievementElements[1];
                newAchievement.PrerequisiteAchievementNames = new List<string>();
                //Add ids of prerequisite achievements
                for (int i = 2; i < achievementElements.Length; i++)
                {
                    newAchievement.PrerequisiteAchievementNames.Add(achievementElements[i]);
                }
            }

            if(newAchievement == null) throw new Exception("New achievement does not exist in the registry.");

            //Load unlocked achievements for user
            string[] achievements = File.ReadAllLines(_achievementStorageLocation + userId + ".csv");
            List<Achievement> unlockedAchievements = new List<Achievement>();
            foreach (var storedAchievement in achievements)
            {
                string[] achievementElements = storedAchievement.Split(":");
                Achievement a = new Achievement();
                a.Name = achievementElements[0];
                a.ImagePath = achievementElements[1];
                //Check if newAchievement is already unlocked.
                if (a.Name.Equals(newAchievement.Name) && a.ImagePath.Equals(newAchievement.ImagePath))
                {
                    throw new InvalidOperationException("Achievement " + newAchievement.Name + " is already unlocked!");
                }
                unlockedAchievements.Add(a);
            }
            
            //Check if user has prerequisite achievements unlocked
            foreach (var prerequisiteAchievement in newAchievement.PrerequisiteAchievementNames)
            {
                bool foundAchievement = false;
                foreach (var a in unlockedAchievements)
                {
                    if (a.Name.Equals(prerequisiteAchievement))
                    {
                        foundAchievement = true;
                        break;
                    }
                }
                if(!foundAchievement) throw new InvalidOperationException("Prerequisite achievement " + prerequisiteAchievement + " not completed.");
            }

            //Save new achievement to storage
            string newAchievementStorageFormat = newAchievement.Name + ":" + newAchievement.ImagePath + "\n";
            File.AppendAllText(_achievementStorageLocation + userId + ".csv", newAchievementStorageFormat);
        }
    }
}
