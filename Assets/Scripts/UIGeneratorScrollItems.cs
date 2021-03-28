using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGeneratorScrollItems : MonoBehaviour
{


    [SerializeField] private RectTransform objectList;
    private List<GameObject> objectsList;
    
    // Start is called before the first frame update
    void Start()
    {
        objectsList = new List<GameObject>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
