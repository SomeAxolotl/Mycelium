using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
[RequireComponent(typeof(ScrollRect))]
[RequireComponent(typeof(ScrollRect))]
public class AutoScroll : MonoBehaviour {
 
    public ScrollRect    scrollRect;
    public RectTransform viewportRectTransform;
    public RectTransform contentRectTransform;
 
    RectTransform selectedRectTransform;
 
    void Update() {
        var selected = EventSystem.current.currentSelectedGameObject;
       
        if (selected == null) return;
 
        
        if (!selected.transform.IsChildOf(contentRectTransform)) return;
 
        selectedRectTransform = selected.GetComponent<RectTransform>();
        var viewportRect = viewportRectTransform.rect;
       
        
        var selectedRect         = selectedRectTransform.rect;
        var selectedRectWorld    = selectedRect.Transform(selectedRectTransform);
        var selectedRectViewport = selectedRectWorld.InverseTransform(viewportRectTransform);
       
       
        var outsideOnTop    = selectedRectViewport.yMax - viewportRect.yMax;
        var outsideOnBottom = viewportRect.yMin - selectedRectViewport.yMin;
       
       
        if (outsideOnTop < 0) outsideOnTop       = 0;
        if (outsideOnBottom < 0) outsideOnBottom = 0;
       
        
        var delta = outsideOnTop > 0 ? outsideOnTop : -outsideOnBottom;
       
        
        if (delta == 0) return;
       
        
        var contentRect         = contentRectTransform.rect;
        var contentRectWorld    = contentRect.Transform(contentRectTransform);
        var contentRectViewport = contentRectWorld.InverseTransform(viewportRectTransform);
 
       
        var overflow = contentRectViewport.height - viewportRect.height;
 
       
        var unitsToNormalized = 1 / overflow;
        scrollRect.verticalNormalizedPosition += delta * unitsToNormalized;
    }  
}
 
internal static class RectExtensions {
   
    public static Rect Transform(this Rect r, Transform transform) {
        return new Rect {
            min = transform.TransformPoint(r.min),
            max = transform.TransformPoint(r.max),
        };
    }
   
   
    public static Rect InverseTransform(this Rect r, Transform transform) {
        return new Rect {
            min = transform.InverseTransformPoint(r.min),
            max = transform.InverseTransformPoint(r.max),
        };
    }
}