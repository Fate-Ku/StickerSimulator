using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class RequestTask : MonoBehaviour
{
    //文字を表示
    private TMP_Text TaskText;

    //依頼の数
    int TaskCount = 3;

    //依頼をリストに入れる
    List<int> taskList = new List<int>() { 1, 2, 3, 4, 5, 6 };

    //文字サイズ指定
    [SerializeField] private int FontSize;

    //依頼内容を表示させる
    private void Start()
    {
        TaskText = GetComponent<TMP_Text>();

        //文字サイズ
        TaskText.fontSize = FontSize;


        
        //依頼の数（3つ）だけ値を取得
        for(int i = 0; i < TaskCount; i++)
        {
            // 残っている中からランダムで選ぶ
            int index = Random.Range(0, taskList.Count);
            int randomTask = taskList[index];

            SetTask(randomTask);

            //重複しないように使ったタスクを削除する
            taskList.RemoveAt(index);
        }
        
    }

    //選ばれた値に応じた依頼を表示させる関数
    private void SetTask(int randomTask)
    {
        switch (randomTask)
        {
            case 1:
                TaskText.text += "・まるの\n　シールをつかって！\n\n";
                break;

            case 2:
                TaskText.text += "・さんかくの\n　シールをつかって！\n\n";
                break;

            case 3:
                TaskText.text += "・くまのシールを\n　つかって！\n\n";
                break;
            case 4:
                TaskText.text += "・ネコのシールを\n　つかって！\n\n";

                break;
            case 5:
                TaskText.text += "・2しゅるいのシールを\n　つかって！\n\n";
                break;
            case 6:
                TaskText.text += "・3しゅるいのシールを\n　つかって！\n\n";
                break;

        }
    }
}
