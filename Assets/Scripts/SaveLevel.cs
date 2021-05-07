using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public static class SaveLevel
{
    public static int levelID;
    private static string _folderPath = Path.Combine(Application.persistentDataPath, "saves");
    private static string _saveFile = levelID.ToString() + "_gamesave.save";
    private static string _savePath = Path.Combine(_folderPath + _saveFile);

    private static LevelSaver CreateSaveObject(List <Detail> _allDetails)
    {
        LevelSaver level = new LevelSaver();
        bool isInstalled;
        float installedDetailsCount = 0;
        //level.percent = _allDetails
        foreach(Detail detail in _allDetails)
        {
            isInstalled = true;
            DetailSaver detailSav = new DetailSaver();

            detailSav.detailName = detail.name;
            detailSav._currentCount = detail.CurrentCount;

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
        //savedGames.Add(Game.current);
        //Debug.Log(LevelContainer.currentLevel);
        LevelSaver level = CreateSaveObject(LevelContainer.currentLevelContainer.GetCurrentLevel());
        BinaryFormatter bf = new BinaryFormatter();
        if(!Directory.Exists(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
        }
        FileStream file = File.Create(_savePath);
        bf.Serialize(file, level);
        file.Close();
    }

    public static bool LoadGame()
    { 
        if (Directory.Exists(_folderPath) && File.Exists(_savePath))
        {
            List<Detail> loadDetailList  = LevelContainer.currentLevelContainer.GetCurrentLevel();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(_savePath, FileMode.Open);
            LevelSaver save = (LevelSaver)bf.Deserialize(file);
            file.Close();

            foreach(var detail in save.detailList)
            {
                foreach(var detailContainer in loadDetailList)
                {
                    if(detailContainer.name == detail.detailName)
                    {
                        detailContainer.CurrentCount = detail._currentCount;
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
            Debug.Log("Game Loaded");
            Debug.Log(loadDetailList.Count);
            return true;
        }
        else
        {
            Debug.Log("No game saved!");
            return false;
        }
    }

    public static void DeleteSaveFile()
    {
        if (Directory.Exists(_folderPath) && File.Exists(_savePath))
        {
            File.Delete(_savePath);
        }
    }

}
