using UnityEngine;
using UnityEngine.UIElements;

public class RotateTool : MonoBehaviour
{

    //private bool canRotate = false;

    public void Start()
    {
        //新しいオブジェクトを設定
        SetTarget(transform);

    }

    //新しいオブジェクトを設定する関数
    public void SetTarget(Transform target)
    {
        //選択オブジェクトを変更する
        Select.targetObject = target;

    }

    //左に15度回転
    public void RotateLeft()
    {
        Select.targetObject.transform.Rotate(0, 0, 15.0f, Space.World);
    }

    //右に15度回転
    public void RotateRight()
    {
        Select.targetObject.transform.Rotate(0, 0, -15.0f, Space.World);
    }


}
