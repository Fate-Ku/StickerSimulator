using UnityEngine;
using UnityEngine.EventSystems;

public class Select : MonoBehaviour
{
    //どれだけ中央と座標がずれているか
    private Vector3 m_offset;

    //オブジェクト選択モードか？デフォルトはtrue
    private bool IsSelectMode = true;

    // 現在選択しているオブジェクト
    private Transform targetObject;
    private SpriteRenderer targetRenderer;

    // 元の色を保存する変数
    private Color defaultColor;

    [SerializeField] private LayerMask stickerMask;

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }

        if (Input.GetMouseButton(0))
        {
            OnMouseDrag();
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


                IsSelectMode = false;
                break;

            //選択モード移行
            case false:
                IsSelectMode = true;
                break;
        }
    }


    //マウスが押された
    public void OnMouseDown()
    {

        //オブジェクト選択モードでなければ処理しない
        if (!IsSelectMode) { return; }

        //マウスポインタの取得
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y));

        //当たり判定
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, 10f, stickerMask);

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



        }

    }
    //マウスがドラッグされた
    private void OnMouseDrag()
    {
        //Stickerオブジェクトが選択されていなければ処理しない
        if (targetObject == null) { return; }

        //マウスポインタの取得
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y));

        //オブジェクトのどこを掴んでも良いようにする、画面の外に出ないようにする
        targetObject.position = new Vector2(Mathf.Clamp(worldPosition.x + m_offset.x, -8.0f, 8.0f), Mathf.Clamp(worldPosition.y + m_offset.y, -4.0f, 4.0f));

    }
}