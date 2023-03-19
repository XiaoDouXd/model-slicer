using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    private const float Step = 0.05f;
    private const float StepInAnima = 0.02f;
    private const float TxtChangeDelay = 0.8f;
    private enum PinChannel { R, G, B }

    public Text txtLoading;
    public Text txtDot;
    public Image img;
    public Vector2Int colorValFromTo;

    private float from;
    private float to;

    private float r;
    private float g;
    private float b;
    private byte a;
    private PinChannel curPinChannel;

    private float delayTime;
    private float inAnimaFactor;

    private Shadow compShadow;
    private RectTransform txtTransf;
    private float compShadowToY;
    private Vector2 txtTransfToBottom;

    private void Start()
    {
        colorValFromTo.x = Mathf.Clamp(colorValFromTo.x, 0, 255);
        colorValFromTo.y = Mathf.Clamp(colorValFromTo.y, 0, 255);
        from = colorValFromTo.x;
        to = colorValFromTo.y;
        
        compShadow = txtLoading.GetComponent<Shadow>();
        txtTransf = txtLoading.rectTransform;
        compShadowToY = compShadow.effectDistance.y;
        txtTransfToBottom = txtTransf.offsetMin;
        
        curPinChannel = PinChannel.R;
        r = to;
        g = from;
        b = to;
        a = (byte)Mathf.FloorToInt(img.color.a * 255);
        txtDot.text = ". . . .";
        
        inAnimaFactor = 0;
        txtTransf.offsetMin = new Vector2(txtTransfToBottom.x, 0);
        compShadow.effectDistance = new Vector2(compShadow.effectDistance.x, 0);
    }

    private void OnEnable()
    {
        inAnimaFactor = 0;
        if (txtTransf) txtTransf.offsetMin = new Vector2(txtTransfToBottom.x, 0);
        if (compShadow) compShadow.effectDistance = new Vector2(compShadow.effectDistance.x, 0);
    }

    private void Update()
    {
        switch (curPinChannel)
        {
        case PinChannel.R:
            g += Step; b -= Step;
            if (g >= to)
            {
                r = to;
                g = to;
                b = from;
                curPinChannel = PinChannel.G;
            }
            break;
        case PinChannel.G:
            b += Step; r -= Step;
            if (b >= to)
            {
                r = from;
                g = to;
                b = to;
                curPinChannel = PinChannel.B;
            }
            break;
        case PinChannel.B:
            r += Step; g -= Step;
            if (r >= to)
            {
                r = to;
                g = from;
                b = to;
                curPinChannel = PinChannel.R;
            }
            break;
        default:
            throw new ArgumentOutOfRangeException();
        }
        img.color = new Color32((byte)r, (byte)g, (byte)b, a);

        if (inAnimaFactor <= 1)
        {
            var fac = (-Mathf.Cos(inAnimaFactor * Mathf.PI) + 1) / 2f;
            txtTransf.offsetMin = new Vector2(txtTransfToBottom.x, txtTransfToBottom.y * fac);
            compShadow.effectDistance = new Vector2(compShadow.effectDistance.x, compShadowToY * fac);
            inAnimaFactor += StepInAnima;
        }
        
        if (delayTime >= TxtChangeDelay)
        {
            txtDot.text = txtDot.text switch
            {
                "." => ". .",
                ". ." => ". . .",
                ". . ." => ". . . .",
                _ => "."
            };
            delayTime = 0;
        }
        delayTime += Time.deltaTime;
    }
}
