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

        if (!t.Saving)
        {
            t.fromToZ = EditorGUILayout.Vector2Field("Zpos 起始终止位置", t.fromToZ);
            t.Count = EditorGUILayout.IntField("切割数", t.Count);
            t.CurI = EditorGUILayout.IntField("当前位置", t.CurI);
        }
        else
        {
            EditorGUILayout.LabelField("导出中...");
            EditorGUILayout.LabelField("若游戏没在运行中:");
            EditorGUILayout.LabelField("请在 Game 窗口的渲染视口中, 按下并拖动鼠标以刷新.");
        }

        GUILayout.Space(16.0f);
        EditorGUILayout.LabelField("当前可变区间: ", t.Saving ?  "*" : t.PosLimit.ToString());
        if (GUILayout.Button("后移")) t.Last();
        if (GUILayout.Button("前移")) t.Next();
        if (GUILayout.Button("后移一点点")) t.LastLittle();
        if (GUILayout.Button("前移一点点")) t.NextLittle();
        GUILayout.Space(16.0f);
        if (GUILayout.Button("缓存微调")) t.SaveOffset();
        if (GUILayout.Button("重置微调")) t.ResetOffset();
        t.path = EditorGUILayout.TextField("保存目录: StreamingAssets/", t.path);
        if (GUILayout.Button("导出")) t.SaveFrame = true;
        if (GUILayout.Button("导出全部")) t.OutPutAll();
    }
}
