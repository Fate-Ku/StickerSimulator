using UnityEngine;

public class ScaleTool : MonoBehaviour
{
    [SerializeField] private Select select;
    [SerializeField] private float changeScale = 0.2f;

    //シール編集エリア
    [SerializeField] private Collider2D stickerArea;

    public void ScaleUp()
    {
        //1.2倍
        Scale(1f + changeScale);
    }

    public void ScaleDown()
    {
        //0.83倍(1f + changeScale)は元の大きさに戻せるようにするため
        Scale(1f / (1f + changeScale));
    }

    //magnification→倍率
    private void Scale(float magnification)
    {
        if (select == null) return;
        if (select.targetObject == null) return;

        Transform target = select.targetObject;

        //シールの大小の上限を取得
        ScaleLimit limit = target.GetComponent<ScaleLimit>();

        //現在の大きさから倍率をかけて大きさを変化する
        Vector3 nextScale = target.localScale * magnification;

        //Mathf.Clamp(値, 最小値, 最大値)
        //値を設定した大小の上限より大きく（小さく）ならないようにする
        target.localScale = new Vector3(
            Mathf.Clamp(nextScale.x, limit.minScale.x, limit.maxScale.x),
            Mathf.Clamp(nextScale.y, limit.minScale.y, limit.maxScale.y),
            Mathf.Clamp(nextScale.z, limit.minScale.z, limit.maxScale.z)
        );

        Collider2D col = target.GetComponent<Collider2D>();
        if (col == null || stickerArea == null) return;

        Bounds objBounds = col.bounds;
        Bounds areaBounds = stickerArea.bounds;

        Vector3 pos = target.position;

        //X方向補正
        if (objBounds.min.x < areaBounds.min.x)
            pos.x += areaBounds.min.x - objBounds.min.x;
        else if (objBounds.max.x > areaBounds.max.x)
            pos.x -= objBounds.max.x - areaBounds.max.x;

        //Y方向補正
        if (objBounds.min.y < areaBounds.min.y)
            pos.y += areaBounds.min.y - objBounds.min.y;
        else if (objBounds.max.y > areaBounds.max.y)
            pos.y -= objBounds.max.y - areaBounds.max.y;

        target.position = pos;
    }
}