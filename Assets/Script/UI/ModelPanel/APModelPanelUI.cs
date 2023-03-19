using System.IO;
using TriLibCore;
using UnityEngine;

public class APModelPanelUI : MonoBehaviour
{
    public GameObject loadingUI;
    public GameObject egModel;
    public Models modelsRoot;
    public GameObject itemsRoot;

    private GameObject[] objectList;
    private APModelPanelItemUI[] itemList;
    private uint curObjCount;
    [HideInInspector] public int curSelected;
    
    private bool Load(string path, int i)
    {
        if (curObjCount >= 8) return false;
        if (objectList[i] != null) return false;

        loadingUI.SetActive(true);
        objectList[i] = new GameObject($"Model_{i}");
        objectList[i].transform.SetParent(modelsRoot.transform, false);
        AssetLoader.LoadModelFromFile(path, 
            null,
            context => {
                foreach (var obj in context.GameObjects.Values)
                {
                    if (!obj.TryGetComponent<MeshRenderer>(out var comp)) continue;
                    var mat = Resources.Load<Material>("Shaders/clipSurf");
                    comp.material = mat;
                    comp.materials[0] = mat;
                }
                OnAddSuccess(path, i);
            },
            null,
            _ =>
            {
                Destroy(objectList[i]);
                objectList[i] = null;
                loadingUI.SetActive(false);
            }, 
            objectList[i]);
        return true;
    }

    private void OnAddSuccess(string filePath, int i)
    {
        curObjCount++;
        itemList[i].OnAddSuccess(Path.GetFileName(filePath));
        if (curObjCount == 1) OnSelected(i);
        if (egModel) egModel.SetActive(false);
        loadingUI.SetActive(false);
    }
    
    public void OnAddItem(int i)
    {
        var filePath = OpenFileWin32.OpenFile();
        if (string.IsNullOrEmpty(filePath))
            Debug.LogError("APModelPanel: can't find file selected.");
        if (!File.Exists(filePath)) return;
        if (!Load(filePath, i)) Debug.LogError("APModelPanelUI: Instantiate model file failure.");
    }

    public void OnDelItem(int i)
    {
        if (objectList[i] == null) return;
        
        objectList[i].SetActive(false);
        Destroy(objectList[i]);
        objectList[i] = null;
        curObjCount--;
        itemList[i].Txt = "";
        itemList[i].TxtColor = Color.white;
        if (curObjCount == 0)
        {
            if (egModel) egModel.SetActive(true);
            OnSelected(-1);
        }
        else
        {
            for (i = 0; i < 8; i++)
            {
                if (objectList[i] == null) continue;
                OnSelected(i);
                return;
            }
        }
        OnSelected(-1);
    }

    public void OnSelected(int i)
    {
        if (i < 0)
        {
            curSelected = -1;
            modelsRoot.curSelectedModel = null;
            return;
        }
        
        if (objectList[i] == null) return;
        curSelected = i;
        modelsRoot.curSelectedModel = objectList[curSelected].transform;
    }
    
    private void Start()
    {
        objectList = new GameObject[8];
        itemList = new APModelPanelItemUI[8];
        OnSelected(-1);
        modelsRoot.curSelectedModel = null;
        
        for (var i = 0; i < 8; i++)
            itemList[i] = itemsRoot.transform.Find($"Model_{i}").
                gameObject.GetComponent<APModelPanelItemUI>();
    }
}
