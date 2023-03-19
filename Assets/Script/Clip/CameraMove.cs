using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraMove : MonoBehaviour
{
    public LoadingUI cover;
    
    public Vector2 fromToZ = new(-3.847f, 3.82f);
    public string path = "OutPut~";

    public bool Saving => _savingAll;
    public float Step => (fromToZ.y - fromToZ.x) / (_count - 1);
    public Vector2 PosLimit =>
        new(fromToZ.x + Step * CurI - 0.5f * Step,
            fromToZ.x + Step * CurI + 0.5f * Step);

    public int CurI 
    { 
        get => (int)_curCount; 
        set 
        {
            if (_savingAll) return;
            if (value <= -1 || value >= _count) return;
            var refreshCheck = value != _curCount;
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
    private uint _curCount;
    private uint _count = 256;
    private List<float> _offset = new();
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
        var position = _transf.position;
        position = new Vector3(position.x, position.y, position.z - 0.05f * Step);
        _transf.position = position;
    }

    public void NextLittle()
    {
        if (_savingAll) return;
        var position = _transf.position;
        position = new Vector3(position.x, position.y, position.z + 0.05f * Step);
        _transf.position = position;
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
        var position = _transf.position;
        position = new Vector3(position.x, position.y, (fromToZ.x + Step * _curCount));
        _transf.position = position;
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
        {
            var position = _transf.position;
            position = new Vector3(position.x, position.y, (fromToZ.x + Step * _curCount));
            _transf.position = position;
        }
        else
        {
            var position = _transf.position;
            position = new Vector3(position.x, position.y,
                (fromToZ.x + Step * _curCount) + _offset[CurI]);
            _transf.position = position;
        }
    }

    // ------------------------------------------------------------

    private void Awake()
    {
        cover = GetComponent<LoadingLink>().cover;
        _transf = GetComponent<Transform>();
        if (!_transf) Debug.LogError("Cant find transform component in the obj self");
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var limit = PosLimit;
        var zPos = _transf.position.z;

        if (zPos < limit.x)
        {
            var position = _transf.position;
            position = new Vector3(position.x, position.y, limit.x);
            _transf.position = position;
        }
        else if (zPos > limit.y)
        {
            var position = _transf.position;
            position = new Vector3(position.x, position.y, limit.y);
            _transf.position = position;
        }

        if (_savingAll)
        {
            if (!cover.gameObject.activeSelf) { cover.gameObject.SetActive(true); }
            if (_curCount == 0) { _savingAll = false; cover.gameObject.SetActive(false); return; }
            _curCount--;
            if (_curCount >= _offset.Count)
            {
                var position = _transf.position;
                position = new Vector3(position.x, position.y, (fromToZ.x + Step * _curCount));
                _transf.position = position;
            }
            else
            {
                var position = _transf.position;
                position = new Vector3(position.x, position.y, (fromToZ.x + Step * _curCount) + _offset[(int)_curCount]);
                _transf.position = position;
            }
        }
    }

    private Texture2D _saveTex;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_savingAll)
        {
            if (!_saveTex) _saveTex = new Texture2D(4096, 4096, TextureFormat.RGBA32, false);
            if (_curCount == 0)
            {
                _curCount = (uint)_lastI;
                _savingAll = false;

                var posX = (int)_curCount % 16;
                var posY = (int)_curCount / 16;
                RenderTexture.active = source;
                _saveTex.ReadPixels(new Rect(0, 0, source.width, source.height), 
                    posX * source.width, posY * source.height);
                _saveTex.Apply();
                RenderTexture.active = null;
                
                var bytes = _saveTex.EncodeToPNG();
                if (!System.IO.File.Exists(Application.streamingAssetsPath + $"/{path}/OutPut.png"))
                {
                    if (!System.IO.Directory.Exists(Application.streamingAssetsPath + $"/{path}/"))
                        System.IO.Directory.CreateDirectory(Application.streamingAssetsPath + $"/{path}/");
                    var s = System.IO.File.Create(Application.streamingAssetsPath + $"/{path}/OutPut.png");
                    s.Write(bytes); s.Close();
                }
                else
                {
                    System.IO.File.WriteAllBytes(Application.streamingAssetsPath + $"/{path}/OutPut.png", bytes);
                }
                Destroy(_saveTex);
                _saveTex = null;
                cover.gameObject.SetActive(false);
            }
            else
            {
                var posX = (int)_curCount % 16;
                var posY = (int)_curCount / 16;
                RenderTexture.active = source;
                _saveTex.ReadPixels(new Rect(0, 0, source.width, source.height), 
                    posX * source.width, posY * source.height);
                RenderTexture.active = null;
            }
        }
        
        if (SaveFrame)
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
