using UnityEngine;
using UnityEngine.UI;

public class LayerUpTool : MonoBehaviour
{
    // 最前面に移動させたいGameObjectをインスペクターから設定するための変数
    public GameObject objectToBringToFront;

    // このメソッドをボタンのOnClickイベントに紐づける
    public void MoveObjectToFront()
    {
        if (objectToBringToFront != null)
        {
            // 対象のオブジェクトを、親要素の最後の子要素（最前面）に設定する
            objectToBringToFront.transform.SetAsLastSibling();
            //Debug.Log(objectToBringToFront.name + "を最前面に移動しました。");
        }
    }
}
