using UnityEditor.Tilemaps;
using UnityEngine;


public class Select:MonoBehaviour
{
    // 現在選択しているオブジェクト
    private Transform targetObject;

    //どれだけ中央と座標がずれているか
    private Vector3 m_offset;

    //マウスが押された
    private void OnMouseDown()
    {

        //マウスポインタの取得
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y));

        //中央とマウスの座標のずれを計算
        m_offset = new Vector2(transform.position.x - worldPosition.x, transform.position.y - worldPosition.y);
    
    }
    //マウスがドラッグされた
    private void OnMouseDrag()
    {
        //マウスポインタの取得
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector2(mousePosition.x, mousePosition.y));

        //オブジェクトのどこを掴んでも良いようにする、画面の外に出ないようにする
        transform.position = new Vector2(Mathf.Clamp(worldPosition.x + m_offset.x, -8.0f, 8.0f), Mathf.Clamp(worldPosition.y + m_offset.y, -4.0f, 4.0f));

    }
}
