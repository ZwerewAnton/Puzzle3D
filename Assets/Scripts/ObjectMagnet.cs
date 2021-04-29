using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectMagnet : MonoBehaviour
{
    //public UISpawnListItems uiSpawnListItems;
    //public LevelContainer levelContainer;
    public List<Detail> detailsList;
    public Camera cam;
    public float magnDist = 0.5f;
    public Detail ground;
    public Material grayMaterial;
    public Vector3 objectOffset;
/*     public SaveLevel saveLevel; */
    public LevelContainer levelContainer;
    public UnityEvent restartEvent;
    
    private GameObject target;
    private float mZCoord;
    private bool _isConnect;
    private bool _isInstantiate;

    private List<GameObject> instantiateDetailObjects;
    private List<Detail> _allDetails;
    private List<Detail> _availableDetails;
    private List<Detail> _installedDetails;
    private List<Point> _connectionPoints;
    private List<GameObject> _connectionObjects;
    private List<PointParentConnector> _allPoints;
    //private GameObject _instObject;
    private Detail _instDetail;
    private Material _mainMaterial;
    private MeshRenderer meshRenderer;
    private Color _grayNearColor = new Color32(255, 255, 255, 150);
    private Color _grayFarColor = new Color32(255, 255, 255, 0);
    private float _lerp = 0;
    
    private GameObject _currentConnObject;
    
    private void Awake() 
    {
        
        //_allDetails = levelContainer.Reset();
 
        //Debug.Log(_allDetails[0].name);
        //Reset(detailsList);
        //_allDetails = saveLevel.GetLoadDetailList();
        //SaveLevel.LoadGame();
        _allDetails = levelContainer.GetLoadLevel();
        
        instantiateDetailObjects = new List<GameObject>();
        _connectionPoints = new List<Point>();
        _connectionObjects = new List<GameObject>();
        _installedDetails = new List<Detail>();
        _availableDetails = new List<Detail>();

        //Debug.Log(_allDetails.Count);
        StartDetailInstance();
    }

    private void Start()
    {
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition + objectOffset;
        // z coordinate of game object on screen
        mousePoint.z = mZCoord;
        // Convert it to world points
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

        //target = Instantiate(_instObject, GetMouseAsWorldPoint(), Quaternion.identity);
        target = Instantiate(_instObject, GetTouchAsWorldPoint(), Quaternion.identity);

        _allPoints = _instDetail.GetPoints;
        
        bool isCon = true;


        foreach(var targetPPC in _allPoints)
        {
            if(!targetPPC.IsInstalled)
            {
                isCon = true;
                foreach(var targetParent in targetPPC.parentList)
                {
                    foreach(var targetParentPoint in targetParent.parentPointList)
                    {
                        foreach(var parentPPC in targetParent.parentDetail.points)
                        {
                            if(targetParentPoint.Position == parentPPC.point.Position &&
                                targetParentPoint.Rotation == parentPPC.point.Rotation)
                                {
                                    if(!parentPPC.IsInstalled && !targetParent.parentDetail.isRoot)
                                    {
                                        isCon = false;
                                    }
                                }
                        }
                    }
                }
                if(isCon)
                {
                    _connectionPoints.Add(targetPPC.point);
                }
            }
        }

        foreach(Point connectionPoint in _connectionPoints)
        {
            GameObject connectionObject = Instantiate(_instObject, connectionPoint.Position, Quaternion.Euler(connectionPoint.Rotation));
            _connectionObjects.Add(connectionObject);
            meshRenderer = connectionObject.GetComponent<MeshRenderer>();
            meshRenderer.material = grayMaterial;
            connectionObject.SetActive(false);
        }
        //connectionPoints = target.GetComponent<SimpleDetail>().GetPoints;
        //Debug.Log(connectionPoints.Count);
        _isInstantiate = true;
        //target.transform.position = GetMouseAsWorldPoint();
    }

    public bool InstalOrDropObject()
    {
        
        //Input.multiTouchEnabled = true;
        bool isDetailInstalled = false;
        _isInstantiate = false;

        foreach(GameObject connectionObject in _connectionObjects)
        {
            Destroy(connectionObject);
        }
        if (_isConnect)
        {
            foreach(Detail installedDetail in _installedDetails)
            {
                if(_instDetail == installedDetail)
                {
                    isDetailInstalled = true;
                    break;
                }
            }
            if(!isDetailInstalled)
            {
                _installedDetails.Add(_instDetail);
                //_availableDetails.
                _availableDetails.AddRange(GetOpenDetails(_instDetail));
            }
            Transform targetTransform = target.GetComponent<Transform>();

            //TODO Connection point instead targetTransform
            foreach(PointParentConnector pointParConn in _instDetail.points)
            {
                if(targetTransform.position == pointParConn.point.Position &&
                    targetTransform.rotation.eulerAngles == pointParConn.point.Rotation)
                {
                    pointParConn.Install();
                    _availableDetails.Remove(_instDetail);
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

    public bool IsLastDetail()
    {
        bool isLastDetail = _instDetail.IsLastDetail();
        if(isLastDetail)
        {
            _allDetails.Remove(_instDetail);
            _availableDetails.Remove(_instDetail);
            //Debug.Log(IsEnd());
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

    private List<Detail> GetOpenDetails(Detail detail)
    {
        List<Detail> list = new List<Detail>();
        foreach(Detail allDetail in _allDetails)
        {
            foreach(PointParentConnector allPointParConn in allDetail.points)
            {

                if(!allPointParConn.IsInstalled)
                {
                    if(allPointParConn.parentList.Count == 1)
                    {
                        foreach(Parent allParentDetail in allPointParConn.parentList)
                        {
                            if(allParentDetail.parentDetail == detail)
                            {
                                list.Add(allDetail);
                            }
                        }
                    }
                    else if(allPointParConn.parentList.Count > 1)
                    {
                        bool isOpen = true;
                        foreach(Detail installDetail in _installedDetails)
                        {
                            foreach(Parent allParentDetail in allPointParConn.parentList)
                            {
                                if(allParentDetail.parentDetail != detail &&
                                     allParentDetail.parentDetail != installDetail)
                                {
                                    isOpen = false;
                                    break;
                                }
                            }
                            if(isOpen)
                            {
                                list.Add(allDetail);
                            }
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
                    _instObject = Instantiate(_instObject, pPC.point.Position, Quaternion.Euler(pPC.point.Rotation));
                    instantiateDetailObjects.Add(_instObject);
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

    public List<Detail> GetALlDetails()
    {
        return _allDetails;
    }
    public List<Detail> GetAvailableDetails()
    {
        return _availableDetails;
    }
    public List<PointParentConnector> GetAllPoints()
    {
        return _allPoints;
    }
    
    private void Update()
    {
        //Debug.Log(GetMouseAsWorldPoint());
        
        //Debug.Log(_availableDetails.Count);
        if(Input.GetKeyDown("space"))
        {
            Debug.Log(target.transform.position);
            //InstantiateObject();
        } 
        //TODO Invert or check another thing
        if (_isInstantiate)
        {   
            //Input.multiTouchEnabled = false;
            //TransformPosition(target.transform.position, GetMouseAsWorldPoint(), magnDist);
            TransformPosition(target.transform.position, GetTouchAsWorldPoint(), magnDist);
            if (Input.GetMouseButton(1))
            {
                if (_isConnect)
                {
                    Debug.Log("!");
                }   
            }
        }


    }
    //TODO Add a camera transform position
    private void TransformPosition(Vector3 targetPosition, Vector3 mousePosition, float distance)
    {
        //Хрень
        //mousePosition = mousePosition;
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
            target.transform.position = Yposition /* + objectOffset */;
        }
        else
        {
            for(int i = 0; i < _connectionPoints.Count; i++)
            {
                dist = Vector3.Distance (targetPosition, _connectionPoints[i].Position);
                if (dist < tempDist)
                {
                    tempDist = dist;
                    bestPoint = _connectionPoints[i].Position;
                    bestRotation = _connectionPoints[i].Rotation;

                    _currentConnObject =_connectionObjects[i];
                    _currentConnObject.SetActive(false);
                }
            }

            _currentConnObject.SetActive(true);

            float distanceApart = GetSqrDistance(bestPoint, mousePosition);
            //Debug.Log(distanceApart);
            _lerp = MapValue(distanceApart, 0f, 50f, 0f, 1f);

            grayMaterial.color = Color.Lerp(_grayNearColor, _grayFarColor, _lerp);
            //grayMaterial.color = _grayColor;
            
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

    public void Save(){
        SaveLevel.SaveGame();
    }
    public void Load(){
        SaveLevel.LoadGame();
    }
    public void Restart()
    {
        SaveLevel.DeleteSaveFile();

        _allDetails = levelContainer.Reset();
        _connectionPoints.Clear();
        _connectionObjects.Clear();
        _installedDetails.Clear();
        _availableDetails.Clear();
        for(int i = 0; i < instantiateDetailObjects.Count; i++)
        {
            Destroy(instantiateDetailObjects[i]);
        }
        instantiateDetailObjects.Clear();

        StartDetailInstance();
        restartEvent.Invoke();
        Debug.Log(_availableDetails.Count);
    }
}

