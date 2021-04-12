using System;
using System.Collections.Generic;
using System.IO;

namespace ExamplesApp.Method
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
            Achievement newAchievement = LoadAchievement(newAchievementId);
            if (newAchievement == null) throw new Exception("New achievement does not exist in the registry.");
            
            List<Achievement> unlockedAchievements = LoadUserAchievements(userId, newAchievement);

            CheckIfPrerequisitesUnlocked(newAchievement, unlockedAchievements);

            SaveAchievement(userId, newAchievement);
        }

        private void SaveAchievement(int userId, Achievement newAchievement)
        {
            //Save new achievement to storage
            string newAchievementStorageFormat = newAchievement.Name + ":" + newAchievement.ImagePath + "\n";
            File.AppendAllText(_achievementStorageLocation + userId + ".csv", newAchievementStorageFormat);
        }

        private static void CheckIfPrerequisitesUnlocked(Achievement newAchievement, List<Achievement> unlockedAchievements)
        {
            //Check if user has prerequisite achievements unlocked
            foreach (var prerequisiteAchievement in newAchievement.PrerequisiteAchievementNames)
            {
                foreach (var a in unlockedAchievements)
                {
                    if (a.Name.Equals(prerequisiteAchievement))
                    {
                        throw new InvalidOperationException("Prerequisite achievement " + prerequisiteAchievement +
                                                            " not completed.");
                    }
                }
            }
        }

        private List<Achievement> LoadUserAchievements(int userId, Achievement newAchievement)
        {

            //Load unlocked achievements for user
            List<Achievement> unlockedAchievements = new List<Achievement>();
            string[] achievements = File.ReadAllLines(_achievementStorageLocation + userId + ".csv");
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

            return unlockedAchievements;
        }

        private Achievement LoadAchievement(int id)
        {
            //Load data for new achievement
            Achievement newAchievement = null;
            string[] allAchievements = File.ReadAllLines(_achievementStorageLocation + "allAchievements.csv");
            foreach (var achievement in allAchievements)
            {
                string[] achievementElements = achievement.Split(":");
                if (!achievementElements[0].Equals(id.ToString())) continue;
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

            return newAchievement;
        }
    }
}
