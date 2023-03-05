using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraMove : MonoBehaviour
{
    public Vector2 fromToZ = new Vector2(-3.847f, 3.82f);
    public int count = 64;
    public string path = "OutPut~";

    public float Step => (fromToZ.y - fromToZ.x) / (count - 1);
    public Vector2 PosLimit
    {
        get
        {
            return new Vector2(
                    (fromToZ.x + Step * CurI) - (0.5f * Step),
                    (fromToZ.x + Step * CurI) + (0.5f * Step));
        }
    }
    public int CurI 
    { 
        get => (int)_curCount; 
        set 
        {
            if (value <= -1 || value >= count) return;
            bool refreshCheck = false;
            if (value != _curCount) refreshCheck = true;
            _curCount = (uint)value;
            if (refreshCheck) Refresh();
        }
    }
    public bool SaveFrame { get; set; }

    private Transform _transf;
    private uint _curCount = 0;

    public void Last()
    {
        if (_curCount == 0) return;
        _curCount--; Refresh();
    }

    public void Next()
    {
        if (_curCount == (count - 1)) return;
        _curCount++; Refresh();
    }

    public void LastLittle()
    {
        _transf.position = new Vector3(_transf.position.x, _transf.position.y, _transf.position.z - 0.05f * Step);
    }

    public void NextLittle()
    {
        _transf.position = new Vector3(_transf.position.x, _transf.position.y, _transf.position.z + 0.05f * Step);
    }

    void Refresh()
    {
        _transf.position = new Vector3(_transf.position.x, _transf.position.y, (fromToZ.x + Step * _curCount));
    }

    // ------------------------------------------------------------

    void Awake()
    {
        _transf = GetComponent<Transform>();
        if (!_transf) UnityEngine.Debug.LogError("Cant find transform component in the obj self");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var limit = PosLimit;
        float zPos = _transf.position.z;

        if (zPos < limit.x) _transf.position = new Vector3(_transf.position.x, _transf.position.y, limit.x);
        else if (zPos > limit.y) _transf.position = new Vector3(_transf.position.x, _transf.position.y, limit.y);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (SaveFrame)
        {
            SaveFrame = false;

            var virtualPhoto = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
            RenderTexture.active = source;
            virtualPhoto.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
            RenderTexture.active = null;

            byte[] bytes;
            bytes = virtualPhoto.EncodeToPNG();
            if (!System.IO.File.Exists(Application.streamingAssetsPath + $"/{path}/{CurI}.png"))
            {
                if (!System.IO.Directory.Exists(Application.streamingAssetsPath + $"/{path}/"))
                    System.IO.Directory.CreateDirectory(Application.streamingAssetsPath + $"/{path}/");
                var s = System.IO.File.Create(Application.streamingAssetsPath + $"/{path}/{CurI}.png");
                s.Write(bytes); s.Close();
            }
            else
            {
                System.IO.File.WriteAllBytes(Application.streamingAssetsPath + $"/{path}/{CurI}.png", bytes);
            }
        }
        Graphics.Blit(source, destination);
    }
}
