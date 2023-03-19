using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClipPanelUI : MonoBehaviour
{
    public CameraMove camMove;

    [Space(10)]
    public InputField from;
    public InputField to;
    public InputField curPos;

    public void OnValueChange(string type)
    {
        switch (type)
        {
            case "from":
            {
                var val = 0f;
                if (float.TryParse(from.text, out var o)) val = o;
                camMove.fromToZ = new Vector2(val, camMove.fromToZ.y);
            } break;
            case "to":
            {
                var val = 0f;
                if (float.TryParse(to.text, out var o)) val = o;
                camMove.fromToZ = new Vector2(camMove.fromToZ.x, val);
            } break;
            case "pos":
            {
                var val = 0;
                if (int.TryParse(curPos.text, out var o)) val = o;
                camMove.CurI = val;
            } break;
        }
    }
    
    public void MoveB() { camMove.Last(); }
    public void MoveF() { camMove.Next(); }

    public void MoveB_lit() { camMove.LastLittle(); }
    public void MoveF_lit() { camMove.NextLittle(); }
    public void SaveMove() { camMove.SaveOffset(); }
    public void ResetOffset() { camMove.ResetOffset(); }
    public void SaveClip() { camMove.SaveFrame = true; }

    private void Update()
    {
        from.text = camMove.fromToZ.x.ToString("F3");
        to.text = camMove.fromToZ.y.ToString("F3");
        curPos.text = camMove.CurI.ToString();
    }
}
