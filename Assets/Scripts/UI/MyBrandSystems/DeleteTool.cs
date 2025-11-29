using UnityEngine;
using UnityEngine.UI;

public class DeleteTool : MonoBehaviour
{
    //削除したいオブジェクトをInspectorから指定する
    public GameObject objectToDestroy;

    public void DestroyObject()
    {
        // 指定したオブジェクトを削除する
        Destroy(objectToDestroy);
    }
}
