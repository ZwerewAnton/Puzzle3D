using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCorrector : MonoBehaviour
{
/*     public int baseTH = 6;
    public int basePPI = 210;
    public int dragTH = 0; */

    void Start()
    {
        int defaultValue = EventSystem.current.pixelDragThreshold;        
        EventSystem.current.pixelDragThreshold = Mathf.Max(defaultValue, (int)(defaultValue * Screen.dpi / 160f));
    }
}
