using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropSystem:MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    Transform parentAfterDrag;

    //ドラッグし始めた
    public void OnBeginDrag(PointerEventData eventData)
    {
        //ドラッグ可能アイテムを最前面にする
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    //ドラッグしている
    public void OnDrag(PointerEventData eventData)
    {
        //マウスの位置でドラッグ
        transform.position = Input.mousePosition;
    }

    //ドラッグが終わった
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
    }
    
}
