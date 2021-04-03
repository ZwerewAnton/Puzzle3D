using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(Detail))]
[CanEditMultipleObjects]
public class DetailEditor : Editor
{
    bool showPoints, showParentPoints = true;
    List<bool> showPointList, showParentDetailList, showParentPointList, showParentsList;

    public void OnEnable()
    {
        //Debug.Log("OnEnable");
    }       
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Detail detail = (Detail) target;
        showPointList = detail.showPointList;
        showParentsList = detail.showParentsList;


        DrawParameters(detail);

        showPoints = EditorGUILayout.Foldout(showPoints, "Points", true);
        if(showPoints)
        {
            EditorGUI.indentLevel++;
            SerializedProperty pointsListSP = serializedObject.FindProperty("points");

            List<PointParentConnector> pointParentConnectorList = detail.points;
            //TODO Focus on field size
            EditorGUI.BeginChangeCheck();
            int pointParentConnectorSize = Mathf.Max(0, EditorGUILayout.IntField("Size", pointParentConnectorList.Count));
            if( EditorGUI.EndChangeCheck() ) {
                EditorUtility.SetDirty(target);
            }

            if(pointParentConnectorList.Count != pointParentConnectorSize)
            {
                while(pointParentConnectorSize > pointParentConnectorList.Count)
                {
                    pointParentConnectorList.Add(null);
                }
                while(pointParentConnectorSize < pointParentConnectorList.Count)
                {
                    pointParentConnectorList.RemoveAt(pointParentConnectorList.Count - 1);
                }
            }
            if(pointParentConnectorSize != showPointList.Count)
            {
                while(pointParentConnectorSize > showPointList.Count)
                {
                    showPointList.Add(false);
                    showParentsList.Add(false);
                }
                while(pointParentConnectorSize < showPointList.Count)
                {
                    showPointList.RemoveAt(showPointList.Count - 1);
                    showParentsList.RemoveAt(showParentsList.Count - 1);
                }
            }

            serializedObject.Update();
            for (int i = 0; i < pointParentConnectorList.Count; i++)
            {
                SerializedProperty pointSP = pointsListSP.GetArrayElementAtIndex(i).FindPropertyRelative("point");
                SerializedProperty parentListSP = pointsListSP.GetArrayElementAtIndex(i).FindPropertyRelative("parentList");

                showPointList[i] = EditorGUILayout.Foldout(showPointList[i],new GUIContent("Point " + (i).ToString()), true);
                if(showPointList[i])
                {   
                    
                    EditorGUI.indentLevel++;
                    EditorGUI.BeginChangeCheck();
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginVertical();
                    pointParentConnectorList[i].point.Position = EditorGUILayout.Vector3Field("Pos", pointParentConnectorList[i].point.Position);
                    pointParentConnectorList[i].point.Rotation = EditorGUILayout.Vector3Field("Rot" , pointParentConnectorList[i].point.Rotation);
                    GUILayout.EndVertical();
                    if (GUILayout.Button("C", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(false))) //8
                    {
                        Transform objectTransform = detail.gameObject.GetComponent<Transform>();
                        pointParentConnectorList[i].point.Position = objectTransform.position;
                        pointParentConnectorList[i].point.Rotation = objectTransform.rotation.eulerAngles;
                    }
                    GUILayout.EndHorizontal();
                    if( EditorGUI.EndChangeCheck() ) {
                        EditorUtility.SetDirty(target);
                    }
                    showParentsList[i] = EditorGUILayout.Foldout(showParentsList[i], "Parents", true);
                    if(showParentsList[i])
                    {
                        EditorGUI.indentLevel++;
                        EditorGUI.BeginChangeCheck();
                        showParentDetailList = detail.points[i].showParentDetailList;
                        int parentListSize = Mathf.Max(0, EditorGUILayout.IntField("Size", pointParentConnectorList[i].parentList.Count));
                        if( EditorGUI.EndChangeCheck() ) {
                            EditorUtility.SetDirty(target);
                        }
                        if(parentListSize != showParentDetailList.Count)
                        {
                            while(parentListSize > showParentDetailList.Count)
                            {
                                showParentDetailList.Add(false);
                            }
                            while(parentListSize < showParentDetailList.Count)
                            {
                                Debug.Log(pointParentConnectorList[i].parentList.Count);
                                int ss = pointParentConnectorList[i].parentList.Count - 1;
                                showParentDetailList.RemoveAt(showParentDetailList.Count - 1);
                            }
                        }
                        if(pointParentConnectorList[i].parentList.Count != parentListSize){

                            while(parentListSize > pointParentConnectorList[i].parentList.Count)
                            {
                                pointParentConnectorList[i].parentList.Add(null);
                            }
                            while(parentListSize < pointParentConnectorList[i].parentList.Count)
                            {
                                pointParentConnectorList[i].parentList.RemoveAt(pointParentConnectorList[i].parentList.Count - 1);
                            }
                        }

                        serializedObject.Update();
                        for (int j = 0; j < pointParentConnectorList[i].parentList.Count; j++)
                        {
                            SerializedProperty parentDetailSP = parentListSP.GetArrayElementAtIndex(j).FindPropertyRelative("parentDetail");
                            SerializedProperty parentPointListSP = parentListSP.GetArrayElementAtIndex(j).FindPropertyRelative("parentPointList");

                            showParentDetailList[j] = EditorGUILayout.Foldout(showParentDetailList[j], new GUIContent("Parent Detail Points " + (j).ToString()), true);
                            if(showParentDetailList[j])
                            { 
                                EditorGUI.indentLevel++;                           
                                EditorGUI.BeginChangeCheck();
                                EditorGUILayout.PropertyField(parentDetailSP, new UnityEngine.GUIContent("Parent" + "_" + (j).ToString()), true);
                                
                                if( EditorGUI.EndChangeCheck() ) {
                                        EditorUtility.SetDirty(target);
                                    }

                                Detail ob = parentDetailSP.objectReferenceValue as Detail;
                                pointParentConnectorList[i].parentList[j].parentDetail = ob;
                                if(ob != null && ob.points != null)
                                {
                                    EditorGUI.BeginChangeCheck(); 
                                    
                                    showParentPointList = detail.points[i].parentList[j].showParentPointList;   
                                    List<Point> thisParentPointList = detail.points[i].parentList[j].parentPointList;
                                    List<bool> checkToogleList = detail.points[i].parentList[j].checkToogleList;
                                    int parentPointListSize = ob.points.Count;
                                    if( EditorGUI.EndChangeCheck() ) {
                                        EditorUtility.SetDirty(target);
                                    }
                                    if(parentPointListSize != showParentPointList.Count)
                                    {
                                        while(parentPointListSize > showParentPointList.Count)
                                        {
                                            showParentPointList.Add(false);
                                            checkToogleList.Add(false);
                                        }
                                        while(parentPointListSize < showParentPointList.Count)
                                        {
                                            showParentPointList.RemoveAt(showParentPointList.Count - 1);
                                            checkToogleList.RemoveAt(checkToogleList.Count - 1);
                                        }
                                    }
                                    showParentPoints = EditorGUILayout.Foldout(showParentPoints, "Parent", true);
                                    if(showParentPoints)
                                    {
                                        EditorGUI.indentLevel++;
                                        serializedObject.Update();
                                        for (int k = 0; k < ob.points.Count; k++)
                                        {
                                            //SerializedProperty parentPointPositionSP = parentPointListSP.GetArrayElementAtIndex(k).FindPropertyRelative("Position");
                                            //SerializedProperty parentPointRotationSP = parentPointListSP.GetArrayElementAtIndex(k).FindPropertyRelative("Rotation");

                                            showParentPointList[k] = EditorGUILayout.Foldout(showParentPointList[k], ("\"" + ob.gameObject.name + "\"" + " point " + k.ToString()), true);
                                            if(showParentPointList[k])
                                            {  
                                                EditorGUI.BeginChangeCheck();
                                                GUILayout.BeginHorizontal();
                                                GUILayout.BeginVertical();   
                                                EditorGUI.BeginChangeCheck();
                                                //detail.points[i].parentList[j].parentPointList[k];
                                                EditorGUI.BeginDisabledGroup(true);
                                                EditorGUILayout.Vector3Field("Pos", ob.points[k].point.Position);
                                                EditorGUILayout.Vector3Field("Rot", ob.points[k].point.Rotation);
                                                EditorGUI.EndDisabledGroup();
                                                GUILayout.EndVertical();
                                                checkToogleList[k] = GUILayout.Toggle(checkToogleList[k], "");                                  
                                                GUILayout.EndHorizontal();
                                                if( EditorGUI.EndChangeCheck() ) 
                                                {
                                                    if(checkToogleList[k])
                                                    {
                                                        thisParentPointList.Add(ob.points[k].point);
                                                    }
                                                    else
                                                    {
                                                        for (int l = 0; l < thisParentPointList.Count; l++)
                                                        {
                                                            if(thisParentPointList[l].Position == ob.points[k].point.Position &&
                                                                thisParentPointList[l].Rotation == ob.points[k].point.Rotation)
                                                                {
                                                                    thisParentPointList.RemoveAt(l);
                                                                }
                                                        }
                                                    }
                                                    EditorUtility.SetDirty(target);
                                                }
                                            }
                                        }
                                        EditorGUI.indentLevel--;
                                    }
                                
                                }
                                EditorGUI.indentLevel--;
                            }
                        
                        }
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                }
            } 
            EditorGUI.indentLevel--;  
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    
    
    static void DrawParameters(Detail detail)
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.LabelField("Detail", EditorStyles.boldLabel); //3

        EditorGUILayout.Space(10f);
        EditorGUILayout.LabelField("Detail features");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Count");
        detail.count = EditorGUILayout.FloatField(detail.count);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Prefab");
        detail._prefab = (GameObject)EditorGUILayout.ObjectField(detail._prefab, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Icon");
        detail.icon = (Sprite)EditorGUILayout.ObjectField(detail.icon, typeof(Sprite), true);
        EditorGUILayout.EndHorizontal();
        if( EditorGUI.EndChangeCheck() ) {
            EditorUtility.SetDirty(detail);
        }
    }
}

