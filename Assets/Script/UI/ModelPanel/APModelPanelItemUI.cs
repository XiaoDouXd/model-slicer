using UnityEngine;
using UnityEngine.UI;

public class APModelPanelItemUI : MonoBehaviour
{
    public APModelPanelUI root;
    public Text nameTxt;
    public Button addBtn;
    public Button delBtn;
    
    public string Txt { set => nameTxt.text = value; }
    public Color TxtColor { set => nameTxt.color = value; }

    private int i;
    private bool isActive;

    public void OnBtnAdd() { root.OnAddItem(i); }
    public void OnBtnDel() 
    { 
        root.OnDelItem(i);
        isActive = false;
        Refresh();
    }
    public void OnSelected() { root.OnSelected(i); }

    public void OnAddSuccess(string fileName)
    {
        isActive = true;
        nameTxt.text = fileName;
        Refresh();
    }

    private void Refresh()
    {
        if (isActive)
        {
            addBtn.gameObject.SetActive(false);
            delBtn.gameObject.SetActive(true);
            nameTxt.gameObject.SetActive(true);
        }
        else
        {
            addBtn.gameObject.SetActive(true);
            delBtn.gameObject.SetActive(false);
            nameTxt.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        isActive = false;
        if (int.TryParse(transform.name[^1..], out var idx)) i = idx;
        else i = -1;
        Refresh();
    }
}
