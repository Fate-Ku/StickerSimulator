using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Select : MonoBehaviour
{
    public LayerControllerTool layerTool;

    //どれだけ中央と座標がずれているか
    private Vector3 m_offset;

    //オブジェクト選択モードか？デフォルトはtrue
    private bool IsSelectMode = true;

    //シールドラッグ状態か？
    private bool isDraggingSticker = false;

    // 現在選択しているオブジェクト
    [NonSerialized] public  Transform targetObject;
    [NonSerialized] public  SpriteRenderer targetRenderer;

    //選択枠
    private GameObject selectionFrame;

    //シール編集エリア
    public Collider2D StickerArea;

    // 選択オブジェクトの元の位置を保存
    private Vector3 originalPosition;

    //選択状態をオフにしておく
    public void Start()
    {
        targetRenderer = null;
        targetObject = null;
        selectionFrame = null;
    }

    void Update()
    {

        //左クリックが押された
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }


        //マウスがドラッグされた
        if (Input.GetMouseButton(0))
        {
            OnMouseDrag();
        }

        //左クリックが離された
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
        }
    }

    //ボタンを押すとオブジェクト選択モードに移行または解除
    public void OnButtonDown()
    {
        switch (IsSelectMode)
        {
            //選択モード・選択オブジェクトの解除
            case true:

                // 以前の選択オブジェクトがあれば選択枠を非表示にし、選択解除
                Deselect();

                //オブジェクト選択モード解除
                IsSelectMode = false;
                break;

            //選択モード移行
            case false:

                //オブジェクト選択モードにする
                IsSelectMode = true;
                break;
        }
    }

    //マウスが押された
    public void OnMouseDown()
    {
        // UIの上にカーソルがあったら、入力を受け付けない
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //オブジェクト選択モードでなければ処理しない
        if (!IsSelectMode) { return; }

        //マウスポインタの取得
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //当たり判定
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPosition, Vector2.zero);

        if (hits.Length == 0) return;

        // 一番前にあるものを探す
        RaycastHit2D? bestHit = null;
        int highestOrder = int.MinValue;

        foreach (var h in hits)
        {
            if (h.collider == null) continue;

            // 親のSortingGroupを取得（子クリック対策）
            SortingGroup sg = h.collider.GetComponentInParent<SortingGroup>();
            if (sg == null) continue;

            if (sg.sortingOrder > highestOrder)
            {
                highestOrder = sg.sortingOrder;
                bestHit = h;
            }
        }

        if (!bestHit.HasValue) return;

        RaycastHit2D hit = bestHit.Value;

        // 既存選択解除
        Deselect();

        // ===============================
        // Cloneable の場合
        // ===============================
        if (hit.collider.CompareTag("Cloneable"))
        {
            GameObject clone = Instantiate(
                hit.collider.gameObject,
                hit.collider.transform.position,
                Quaternion.identity
            );

            clone.tag = "Sticker";

            foreach (Transform child in clone.transform)
            {
                if (child.name != "SelectSticker")
                    child.tag = "Sticker";
            }

            SortingGroup sg = clone.GetComponent<SortingGroup>();
            if (sg != null && layerTool != null)
            {
                layerTool.RegisterNewLayer(sg);
            }

            targetObject = clone.transform;
            targetRenderer = clone.GetComponent<SpriteRenderer>();

            m_offset = targetObject.position - worldPosition;
            originalPosition = targetObject.position;

            SelectSticker(clone.transform, worldPosition);

            isDraggingSticker = true;
            return;
        }

        // ===============================
        // Sticker の場合
        // ===============================
        if (hit.collider.CompareTag("Sticker"))
        {
            Transform root = hit.transform;

            if (root.parent != null && root.parent.CompareTag("Sticker"))
            {
                root = root.parent;
            }

            SelectSticker(root, worldPosition);

            targetObject = root;
            targetRenderer = root.GetComponent<SpriteRenderer>();

            if (layerTool != null)
            {
                SortingGroup sg = root.GetComponent<SortingGroup>();
                if (sg != null)
                {
                    layerTool.SetSelectedIndexFromSticker(sg);
                }
            }

            m_offset = targetObject.position - worldPosition;
            originalPosition = targetObject.position;

            isDraggingSticker = true;
        }
    }

    //シールの選択枠を表示する
    private void SelectSticker(Transform sticker, Vector3 worldPosition)
    {
        targetObject = sticker;
        targetRenderer = sticker.GetComponent<SpriteRenderer>();

        //選択枠を取得
        if (sticker.Find("SelectSticker") != null)
            selectionFrame = sticker.Find("SelectSticker").gameObject;

        //表示
        if (selectionFrame != null)
            selectionFrame.SetActive(true);

        //位置情報
        m_offset = targetObject.position - worldPosition;
        originalPosition = targetObject.position;
    }

    // 選択解除
    private void Deselect()
    {
        if (selectionFrame != null)
            selectionFrame.SetActive(false);

        targetObject = null;
        targetRenderer = null;
        selectionFrame = null;
    }

    //マウスがドラッグされた
    private void OnMouseDrag()
    {
        if (!isDraggingSticker) return;

        // UIの上にカーソルがあったら、入力を受け付けない
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //オブジェクトが選択されていなければ処理しない
        if (targetObject == null) { return; }

        //マウスポインタの取得
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y));

        //オブジェクトのどこを掴んでも良いようにする、画面の外に出ないようにする
        targetObject.position = new Vector2(Mathf.Clamp(worldPosition.x + m_offset.x, -8.0f, 8.0f), Mathf.Clamp(worldPosition.y + m_offset.y, -4.0f, 4.0f));

    }

    //マウスが離された
    private void OnMouseUp()
    {
        //シール選択状態解除
        isDraggingSticker = false;

        //オブジェクトが選択されていなければ処理しない
        if (targetObject == null || StickerArea == null) return;

        Collider2D col = targetObject.GetComponent<Collider2D>();
        if (col == null) return;

        Bounds objBounds = col.bounds;
        Bounds areaBounds = StickerArea.bounds;

        bool wasOutside = !StickerArea.OverlapPoint(originalPosition);
        bool isOutsideNow = !StickerArea.OverlapPoint(targetObject.position);

        //外→外の場合だけ削除する
        if (wasOutside && isOutsideNow)
        {
            Destroy(targetObject.gameObject);
            return;
        }

        Vector3 pos = targetObject.position;

        // X方向補正
        if (objBounds.min.x < areaBounds.min.x)
            pos.x += areaBounds.min.x - objBounds.min.x;
        else if (objBounds.max.x > areaBounds.max.x)
            pos.x -= objBounds.max.x - areaBounds.max.x;

        // Y方向補正
        if (objBounds.min.y < areaBounds.min.y)
            pos.y += areaBounds.min.y - objBounds.min.y;
        else if (objBounds.max.y > areaBounds.max.y)
            pos.y -= objBounds.max.y - areaBounds.max.y;

        targetObject.position = pos;
    }


}