using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraMove))]
public class CameraMoveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CameraMove t = (CameraMove)target;

        t.fromToZ = EditorGUILayout.Vector2Field("Zpos 起始终止位置", t.fromToZ);
        t.count = EditorGUILayout.IntField("切割数", t.count);
        t.CurI = EditorGUILayout.IntField("当前位置", t.CurI);

        GUILayout.Space(16.0f);
        EditorGUILayout.LabelField("当前可变区间: ", t.PosLimit.ToString());
        if (GUILayout.Button("后移")) t.Last();
        if (GUILayout.Button("前移")) t.Next();
        if (GUILayout.Button("后移一点点")) t.LastLittle();
        if (GUILayout.Button("前移一点点")) t.NextLittle();
        GUILayout.Space(16.0f);
        t.path = EditorGUILayout.TextField("保存目录: StreamingAssets/", t.path);
        if (GUILayout.Button("保存")) t.SaveFrame = true;
    }
}
