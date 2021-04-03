using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDetail : MonoBehaviour
{


    public static GameObject target;
    public static GameObject instObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void InstantiateObject()
    {
        //DragEvent.Invoke();
        
        //mZCoord = cam.WorldToScreenPoint(instObject.transform.position).z;
        //mOffset = instObject.transform.position - GetMouseAsWorldPoint();
        
        //target = Instantiate(instObject, GetMouseAsWorldPoint(), Quaternion.identity);
        target = Instantiate(instObject);
        //connectionPoints = target.GetComponent<SimpleDetail>().GetPoints;
        //target.transform.position = GetMouseAsWorldPoint();
        
    }

    public void DestroyObject()
    {
        Destroy(instObject);
    }

}
