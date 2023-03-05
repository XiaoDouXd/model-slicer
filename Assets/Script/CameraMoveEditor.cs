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

        t.fromToZ = EditorGUILayout.Vector2Field("Zpos ��ʼ��ֹλ��", t.fromToZ);
        t.count = EditorGUILayout.IntField("�и���", t.count);
        t.CurI = EditorGUILayout.IntField("��ǰλ��", t.CurI);

        GUILayout.Space(16.0f);
        EditorGUILayout.LabelField("��ǰ�ɱ�����: ", t.PosLimit.ToString());
        if (GUILayout.Button("����")) t.Last();
        if (GUILayout.Button("ǰ��")) t.Next();
        if (GUILayout.Button("����һ���")) t.LastLittle();
        if (GUILayout.Button("ǰ��һ���")) t.NextLittle();
        GUILayout.Space(16.0f);
        t.path = EditorGUILayout.TextField("����Ŀ¼: StreamingAssets/", t.path);
        if (GUILayout.Button("����")) t.SaveFrame = true;
    }
}
