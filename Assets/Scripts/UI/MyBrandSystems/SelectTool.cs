using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Select : MonoBehaviour
{
    //どれだけ中央と座標がずれているか
    private Vector3 m_offset;

    //オブジェクト選択モードか？デフォルトはtrue
    private bool IsSelectMode = true;

    // 現在選択しているオブジェクト
    [NonSerialized] public  Transform targetObject;
    [NonSerialized] public  SpriteRenderer targetRenderer;

    //選択枠
    private GameObject selectionFrame;

    //シール編集エリア
    public Collider2D StickerArea;

    // 選択オブジェクトの元の位置を保存
    private Vector3 originalPosition;

    //LayerUpToolを使うための変数を宣言
    //private LayerUpTool layerUpTool;

    //LayerUpDelectManagerで使うための変数宣言
    //public LayerUpTool targetobject;

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
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y));
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //当たり判定
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        // 既存選択を解除
        Deselect();

        // 2025.12.12 added by ko
        // Cloneable タグなら「複製」して Sticker に変える
        if (hit.collider != null && hit.collider.CompareTag("Cloneable"))
        {
            // 複製生成
            GameObject clone = Instantiate(hit.collider.gameObject, hit.collider.transform.position, Quaternion.identity);

            // 複製されたオブジェクトはもう複製できないようにタグ変更
            clone.tag = "Sticker";

            // 複製されたオブジェクトを新しい選択対象にする
            targetObject = clone.transform;
            targetRenderer = clone.GetComponent<SpriteRenderer>();


            //座標のずれを計算
            m_offset = targetObject.position - worldPosition;

            // 元の位置も保存
            originalPosition = targetObject.position;

            SelectSticker(clone.transform, worldPosition);

            return; // Sticker 選択処理に進まない
        }
        // 2025.12.12 added by ko

        // Sticker を選択した場合の処理
        // オブジェクトが選択されているかつそのオブジェクトがStickerタグを持っている場合
        if (hit.collider != null && hit.collider.CompareTag("Sticker"))
        {
            //選択枠を呼ぶ
            SelectSticker(hit.transform, worldPosition);

            //クリックしたオブジェクトを選択対象として登録
            targetObject = hit.transform;
            targetRenderer = targetObject.GetComponent<SpriteRenderer>();

            //座標のずれを計算
            m_offset = targetObject.position - worldPosition;

            // 元の位置も保存
            originalPosition = targetObject.position;

            

        }

    }

    private void SelectSticker(Transform sticker, Vector3 worldPosition)
    {
        targetObject = sticker;
        targetRenderer = sticker.GetComponent<SpriteRenderer>();

        // 選択枠を取得
        selectionFrame = sticker.Find("SelectSticker")?.gameObject;
        
        if (selectionFrame != null)
            selectionFrame.SetActive(true);

        // 位置情報
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
        // UIの上にカーソルがあったら、入力を受け付けない
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //オブジェクトが選択されていなければ処理しない
        if (targetObject == null) return;

        //シール編集エリアの中に現在位置があるか？
        bool isInsideArea = StickerArea.OverlapPoint(targetObject.position);

        // 枠内なら何もしない
        if (isInsideArea) { return; }

        else
        {
            // 枠外なら削除する
            Destroy(targetObject.gameObject); 
        }
    }


}