using UnityEngine;
using UnityEngine.UI;

public class APTransUI : MonoBehaviour
{
    public GameObject contentEmptyRoot;
    public GameObject contentRoot;
    public Models modelRoot;
    
    public InputField xPos;
    public InputField yPos;
    public InputField zPos;

    public InputField xRota;
    public InputField yRota;
    public InputField zRota;
    
    public InputField xScale;
    public InputField yScale;
    public InputField zScale;

    private Transform TargetObj => modelRoot.curSelectedModel;
    private bool emptyState;

    public void OnValueChange(string type)
    {
        if (TargetObj == null) return;
        switch (type)
        {
            case "XPos":
            {
                var val = 0f;
                if (float.TryParse(xPos.text, out var o)) val = o;
                var localPosition = TargetObj.localPosition;
                localPosition =
                    new Vector3(val, localPosition.y, localPosition.z);
                TargetObj.localPosition = localPosition;
                break;
            }
            case "YPos":
            {
                var val = 0f;
                if (float.TryParse(yPos.text, out var o)) val = o;
                var localPosition = TargetObj.localPosition;
                localPosition =
                    new Vector3( localPosition.x, val, localPosition.z);
                TargetObj.localPosition = localPosition;
                break;
            }
            case "ZPos": 
            {
                var val = 0f;
                if (float.TryParse(zPos.text, out var o)) val = o;
                var localPosition = TargetObj.localPosition;
                localPosition =
                    new Vector3(localPosition.x, localPosition.y, val);
                TargetObj.localPosition = localPosition;
                break;
            }
            case "XRota":
            {
                var val = 0f;
                if (float.TryParse(xRota.text, out var o)) val = o;
                var localRotation = TargetObj.localRotation.eulerAngles;
                localRotation =
                    new Vector3(val, localRotation.y, localRotation.z);
                TargetObj.localRotation = Quaternion.Euler(localRotation);
                break;
            }
            case "YRota":
            {
                var val = 0f;
                if (float.TryParse(yRota.text, out var o)) val = o;
                var localRotation = TargetObj.localRotation.eulerAngles;
                localRotation =
                    new Vector3(localRotation.x, val, localRotation.z);
                TargetObj.localRotation = Quaternion.Euler(localRotation);
                break;
            }
            case "ZRota":
            {
                var val = 0f;
                if (float.TryParse(zRota.text, out var o)) val = o;
                var localRotation = TargetObj.localRotation.eulerAngles;
                localRotation =
                    new Vector3(localRotation.x, localRotation.y, val);
                TargetObj.localRotation = Quaternion.Euler(localRotation);
                break;
            }
            case "XScale":
            {
                var val = 0f;
                if (float.TryParse(xScale.text, out var o)) val = o;
                var localScale = TargetObj.localScale;
                localScale =
                    new Vector3(val, localScale.y, localScale.z);
                TargetObj.localScale = localScale;
                break;
            }
            case "YScale":
            {
                var val = 0f;
                if (float.TryParse(yScale.text, out var o)) val = o;
                var localScale = TargetObj.localScale;
                localScale =
                    new Vector3(localScale.x, val, localScale.z);
                TargetObj.localScale = localScale;
                break;
            }
            case "ZScale":
            {
                var val = 0f;
                if (float.TryParse(yScale.text, out var o)) val = o;
                var localScale = TargetObj.localScale;
                localScale =
                    new Vector3(localScale.x, localScale.y, val);
                TargetObj.localScale = localScale;
                break;
            }
        }
    }

    private void Start()
    {
        emptyState = true;
    }

    private void Update()
    {
        if (!TargetObj && !emptyState)
        {
            contentRoot.SetActive(false);
            contentEmptyRoot.SetActive(true);
            emptyState = true;
        }
        else if (emptyState && TargetObj)
        {
            contentRoot.SetActive(true);
            contentEmptyRoot.SetActive(false);

            var pos = TargetObj.localPosition;
            xPos.text = pos.x.ToString("F2");
            yPos.text = pos.y.ToString("F2");
            zPos.text = pos.z.ToString("F2");
            var rota = TargetObj.localRotation.eulerAngles;
            xRota.text = rota.x.ToString("F2");
            yRota.text = rota.y.ToString("F2");
            zRota.text = rota.z.ToString("F2");
            var scale = TargetObj.localScale;
            xScale.text = scale.x.ToString("F2");
            yScale.text = scale.y.ToString("F2");
            zScale.text = scale.z.ToString("F2");
            emptyState = false;
        }
    }
}
