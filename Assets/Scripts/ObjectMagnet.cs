using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectMagnet : MonoBehaviour
{
    public List<Detail> detailsList;
    public Camera cam;
    public float magnDist = 0.5f;
    public Material grayMaterial;
    public Vector3 objectOffset;
    public UnityEvent restartEvent;
    private GameObject target;
    private float mZCoord;
    private bool _isConnect;
    private bool _isInstantiate;

    private List<GameObject> instantiateDetailObjects;
    private List<Detail> _allDetails;
    private List<Point> _connectionPoints;
    private List<GameObject> _connectionObjects;
    private List<PointParentConnector> _allPoints;
    private Detail _instDetail;
    private Material _mainMaterial;
    private MeshRenderer meshRenderer;
    private Color _grayNearColor = new Color32(255, 255, 255, 150);
    private Color _grayFarColor = new Color32(255, 255, 255, 0);
    private float _lerp = 0;
    
    private GameObject _currentConnObject;
    private Transform _targetTransform;
    private Detail _ground;
    [SerializeField] private UnityEvent<Transform> _onInstantiateGround;
    [SerializeField] private ParticlePlayer particlePlayer;
    [SerializeField] private UILevelMenu levelMenu;
    
    private void Awake() 
    {
        _allDetails = LevelContainer.currentLevelContainer.GetLoadLevel();
        _ground = LevelContainer.currentLevelContainer.GetCurrentLevelGround();
        
        instantiateDetailObjects = new List<GameObject>();
        _connectionPoints = new List<Point>();
        _connectionObjects = new List<GameObject>();
        StartDetailInstance();
        if(IsEnd())
        {
            levelMenu.ShowHomeButton();
        }

        objectOffset.y = Screen.height * 0.1f;
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition + objectOffset;
        mousePoint.z = mZCoord;
        return cam.ScreenToWorldPoint(mousePoint);
    }
    private Vector3 GetTouchAsWorldPoint()
    {
        Touch touch = Input.GetTouch(0);
        //touch.fingerId;
        Vector3 touchData = Input.GetTouch(0).position;
        
        Vector3 touchPoint = touchData + objectOffset;
        touchPoint.z = mZCoord;
        return cam.ScreenToWorldPoint(touchPoint);
    }

    public void InstantiateObject(Detail instDetail)
    {
        _instDetail = instDetail;
        GameObject _instObject = instDetail._prefab.transform.GetChild(0).gameObject;

        _connectionPoints.Clear();
        _connectionObjects.Clear();

        _instObject.transform.position = Vector3.zero;
        mZCoord = cam.WorldToScreenPoint(_instObject.transform.position).z;

        if(Input.touchSupported)
        {
            target = Instantiate(_instObject, GetTouchAsWorldPoint(), Quaternion.identity);
        }
        else
        {
            target = Instantiate(_instObject, GetMouseAsWorldPoint(), Quaternion.identity);
        }
        _targetTransform = target.GetComponent<Transform>();

        _connectionPoints = _instDetail.GetAvaiablePoints();

        foreach(Point connectionPoint in _connectionPoints)
        {
            GameObject connectionObject = Instantiate(_instObject, connectionPoint.Position, Quaternion.Euler(connectionPoint.Rotation));
            _connectionObjects.Add(connectionObject);
            meshRenderer = connectionObject.GetComponent<MeshRenderer>();
            meshRenderer.material = grayMaterial;
            connectionObject.SetActive(false);
        }
        _isInstantiate = true;
    }

    public bool InstalOrDropObject()
    {
        _isInstantiate = false;

        foreach(GameObject connectionObject in _connectionObjects)
        {
            Destroy(connectionObject);
        }

        if (_isConnect)
        {
            Transform targetTransform = target.GetComponent<Transform>();
            foreach(PointParentConnector pointParConn in _instDetail.points)
            {
                float angle = Quaternion.Angle(targetTransform.rotation, Quaternion.Euler(pointParConn.point.Rotation));
                if(targetTransform.position == pointParConn.point.Position && angle == 0)
                {
                    pointParConn.Install();
                }
            }
            meshRenderer.material = _mainMaterial;
            instantiateDetailObjects.Add(target);
            target = null;
            return true;
        }
        else
        {
            Destroy(target);
            return false;
        }
    }

    private void Reset(List<Detail> detailsList)
    {
        foreach(Detail detail in detailsList)
        {
            detail.Reset();
        }
    }

    private void InstallDetail(Detail detail, Vector3 position, Vector3 rotation)
    {   
        foreach(PointParentConnector pointParConn in detail.points)
        {
            if(position == pointParConn.point.Position &&
                rotation == pointParConn.point.Rotation)
            {
                pointParConn.Install();
            }
        }
    }

    public bool IsLastDetail()
    {
        bool isLastDetail = _instDetail.IsLastDetail();
        if(isLastDetail)
        {
            _allDetails.Remove(_instDetail);
            
            if(IsEnd())
            {
                particlePlayer.Play();
                levelMenu.PlayEndClip();
                levelMenu.ShowHomeButton();
            }
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public bool IsEnd()
    {
        if(_allDetails.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private List<Detail> GetOpenDetails()
    {
        List<Detail> list = new List<Detail>();
        foreach(Detail allDetail in _allDetails)
        {
            foreach(PointParentConnector allDetailPPC in allDetail.points)
            {
                if(!allDetailPPC.IsInstalled)
                {
                    bool isOpen = true;
                    foreach(Parent allDetailParent in allDetailPPC.parentList)
                    {
                        foreach(PointParentConnector allDetailParentPPC in allDetailParent.GetAllPPC())
                        {
                            if(!allDetailParentPPC.IsInstalled){
                                isOpen = false;
                            }
                        }
                    }
                    if(isOpen)
                    {
                        list.Add(allDetail);
                    }
                }
            }
        }
        return list;
    }

    public void StartDetailInstance()
    {
        GameObject _instObject;
        List<Detail> clearDetailList = new List<Detail>();
        InstantiateGround(_ground);
        foreach(Detail detail in _allDetails)
        {
            bool isInstalled = true;
            foreach(PointParentConnector pointParentConnector in detail.points)
            {
                if(pointParentConnector.IsInstalled)
                {
                    _instObject = detail._prefab.transform.GetChild(0).gameObject;
                    _instObject = Instantiate(_instObject, pointParentConnector.point.Position, Quaternion.Euler(pointParentConnector.point.Rotation));
                    instantiateDetailObjects.Add(_instObject);
                }
                else{
                    isInstalled = false;
                }
            }
            if(!isInstalled)
            {
                clearDetailList.Add(detail);
            }
        }
        _allDetails = clearDetailList;
    }

    public List<Detail> GetALlDetails()
    {
        return _allDetails;
    }
    public List<Detail> GetAvailableDetails()
    {
        return GetOpenDetails();
    }
    public List<PointParentConnector> GetAllPoints()
    {
        return _allPoints;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneLoader.sceneLoader.LoadNextScene();
        }
        if (_isInstantiate)
        {   
            if(Input.touchSupported)
            {
                TransformPosition(target.transform.position, GetTouchAsWorldPoint(), magnDist);
            }
            else
            {
                TransformPosition(target.transform.position, GetMouseAsWorldPoint(), magnDist);
            }
        }
    }

    private void InstantiateGround(Detail ground)
    {
        ground.points[0].Install();
        GameObject instObject = Instantiate(ground._prefab.transform.GetChild(0).gameObject, ground.points[0].point.Position, Quaternion.Euler(ground.points[0].point.Rotation));
        _onInstantiateGround.Invoke((Transform)instObject.transform);

    }

    private void TransformPosition(Vector3 targetPosition, Vector3 mousePosition, float distance)
    {
        Vector3 bestPoint = Vector3.zero, bestRotation = Vector3.zero;
        Vector3 Yposition = Vector3.zero;
        float tempDist = Single.MaxValue, dist = 0;

        if(_connectionPoints.Count == 0)
        {
            _isConnect = false;
            Yposition =  mousePosition;
            if(Yposition.y < 0)
            {
                Yposition.y = 0f;
            }
            target.transform.position = Yposition;
        }
        else
        {
            for(int i = 0; i < _connectionPoints.Count; i++)
            {
                dist = Vector3.Distance (targetPosition, _connectionPoints[i].Position);
                _connectionObjects[i].SetActive(false);

                if (dist < tempDist)
                {
                    tempDist = dist;
                    bestPoint = _connectionPoints[i].Position;
                    bestRotation = _connectionPoints[i].Rotation;
                    _currentConnObject =_connectionObjects[i];
                }
            }
            _currentConnObject.SetActive(true);

            float distanceApart = GetSqrDistance(bestPoint, mousePosition);
            _lerp = MapValue(distanceApart, 0f, 50f, 0f, 1f);
            grayMaterial.color = Color.Lerp(_grayNearColor, _grayFarColor, _lerp);
            
            Debug.DrawLine(targetPosition, bestPoint);
    
            mZCoord = cam.WorldToScreenPoint(bestPoint).z;
            target.transform.rotation = Quaternion.Euler(bestRotation);

            if(distanceApart <= distance)
            {
                _currentConnObject.SetActive(false);
                _isConnect = true;
                target.transform.position = bestPoint;
            }
            else
            {
                _isConnect = false;
                Yposition =  mousePosition;
                target.transform.position = Yposition;
            }
        }
    }

    private float GetSqrDistance(Vector3 v1, Vector3 v2)
    {
        return (v1 - v2).sqrMagnitude;
    }

    private float MapValue(float mainValue, float inValueMin, float inValueMax, float outValueMin, float outValueMax)
    {
        return (mainValue - inValueMin) * (outValueMax - outValueMin) / (inValueMax - inValueMin) + outValueMin;
    }

    public void Save()
    {
        SaveLevel.SaveGame();
    }
    public void Load()
    {
        SaveLevel.LoadGame();
    }
    public void Restart()
    {
        SaveLevel.DeleteSaveFile();

        _allDetails = LevelContainer.currentLevelContainer.Reset();
        _connectionPoints.Clear();
        _connectionObjects.Clear();
        for(int i = 0; i < instantiateDetailObjects.Count; i++)
        {
            Destroy(instantiateDetailObjects[i]);
        }
        instantiateDetailObjects.Clear();
        StartDetailInstance();
        restartEvent.Invoke();
    }
}

