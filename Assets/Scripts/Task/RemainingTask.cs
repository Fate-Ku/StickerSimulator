using TMPro;
using UnityEngine;

public class RemainingTask : MonoBehaviour
{
    //文字を表示
    [SerializeField] private TMP_Text RemainingTaskText;

    //残りの依頼の数
    int remainingTaskCount = 3;

    private void Start()
    {
        //残りの依頼の数を表示（$は変数を文字列に組み込む）
        RemainingTaskText.text = $"{remainingTaskCount}/3";
    }

    //発送ボタンが押されたら残りの依頼の数を減らす
    public void RemainingTaskCount()
    {
        //残りの依頼の数を減らす
        remainingTaskCount--;

        //残りの依頼の数を表示（$は変数を文字列に組み込む）
        RemainingTaskText.text = $"{remainingTaskCount}/3";

    }
}
