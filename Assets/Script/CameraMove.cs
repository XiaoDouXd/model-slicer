using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraMove : MonoBehaviour
{
    public Vector2 fromToZ = new Vector2(-3.847f, 3.82f);
    public string path = "OutPut~";

    public bool Saving => _savingAll;
    public float Step => (fromToZ.y - fromToZ.x) / (_count - 1);
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
            if (_savingAll) return;
            if (value <= -1 || value >= _count) return;
            bool refreshCheck = false;
            if (value != _curCount) refreshCheck = true;
            _curCount = (uint)value;
            if (refreshCheck) Refresh();
        }
    }
    public int Count 
    {
        get => (int)_count;
        set {
            if (_savingAll) return;
            if (value <= 0) return;
            _count = (uint)value;
        }
    }
    public bool SaveFrame { get; set; }

    private Transform _transf;
    private uint _curCount = 0;
    private uint _count = 256;
    private List<float> _offset = new List<float>();
    private bool _savingAll;
    private int _lastI;

    public void Last()
    {
        if (_savingAll) return;
        if (_curCount == 0) return;
        _curCount--; Refresh();
    }

    public void Next()
    {
        if (_savingAll) return;
        if (_curCount == (_count - 1)) return;
        _curCount++; Refresh();
    }

    public void LastLittle()
    {
        if (_savingAll) return;
        _transf.position = new Vector3(_transf.position.x, _transf.position.y, _transf.position.z - 0.05f * Step);
    }

    public void NextLittle()
    {
        if (_savingAll) return;
        _transf.position = new Vector3(_transf.position.x, _transf.position.y, _transf.position.z + 0.05f * Step);
    }

    public void SaveOffset()
    {
        if (_savingAll) return;
        if (CurI >= _offset.Count)
            _offset.AddRange(Enumerable.Repeat(.0f, CurI - _offset.Count + 1));
        _offset[CurI] = _transf.position.z - (fromToZ.x + Step * _curCount);
    }

    public void ResetOffset()
    {
        if (_savingAll) return;
        _transf.position = new Vector3(_transf.position.x, _transf.position.y, (fromToZ.x + Step * _curCount));
    }

    public void OutPutAll()
    {
        if (_savingAll) return;
        _lastI = (int)_curCount;
        _curCount = _count;
        _savingAll = true;
    }

    void Refresh()
    {
        if (CurI >= _offset.Count)
            _transf.position = new Vector3(_transf.position.x, _transf.position.y, (fromToZ.x + Step * _curCount));
        else
            _transf.position = new Vector3(_transf.position.x, _transf.position.y, (fromToZ.x + Step * _curCount) + _offset[CurI]);
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

        if (_savingAll)
        {
            if (_curCount == 0) { _savingAll = false; return; }
            _curCount--;
            if (_curCount >= _offset.Count)
                _transf.position = new Vector3(_transf.position.x, _transf.position.y, (fromToZ.x + Step * _curCount));
            else
                _transf.position = new Vector3(_transf.position.x, _transf.position.y, (fromToZ.x + Step * _curCount) + _offset[(int)_curCount]);
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (SaveFrame || _savingAll)
        {
            SaveFrame = false;
            if (_curCount == 0)
            {
                _curCount = (uint)_lastI;
                _savingAll = false;
            }

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
