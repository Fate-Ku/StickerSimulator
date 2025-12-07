using UnityEngine;
using UnityEngine.UIElements;


public class ScaleTool : MonoBehaviour
{

    //元のサイズを保存
    private Vector2 defaultSize;

    //現在のサイズの段階
    private int scaleStep = 0;

    //最大の段階
    private int maxStep = 5;

    //最小の段階
    private int minStep = -4;

    //1段階のサイズの変化量
    private float changeScale = 0.2f;

    public void Start()
    {
        //新しいオブジェクトを設定
        SetTarget(transform);

        //オブジェクトの元のサイズを保存
        defaultSize = Select.targetObject.transform.localScale; 

    }

    //オブジェクトの拡大
    public void ScaleUp()
    {
        //選択状態でなければ処理しない
        if (Select.targetObject == null) { return; }

        //段階が最大でない場合、拡大
        if (scaleStep < maxStep)
        {
            //段階を1増やす
            scaleStep++;

            //デフォルトのサイズを基準にサイズ変更
            Select.targetObject.transform.localScale = defaultSize * (1 + scaleStep * changeScale);

        }

    }

    //オブジェクトの縮小
    public void ScaleDown()
    {
        //選択状態でなければ処理しない
        if (Select.targetObject == null) { return; }

        //段階が最小でない場合、縮小
        if (scaleStep > minStep)
        {
            //段階を1減らす
            scaleStep--;

            //デフォルトのサイズを基準にサイズ変更
            Select.targetObject.transform.localScale = defaultSize * (1 + scaleStep * changeScale);


        }

    }

    //新しいオブジェクトを設定する関数
    public void SetTarget(Transform target)
    {
        //選択オブジェクトを変更する
        Select.targetObject = target;

        //選択されていれば
        if (Select.targetObject != null)
        {
            //オブジェクトの元のサイズを保存
            defaultSize = Select.targetObject.localScale;
        }
    }


}