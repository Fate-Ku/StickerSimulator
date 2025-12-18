using UnityEngine;
using UnityEngine.UIElements;


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
    private int maxStep = 5;

    //最小の段階
    private int minStep = -4;

    //1段階のサイズの変化量
    private float changeScale = 0.2f;

    Transform target;

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

        //段階が最大でない場合、拡大
        if (scaleStep < maxStep)
        {
            //段階を1増やす
            scaleStep++;

            //デフォルトのサイズを基準にサイズ変更
            select.targetObject.transform.localScale = defaultSize * (1 + scaleStep * changeScale);

        }

    }

    //オブジェクトの縮小
    public void ScaleDown()
    {
        //選択状態でなければ処理しない
        if (select.targetObject == null) { return; }

        //段階が最小でない場合、縮小
        if (scaleStep > minStep)
        {
            //段階を1減らす
            scaleStep--;

            //デフォルトのサイズを基準にサイズ変更
            select.targetObject.transform.localScale = defaultSize * (1 + scaleStep * changeScale);


        }

    }

    //新しいオブジェクトを設定する関数
    public void SetTarget(Transform target)
    {
        //選択オブジェクトを変更する
        select.targetObject = target;

        //選択されていれば
        if (select.targetObject != null)
        {
            //オブジェクトの元のサイズを保存
            defaultSize = select.targetObject.localScale;
        }
    }


}