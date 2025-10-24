using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldMapToggleButtonUI : TweenUIButton
{
    public void ToggleWorldMap()
    {
        if (WorldMapHandler.Instance.IsVisible)
            WorldMapHandler.Instance.Hide();
        else
            WorldMapHandler.Instance.Show();
    }
}

public class TweenUIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    Tweener activeTweener;
    public void OnPointerEnter(PointerEventData eventData)
    {
        activeTweener?.Kill();
        activeTweener = transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        activeTweener?.Kill();
        activeTweener = transform.DOScale(1f, 0.2f).SetEase(Ease.InSine);
    }
    public void OnPointerDown(PointerEventData eventData)
    {        
        activeTweener?.Kill();
        activeTweener = transform.DOScale(.9f, 0.2f).SetEase(Ease.InOutSine);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        activeTweener?.Kill();
        activeTweener = transform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce);
    }
}
