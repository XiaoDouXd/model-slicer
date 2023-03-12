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
            t.fromToZ = EditorGUILayout.Vector2Field("Zpos ��ʼ��ֹλ��", t.fromToZ);
            t.Count = EditorGUILayout.IntField("�и���", t.Count);
            t.CurI = EditorGUILayout.IntField("��ǰλ��", t.CurI);
        }
        else
        {
            EditorGUILayout.LabelField("������...");
            EditorGUILayout.LabelField("����Ϸû��������:");
            EditorGUILayout.LabelField("���� Game ���ڵ���Ⱦ�ӿ���, ���²��϶������ˢ��.");
        }

        GUILayout.Space(16.0f);
        EditorGUILayout.LabelField("��ǰ�ɱ�����: ", t.Saving ?  "*" : t.PosLimit.ToString());
        if (GUILayout.Button("����")) t.Last();
        if (GUILayout.Button("ǰ��")) t.Next();
        if (GUILayout.Button("����һ���")) t.LastLittle();
        if (GUILayout.Button("ǰ��һ���")) t.NextLittle();
        GUILayout.Space(16.0f);
        if (GUILayout.Button("����΢��")) t.SaveOffset();
        if (GUILayout.Button("����΢��")) t.ResetOffset();
        t.path = EditorGUILayout.TextField("����Ŀ¼: StreamingAssets/", t.path);
        if (GUILayout.Button("����")) t.SaveFrame = true;
        if (GUILayout.Button("����ȫ��")) t.OutPutAll();
    }
}
