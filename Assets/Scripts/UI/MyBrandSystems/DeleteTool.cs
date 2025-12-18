using UnityEngine;
using UnityEngine.UI;

public class DeleteTool : MonoBehaviour
{
    [SerializeField] private Select select;

    //削除したいオブジェクトをInspectorから指定する
    public GameObject objectToDestroy;

    public void DestroyObject()
    {
        // 何も選択していなければ終了
        if (select.targetObject == null){ return; }

        // 選択されているオブジェクトを削除
        Destroy(select.targetObject.gameObject);

        // 選択解除
        select.targetObject = null;
    }
}
