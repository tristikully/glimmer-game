using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;

public class InventoryPanel : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform topBar; // Inventory panel

    public RectTransform[] uiElements; // Array of UI elements to detect hover

    public float panel_moveDistance = 100f;
    public float in_moveDuration = 0.3f;
    public float out_moveDuration = 0.5f;

    private Vector2 panel_hiddenPosition;
    private Vector2 panel_visiblePosition;

    //private bool mouseHover = false;
    private bool panelVisible = false;
    private Coroutine hideCoroutine;

    private bool IsMouseOverUIElements()
    {
        foreach (RectTransform uiElement in uiElements)
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(uiElement, Input.mousePosition, null, out localPoint) &&
                uiElement.rect.Contains(localPoint))
            {
                return true;
            }
        }
        return false;
    }

    private void Start()
    {
        // Initialize the panel positions
        panel_visiblePosition = new Vector2(topBar.anchoredPosition.x, topBar.anchoredPosition.y - panel_moveDistance);
        panel_hiddenPosition = topBar.anchoredPosition;

        topBar.anchoredPosition = panel_hiddenPosition;
    }

    void Update()
    {
        bool isMouseOverUI = IsMouseOverUIElements();

        if (isMouseOverUI && !panelVisible)
        {
            topBar.DOAnchorPos(panel_visiblePosition, in_moveDuration).SetEase(Ease.OutQuad);
            panelVisible = true;

            // Stop any existing hide coroutine when the panel is shown
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
                hideCoroutine = null;
            }
        }
        else if (isMouseOverUI && panelVisible)
        {
            // Start the coroutine to hide the panel with a delay
            if (hideCoroutine == null)
            {
                hideCoroutine = StartCoroutine(HidePanelAfterDelay());
            }
        }
    }

    private IEnumerator HidePanelAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        
        if (!IsMouseOverUIElements())
        {
            topBar.DOAnchorPos(panel_hiddenPosition, out_moveDuration).SetEase(Ease.InQuad);
            panelVisible = false;
        }

        hideCoroutine = null;
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    mouseHover = true;
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    mouseHover = false;
    //}
}