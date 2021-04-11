using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveLevel : MonoBehaviour
{
    public ObjectMagnet objectMagnet;
    //byte Progress;
    //public List<Level> levelList = new List<Level>();
    private Level CreateSaveObject()
    {
        Level level = new Level();
        List<Detail> _allDetails = objectMagnet.detailsList;
        

        foreach(var detail in _allDetails)
        {
            
            DetailSaver detailSav = new DetailSaver();
            Texture2D tex = detail.icon.texture;

            detailSav.detailName = detail.name;
            detailSav._count = detail.count;

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

    public void SaveGame()
    {
        Level level = CreateSaveObject();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, level);
        file.Close();
    }

    public void LoadGame()
    { 
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            //Clear
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Level save = (Level)bf.Deserialize(file);
            file.Close();

            foreach(var detail in save.detailList)
            {
                Debug.Log(detail.detailName);
            } 

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }
    
    
}
