using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectMagnet : MonoBehaviour
{
    //public UISpawnListItems uiSpawnListItems;
    public List<Detail> detailsList;
    public Camera cam;
    public float magnDist = 0.5f;
    public Detail ground;
    public Material grayMaterial;
    
    [SerializeField]
    //private GameObject instObject;
    private GameObject target;
    private Vector3 mOffset;
    private Vector3 _position;
    private float mZCoord;
    private bool _isConnect;
    private bool _isInstantiate;

    private List<Detail> _allDetails;
    private List<Point> connectionPoints;
    private List<Detail> _installedDetails;
    private List<PointParentConnector> _allPoints;
    //private GameObject _instObject;
    private Detail _instDetail;
    private Material _mainMaterial;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        Reset(detailsList);
        connectionPoints = new List<Point>();
        _installedDetails = new List<Detail>();
        _installedDetails.Add(ground);


        //uiSpawnListItems.spawnListEvent.AddListener(InstantiateObject);
        //uiSpawnListItems.dropDetailEvent.AddListener(InstalOrDropObject);
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        //Debug.Log(mousePoint);
        // z coordinate of game object on screen
        mousePoint.z = mZCoord;
        // Convert it to world points
        return cam.ScreenToWorldPoint(mousePoint);
    }

    public void InstantiateObject(Detail instDetail)
    {
        _instDetail = instDetail;
        GameObject _instObject = instDetail._prefab.transform.GetChild(0).gameObject;
        //_instObject = instDetail._prefab;
        /*
        foreach(var det in detailsList){
            if(instDetail == det){
                Debug.Log("dddd");
            }
        }*/
        //_instObject = instObject;
        //instObject.
        connectionPoints.Clear();

        mZCoord = cam.WorldToScreenPoint(_instObject.transform.position).z;
        //Debug.Log(mZCoord);
        mOffset = _instObject.transform.position - GetMouseAsWorldPoint();

        
        //GameObject childGO = _instObject.transform.GetChild(0).gameObject;
        target = Instantiate(_instObject, GetMouseAsWorldPoint(), Quaternion.identity);
        meshRenderer = target.GetComponent<MeshRenderer>();
        _mainMaterial = meshRenderer.material;
        meshRenderer.material = grayMaterial;

        //_allPoints = target.GetComponent<Detail>().GetPoints;
        _allPoints = _instDetail.GetPoints;
        


        foreach (var point in _allPoints)
        {
            if(!point.IsInstalled)
            {
                foreach(var pointParentConn in point.parentList)
                {
                    foreach(var instalDetail in _installedDetails)
                    {
                        if(pointParentConn.parentDetail == instalDetail)
                        {
                            foreach(var parentPoint in pointParentConn.parentPointList)
                            {
                                foreach(var instalDetailPoint in instalDetail.GetPoints)
                                {
                                    if(parentPoint.Position == instalDetailPoint.point.Position &&
                                        parentPoint.Rotation == instalDetailPoint.point.Rotation)
                                    {
                                        connectionPoints.Add(point.point);

                                    }
                                }
                            }
                            //Debug.Log(pointParentConn.parentDetail.name);
                            
                        }
                    }
                }
            }
            
        }
        //connectionPoints = target.GetComponent<SimpleDetail>().GetPoints;
        //Debug.Log(connectionPoints.Count);
        _isInstantiate = true;
        //target.transform.position = GetMouseAsWorldPoint();
    }

/*
    public bool InstalOrDropObject()
    {
        if (_isInstantiate)
        {
            if (_isConnect)
            {
                target = null;
                return true;
            }
            else
            {
                Destroy(target);
                return false;
            }
            _isInstantiate = false;
        }
    }
    */
    public bool InstalOrDropObject()
    {
        bool isDetailInstalled = false;
        _isInstantiate = false;
        if (_isConnect)
        {
            foreach(Detail installedDetail in _installedDetails){
                if(_instDetail == installedDetail){
                    isDetailInstalled = true;
                    break;
                }
            }
            if(!isDetailInstalled){
                _installedDetails.Add(_instDetail);
            }
            Transform targetTransform = target.GetComponent<Transform>();

            //TODO Connection point instead targetTransform
            foreach(PointParentConnector pointParConn in _instDetail.points){
                if(targetTransform.position == pointParConn.point.Position &&
                    targetTransform.rotation.eulerAngles == pointParConn.point.Rotation)
                {
                    pointParConn.Install();
                }
            }

            meshRenderer.material = _mainMaterial;
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
        _allDetails = new List<Detail>(detailsList);
    }

    public bool IsLastDetail()
    {
        bool isInstall = InstalOrDropObject();
        if(isInstall){
            bool isLastDetail = _instDetail.IsLastDetail();
            if(isLastDetail){
                _allDetails.Remove(_instDetail);
                
                Debug.Log(IsEnd());
                return true;
            }
            else{
                return false;
            }
        }
        else{
            return false;
        }
    }

    public bool IsEnd(){
        if(_allDetails.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private void Update()
    {
        //Debug.Log(GetMouseAsWorldPoint());
        if(Input.GetKeyDown("space"))
        {
            Debug.Log(target.transform.position);
            //InstantiateObject();
        } 
        //TODO Invert or check another thing
        if (_isInstantiate)
        {
            _position = GetMouseAsWorldPoint() /*+ mOffset*/;
            TransformPosition(target.transform.position, _position, magnDist);
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
        Vector3 bestPoint = Vector3.zero, bestRotation = Vector3.zero;
        Vector3 Yposition = Vector3.zero;
        float tempDist = Single.MaxValue, dist = 0;

        if(connectionPoints.Count == 0)
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
            foreach (var point in connectionPoints)
            {
                dist = Vector3.Distance (targetPosition, point.Position);
                if (dist < tempDist)
                {
                    tempDist = dist;
                    bestPoint = point.Position;
                    bestRotation = point.Rotation;
                }
            }
            
            Debug.DrawLine(targetPosition, bestPoint);
    
            target.transform.rotation = Quaternion.Euler(bestRotation);
            //TODO WTF???
            /*if ((Math.Abs(mousePosition.x) <= distance) &&
                (Math.Abs(mousePosition.y) <= distance) &&
                (Math.Abs(mousePosition.z) <= distance))*/
            if(((Math.Abs(mousePosition.x) - (Math.Abs(bestPoint.x))) <= distance) &&
                ((Math.Abs(mousePosition.y) - (Math.Abs(bestPoint.y))) <= distance) &&
                ((Math.Abs(mousePosition.z) - (Math.Abs(bestPoint.z))) <= distance))
            {
                _isConnect = true;
                target.transform.position = bestPoint;
            }
            else
            {
                _isConnect = false;
                Yposition =  mousePosition;
                if(Yposition.y < 0)
                {
                    Yposition.y = 0f;
                }
                target.transform.position = Yposition;
            }
        }
    }
}

