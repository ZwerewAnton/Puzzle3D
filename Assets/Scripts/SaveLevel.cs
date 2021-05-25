using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public static class SaveLevel
{
    private static string _folderPath = Path.Combine(Application.persistentDataPath, "saves");
    public static int levelID;

    private static LevelSaver CreateSaveObject(List <Detail> _allDetails)
    {
        LevelSaver level = new LevelSaver();
        bool isInstalled;
        float installedDetailsCount = 0;
        foreach(Detail detail in _allDetails)
        {
            isInstalled = true;
            DetailSaver detailSav = new DetailSaver();

            detailSav.detailName = detail.name;
            detailSav.currentCount = detail.CurrentCount;

            List<PointParentConnectorSaver> pPCSaverList = new List<PointParentConnectorSaver>();
            detailSav.parentList = pPCSaverList;

            foreach(var pointPC in detail.points)
            {
                PointParentConnectorSaver pPCSaver = new PointParentConnectorSaver();

                pPCSaver._isInstalled = pointPC.IsInstalled;
                if(!pointPC.IsInstalled || !detail.isRoot)
                {
                    isInstalled = false;
                }

                pPCSaverList.Add(pPCSaver);
            }
            if(isInstalled)
            {
                installedDetailsCount++;
            }
            level.detailList.Add(detailSav);
        }
        level.percent = Mathf.Round((installedDetailsCount/_allDetails.Count)*100);
        return level;
    }

    public static void SaveGame()
    {
        LevelSaver level = CreateSaveObject(LevelContainer.currentLevelContainer.GetCurrentLevelDetails());
        BinaryFormatter bf = new BinaryFormatter();
        if(!Directory.Exists(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
        }
        FileStream file = File.Create(GetSavePath(levelID));
        bf.Serialize(file, level);
        file.Close();
        SavePercent(levelID, LevelContainer.currentLevelContainer.GetPercent());
    }

    public static bool LoadGame()
    { 
        if (Directory.Exists(_folderPath) && File.Exists(GetSavePath(levelID)))
        {
            List<Detail> loadDetailList  = LevelContainer.currentLevelContainer.GetCurrentLevelDetails();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(GetSavePath(levelID), FileMode.Open);
            LevelSaver save = (LevelSaver)bf.Deserialize(file);
            file.Close();

            foreach(var detail in save.detailList)
            {
                foreach(var detailContainer in loadDetailList)
                {
                    if(detailContainer.name == detail.detailName)
                    {
                        detailContainer.CurrentCount = detail.currentCount;
                        for(int i = 0; i < detailContainer.points.Count; i++)
                        {
                            if( detail.parentList[i]._isInstalled)
                            {
                                detailContainer.points[i].Install();
                            }
                        }
                    }
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void DeleteSaveFile()
    {
        if (Directory.Exists(_folderPath) && File.Exists(GetSavePath(levelID)))
        {
            File.Delete(GetSavePath(levelID));
        }
    }

    private static string GetSavePath(int levelID)
    {
        string saveFile = levelID.ToString() + "_gamesave.save";
        return Path.Combine(_folderPath + saveFile);
    }

    private static void SavePercent(int levelID, float percent)
    {
        string key = PropertiesStorage.GetPercentKey() + levelID.ToString();
        PlayerPrefs.SetFloat(key, percent);
        PlayerPrefs.Save();
    }
    public static float[] LoadPercents()
    {
        int levelCount = LevelContainer.currentLevelContainer.GetLevelCount();
        float[] percents = new float[levelCount - 1];
        string key;
        for(int i = 0; i < levelCount; i++)
        {
            key = PropertiesStorage.GetPercentKey() + i.ToString();
            if(PlayerPrefs.HasKey(key)){
                percents[i] = PlayerPrefs.GetInt(key);
            }
            else
            {
                PlayerPrefs.SetFloat(key, 0f);
                PlayerPrefs.Save();
            }
        }
        return percents;
    }

}
