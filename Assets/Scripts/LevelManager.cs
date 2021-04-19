using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Detail ground;
    public LevelContainer levelContainer;
    private bool _isInstantiate;
    private List<Detail> _allDetails;
    private List<Detail> _availableDetails;
    private List<Detail> _installedDetails;
    private List<Point> _connectionPoints;
    private List<GameObject> _connectionObjects;
    private List<PointParentConnector> _allPoints;
    private Detail _instDetail;

    private void Awake()
    {
        _allDetails = levelContainer.GetLoadLevel();
        

        _connectionPoints = new List<Point>();
        _connectionObjects = new List<GameObject>();
        _installedDetails = new List<Detail>();
        _availableDetails = new List<Detail>();

        Debug.Log(_allDetails.Count);
        StartDetailInstance();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private List<Detail> GetOpenDetails(Detail detail)
    {
        List<Detail> list = new List<Detail>();
        foreach(var det in _allDetails)
        {
            foreach(var pointParConn in det.points)
            {
                if(!pointParConn.IsInstalled)
                {
                    foreach(var parentDet in pointParConn.parentList)
                    {
                        if(parentDet.parentDetail == detail)
                        {
                            list.Add(det);
                        }
                    }
                }
            }
        }
        return list;
    }
    public void StartDetailInstance()
    {
        GameObject _instObject;
        bool isDetailInstalled;
        
        _installedDetails.Add(ground);
        _availableDetails.AddRange(GetOpenDetails(ground));

        foreach(var detail in _allDetails)
        {
            foreach(var pPC in detail.points)
            {
                isDetailInstalled = false;
                if(pPC.IsInstalled)
                {
                    _instObject = detail._prefab.transform.GetChild(0).gameObject;
                    Instantiate(_instObject, pPC.point.Position, Quaternion.Euler(pPC.point.Rotation));
                    foreach(Detail installedDetail in _installedDetails)
                    {
                        if(detail == installedDetail)
                        {
                            isDetailInstalled = true;
                            break;
                        }
                    }
                    if(!isDetailInstalled)
                    {
                        _installedDetails.Add(detail);
                        _availableDetails.AddRange(GetOpenDetails(detail));
                    }
                }
            }
        }
    }


}
