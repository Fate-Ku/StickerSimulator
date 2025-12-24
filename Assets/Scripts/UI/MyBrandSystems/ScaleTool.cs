using UnityEngine;


public class ScaleTool : MonoBehaviour
{
    [SerializeField] private Select select;

    //元のサイズを保存
    private Vector2 defaultSize;

    //最後に選択していたオブジェクト
    private Transform lastTarget;

    //現在のサイズの段階
    private int scaleStep = 0;

    //最大の段階
    private int maxStep = 10;

    //最小の段階
    private int minStep = -4;

    //1段階のサイズの変化量
    private float changeScale = 0.2f;

    public void Update()
    {
        var target = select.targetObject;

        //選択しているオブジェクトが同じなら変更しない
        if (target == lastTarget) return;

        lastTarget = target;
        SetTarget(target);
    }

    //オブジェクトの拡大
    public void ScaleUp()
    {
        //選択状態でなければ処理しない
        if (select.targetObject == null) { return; }

        Transform t = select.targetObject;

        float factor = 1f + changeScale;

        Vector3 nextScale = t.localScale * factor;

        float maxScale = 1.7f;

        nextScale.x = Mathf.Min(nextScale.x, maxScale);
        nextScale.y = Mathf.Min(nextScale.y, maxScale);
        nextScale.z = Mathf.Min(nextScale.z, maxScale);

        t.localScale = nextScale;

    }

    //オブジェクトの縮小
    public void ScaleDown()
    {
        //選択状態でなければ処理しない
        if (select.targetObject == null) { return; }

        Transform t = select.targetObject;

        float factor = 1.0f + changeScale;

        Vector3 nextScale = t.localScale / factor;

        float minScale = 0.1f;

        nextScale.x = Mathf.Max(nextScale.x, minScale);
        nextScale.y = Mathf.Max(nextScale.y, minScale);
        nextScale.z = Mathf.Max(nextScale.z, minScale);

        t.localScale = nextScale;

    }

    //新しいオブジェクトを選択したときに呼ぶ関数
    public void SetTarget(Transform target)
    {
        //選択されていなければ処理しない
        if (target == null) return;

        select.targetObject = target;

        //選択が変わったら初期化
        defaultSize = target.localScale;
    }
}