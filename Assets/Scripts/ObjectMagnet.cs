using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectMagnet : MonoBehaviour
{
    public UISpawnListItems uiSpawnListItems;
    public GameObject target;
    public Camera cam;
    public float magnDist = 0.5f;
    public Detail ground;
    public Material grayMaterial;
    
    [SerializeField]
    //private GameObject instObject;
    private Vector3 mOffset;
    private Vector3 _position;
    private float mZCoord;
    private bool _isConnect;
    private bool _isInstantiate;

    [SerializeField]
    private List<Point> connectionPoints;
    private List<Detail> _installedDetails;
    private List<PointParentConnector> _allPoints;
    private GameObject _instObject;
    private Material _mainMaterial;
    private MeshRenderer meshRenderer;

    private void Start()
    {
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

    public void InstantiateObject(GameObject instObject)
    {
        //instObject = listItem.instObject;
        _instObject = instObject;
        //instObject.
        connectionPoints.Clear();

        mZCoord = cam.WorldToScreenPoint(instObject.transform.position).z;
        mOffset = instObject.transform.position - GetMouseAsWorldPoint();
        
        target = Instantiate(instObject, GetMouseAsWorldPoint(), Quaternion.identity);
        meshRenderer = target.GetComponentInChildren<MeshRenderer>();
        _mainMaterial = meshRenderer.material;
        meshRenderer.material = grayMaterial;

        _allPoints = target.GetComponent<Detail>().GetPoints;

        


        foreach (var point in _allPoints)
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
                        Debug.Log(pointParentConn.parentDetail.name);
                        
                    }
                }
            }
        }
        //connectionPoints = target.GetComponent<SimpleDetail>().GetPoints;
        Debug.Log(connectionPoints.Count);
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
        _isInstantiate = false;
        if (_isConnect)
        {
            _installedDetails.Add(_instObject.GetComponent<Detail>());
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

    
    private void Update()
    {
        //Debug.Log(GetMouseAsWorldPoint());
        if(Input.GetKeyDown("space"))
        {
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
            
            //Debug.DrawLine(targetPosition, bestPoint);
    
            target.transform.rotation = Quaternion.Euler(bestRotation);
            //TODO WTF???
            if ((Math.Abs(mousePosition.x) <= distance) &&
                (Math.Abs(mousePosition.y) <= distance) &&
                (Math.Abs(mousePosition.z) <= distance))
            {
                _isConnect = true;
                target.transform.position =  bestPoint;
            }
            else
            {
                _isConnect = false;
                Yposition =  mousePosition  + bestPoint;
                if(Yposition.y < 0)
                {
                    Yposition.y = 0f;
                }
                target.transform.position = Yposition;
            }
        }
    }
}

