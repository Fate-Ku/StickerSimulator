using UnityEngine;
using UnityEngine.UIElements;

public class RotateTool : MonoBehaviour
{

    private bool canRotate = false;

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

    public void RotatePicture()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Select.targetObject.transform.Rotate(0, 0, -45f); // click → turn left 45°
        }
    }


}
