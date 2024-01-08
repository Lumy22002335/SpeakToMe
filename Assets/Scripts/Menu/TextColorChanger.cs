using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextColorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Text")]
    [SerializeField] private TMP_Text _textMesh;

    private Color _baseColor;
    private Color _hoverColor;
    private Color _pressedColor;

    private bool _isHovering;
    private bool _isPressing;

    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _baseColor = _button.colors.normalColor;
        _hoverColor = _button.colors.highlightedColor;
        _pressedColor = _button.colors.pressedColor;
    }

    private void Update()
    {
        if (!_isHovering && EventSystem.current.currentSelectedGameObject != gameObject)
        {
            _textMesh.color = _baseColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovering = true;
        _textMesh.color = _hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;
        if (_isPressing)
        {
            _textMesh.color = _pressedColor;
        }
        else if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            _textMesh.color = _hoverColor;
        }
        else
        {
            _textMesh.color = _baseColor;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressing = true;
        _textMesh.color = _pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressing = false;
        if (_isHovering || EventSystem.current.currentSelectedGameObject == gameObject)
        {
            _textMesh.color = _hoverColor;
        }
        else
        {
            _textMesh.color = _baseColor;
        }
    }
}
