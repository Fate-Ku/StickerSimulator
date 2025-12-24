using UnityEngine;

public class ShippingButton:MonoBehaviour
{

    //依頼の数を取得
    [SerializeField] private RequestTask requestTask;


    //発送ボタンが押された
    public void OnShippingButton()
    {
        //シール編集エリアにあるシール情報を保存する

        //シール編集エリアにあるシールを全て削除する

        //依頼の内容を変更する
        requestTask.CompleteTask();

    }
}
