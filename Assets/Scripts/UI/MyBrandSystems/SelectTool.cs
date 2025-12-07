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
    [NonSerialized] public static Transform targetObject;
    [NonSerialized] public static SpriteRenderer targetRenderer;

    //シール編集エリア
    public Collider2D StickerArea;

    // 選択オブジェクトの元の位置を保存
    private Vector3 originalPosition;


    // 元の色を保存する変数
    [NonSerialized] public static Color defaultColor;

    //選択状態をオフにしておく
    public void Start()
    {
        targetRenderer = null;
        targetObject = null;
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

                // 以前の選択オブジェクトがあれば色を戻し、選択解除
                if (targetRenderer != null)
                {
                    targetRenderer.color = defaultColor;

                    targetRenderer = null;
                    targetObject = null;
                }

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
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y));

        //当たり判定
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
       


        // 以前の選択オブジェクトがあれば色を戻し、選択解除
        if (targetRenderer != null)
        {
            targetRenderer.color = defaultColor;

            targetRenderer = null;
            targetObject = null;
         }

        //オブジェクトが選択されているかつそのオブジェクトがStickerタグを持っている場合
        if (hit.collider != null && hit.collider.CompareTag("Sticker"))
        {

            //クリックしたオブジェクトを選択対象として登録
            targetObject = hit.transform;
            targetRenderer = targetObject.GetComponent<SpriteRenderer>();


            //元の色を保存
            defaultColor = targetRenderer.color;

            //色の変更
            targetRenderer.color = new Color(0.8f, 0.8f, 0.8f);

            //座標のずれを計算
            m_offset = targetObject.position - worldPosition;

            // 元の位置も保存
            originalPosition = targetObject.position;

        }

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
        //オブジェクトが選択されていなければ処理しない
        if (targetObject == null) return;

        //シール編集エリアの中に現在位置があるか？
        bool isInsideArea = StickerArea.OverlapPoint(targetObject.position);

        // 枠内なら何もしない
        if (isInsideArea) { return; }

        else
        {
            // 枠外なら元の位置に戻す
            targetObject.position = originalPosition;
        }
    }
}