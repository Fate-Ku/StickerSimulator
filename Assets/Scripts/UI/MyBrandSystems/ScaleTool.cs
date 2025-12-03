using UnityEngine;


public class ScaleTool : MonoBehaviour
{
    //サイズを変更するオブジェクト
    public Transform targetObject;

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

        //オブジェクトの元のサイズを保存
        defaultSize = targetObject.transform.localScale; 

    }

    //オブジェクトの拡大
    public void ScaleUp()
    {
        if(targetObject == null) { return; }

        //段階が最大でない場合
        if (scaleStep < maxStep)
        {
            scaleStep++;
            targetObject.transform.localScale = defaultSize * (1 + scaleStep * changeScale);

        }

    }

    //オブジェクトの縮小
    public void ScaleDown()
    {
        if (targetObject == null) { return; }

        //段階が最小でない場合
        if (scaleStep > minStep)
        {
            scaleStep--;
            targetObject.transform.localScale = defaultSize * (1 + scaleStep * changeScale);


        }

    }

    //targetObject を Set できる関数
    public void SetTarget(Transform target)
    {
        targetObject = target;

        if (targetObject != null)
        {
            defaultSize = targetObject.localScale;
        }
    }


}