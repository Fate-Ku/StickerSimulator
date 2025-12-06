using UnityEngine;
using UnityEngine.UI;

public class DeleteTool : MonoBehaviour
{
    //削除したいオブジェクトをInspectorから指定する
    public GameObject objectToDestroy;

    public void DestroyObject()
    {
        // 何も選択していなければ終了
        if (Select.targetObject == null){ return; }

        // 選択されているオブジェクトを削除
        Destroy(Select.targetObject.gameObject);

        // 選択解除
        Select.targetObject = null;
    }
}
