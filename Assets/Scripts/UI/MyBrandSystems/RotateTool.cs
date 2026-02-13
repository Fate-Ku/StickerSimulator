using UnityEngine;
using UnityEngine.UIElements;

public class RotateTool : MonoBehaviour
{
    [SerializeField] public Select select;

    //最後に選択していたオブジェクト
    private Transform lastTarget;


    //シール編集エリア
    public Collider2D StickerArea;

    public void Update()
    {
        if (select == null) return; // 何も選択しないなら

        var target = select.targetObject;

        if (target == null) return; // // 何も選択しないなら

        //選択しているオブジェクトが同じなら変更しない
        if (target == lastTarget) return;

        lastTarget = target;
        SetTarget(target);
    }

    //新しいオブジェクトを設定する関数
    public void SetTarget(Transform target)
    {
        //選択オブジェクトを変更する
        select.targetObject = target;

    }

    //左に15度回転
    public void RotateLeft()
    {
        if (select.targetObject == null) return;

        select.targetObject.Rotate(0, 0, 15.0f, Space.World);

        ClampInsideArea();
    }

    //右に15度回転
    public void RotateRight()
    {
        if (select.targetObject == null) return;

        select.targetObject.Rotate(0, 0, -15.0f, Space.World);

        ClampInsideArea();
    }

    private void ClampInsideArea()
    {
        if (select.targetObject == null || StickerArea == null) return;

        Collider2D col = select.targetObject.GetComponent<Collider2D>();
        if (col == null) return;

        Bounds objBounds = col.bounds;
        Bounds areaBounds = StickerArea.bounds;

        Vector3 pos = select.targetObject.position;

        if (objBounds.min.x < areaBounds.min.x)
            pos.x += areaBounds.min.x - objBounds.min.x;
        else if (objBounds.max.x > areaBounds.max.x)
            pos.x -= objBounds.max.x - areaBounds.max.x;

        if (objBounds.min.y < areaBounds.min.y)
            pos.y += areaBounds.min.y - objBounds.min.y;
        else if (objBounds.max.y > areaBounds.max.y)
            pos.y -= objBounds.max.y - areaBounds.max.y;

        select.targetObject.position = pos;
    }

}
