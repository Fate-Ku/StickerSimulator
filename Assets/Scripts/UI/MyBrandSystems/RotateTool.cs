using UnityEngine;
using UnityEngine.UIElements;

public class RotateTool : MonoBehaviour
{
    [SerializeField] private Select select;

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
