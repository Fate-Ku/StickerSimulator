using UnityEngine;
using UnityEngine.UIElements;

public class RotateTool : MonoBehaviour
{
    [SerializeField] private Select select;

    //最後に選択していたオブジェクト
    private Transform lastTarget;

    //private bool canRotate = false;

    public void Update()
    {
        var target = select.targetObject;

        //選択しているオブジェクトが同じなら変更しない
        if (target == lastTarget) return;

        lastTarget = target;
        SetTarget(target);
    }

    //新しいオブジェクトを設定する関数
    public void SetTarget(Transform target)
    {
        //選択オブジェクトを変更する
        select.targetObject = target;

    }

    //左に15度回転
    public void RotateLeft()
    {
        select.targetObject.transform.Rotate(0, 0, 15.0f, Space.World);
    }

    //右に15度回転
    public void RotateRight()
    {
        select.targetObject.transform.Rotate(0, 0, -15.0f, Space.World);
    }


}
