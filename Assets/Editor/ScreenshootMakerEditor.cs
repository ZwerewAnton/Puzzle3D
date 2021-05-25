using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScreenshootMaker))]
public class ScreenshootMakerEditor : Editor 
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if(GUILayout.Button("Take Screenshot")) {
            ((ScreenshootMaker)serializedObject.targetObject).TakeScreenshot();
        }
    }

    private void Update()
    {
        if(Input.GetKey("spacebar"))
        {
            ((ScreenshootMaker)serializedObject.targetObject).TakeScreenshot();
        }
    }
}
