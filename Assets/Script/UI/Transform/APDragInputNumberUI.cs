using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class APDragInputNumberUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InputField input;

    private Text _lable;
    private static bool _isDraggingOther;
    private bool _isSelfDragging;
    
    private void Start()
    {
        _isSelfDragging = false;
        _lable = GetComponent<Text>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isSelfDragging || !eventData.IsPointerMoving()) return;

        var num = 0f;
        _lable.color = Color.red;
        if (float.TryParse(input.text, out var val)) num = val;
        var delta = eventData.delta;
        input.text = input.characterValidation switch
        {
            InputField.CharacterValidation.Decimal => (num + (delta.x + delta.y) * 0.02f).ToString("F2"),
            InputField.CharacterValidation.Integer => Mathf.FloorToInt(num + (delta.x + delta.y) * 0.5f).ToString(),
            _ => input.text
        };
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_isDraggingOther) _isSelfDragging = _isDraggingOther = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isSelfDragging) _isSelfDragging = _isDraggingOther = false;
        _lable.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isDraggingOther) return;
        _lable.color = Color.red;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        _lable.color = Color.white;
    }
}
