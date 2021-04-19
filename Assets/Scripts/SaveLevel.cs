using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public static class SaveLevel
{
    //public ObjectMagnet objectMagnet;
    //public LevelContainer levelContainer;
    //List<Detail> loadDetailList;
    //byte Progress;
    //public List<Level> levelList = new List<Level>();
/*     private void Awake() {
        
        loadDetailList  = new List<Detail>(levelContainer._currentLevel);
    } */
    //public static LevelContainer savedGame = new LevelContainer();

    private static string folderPath = Path.Combine(Application.persistentDataPath, "saves");
    private static string savePath = Path.Combine(folderPath + "gamesave.save");

    private static Level CreateSaveObject(List <Detail> _allDetails)
    {
        Level level = new Level();
        //List<Detail> _allDetails = objectMagnet.detailsList;
        

        foreach(var detail in _allDetails)
        {
            DetailSaver detailSav = new DetailSaver();

            detailSav.detailName = detail.name;
            detailSav._currentCount = detail.CurrentCount;

            List<PointParentConnectorSaver> pPCSaverList = new List<PointParentConnectorSaver>();
            detailSav.parentList = pPCSaverList;

            foreach(var pointPC in detail.points)
            {
                PointParentConnectorSaver pPCSaver = new PointParentConnectorSaver();

                pPCSaver._isInstalled = pointPC.IsInstalled;

                pPCSaverList.Add(pPCSaver);
            }
            
            level.detailList.Add(detailSav);
        }

        return level;
    }

    public static void SaveGame()
    {
        //savedGames.Add(Game.current);
        //Debug.Log(LevelContainer.currentLevel);
        Level level = CreateSaveObject(LevelContainer.currentLevelContainer.GetCurrentLevel());
        BinaryFormatter bf = new BinaryFormatter();
        if(!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        FileStream file = File.Create(savePath);
        bf.Serialize(file, level);
        file.Close();
    }

    public static bool LoadGame()
    { 
        if (Directory.Exists(folderPath) && File.Exists(savePath))
        {
            List<Detail> loadDetailList  = LevelContainer.currentLevelContainer.GetCurrentLevel();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            Level save = (Level)bf.Deserialize(file);
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
            //Debug.Log(SaveLevel.savedGame.GetLevel().Count);
            //SaveLevel.savedGame._currentLevel = loadDetailList;
        }
        else
        {
            Debug.Log("No game saved!");
            return false;
        }
    }

    public static void DeleteSaveFile()
    {
        //string path = Application.persistentDataPath + "/saves/";
        //DirectoryInfo directory = new DirectoryInfo(folderPath);
        //directory.Delete(true);
        //Directory.CreateDirectory(folderPath);
        if (Directory.Exists(folderPath) && File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }
    
/*     public static List<Detail> GetLoadDetailList()
    {
        LoadGame();
        return loadDetailList;
    } */
    
}
