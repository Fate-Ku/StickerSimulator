using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System;

public class RequestTask : MonoBehaviour
{
    [SerializeField] private TMP_Text taskText;

    // 実行する依頼の回数
    [NonSerialized] public int taskCount = 3;

    // 文字サイズ
    [SerializeField] private int fontSize;

    // タスク候補（重複なし）
    private List<int> taskList = new List<int>() { 1, 2, 3, 4, 5, 6 };

    // 今表示している依頼
    private int currentTask = -1;

    private void Start()
    {
        //文字のサイズを設定
        taskText.fontSize = fontSize;
        taskText.text = "";

        //初めの依頼を表示
        ShowTask();
    }

    ///タスクをランダムで表示
    private void ShowTask()
    {

        //リストからランダムに選択
        int index = UnityEngine.Random.Range(0, taskList.Count);
        currentTask = taskList[index];

        //重複しないようにリストから削除する
        taskList.RemoveAt(index);

        taskText.text = GetTaskText(currentTask);
    }

    /// タスク内容を文字列で返す
    private string GetTaskText(int task)
    {
        switch (task)
        {
            case 1:
                return "・まるの\n　シールをつかって！";
            case 2:
                return "・さんかくの\n　シールをつかって！";
            case 3:
                return "・くまのシールを\n　つかって！";
            case 4:
                return "・ネコのシールを\n　つかって！";
            case 5:
                return "・2しゅるいのシールを\n　つかって！";
            case 6:
                return "・3しゅるいのシールを\n　つかって！";
        }
        return "";
    }

    ///発送ボタンが押されたら呼ばれる
    public void CompleteTask()
    {
        // 依頼の数を減らす
        taskCount--;

        // 指定回数終わったら報酬画面へシーン遷移
        if (taskCount <= 0)
        {
            RewardScene();

            return;
        }

        // 表示を消す
        taskText.text = "";

        // 次のタスクへ
        ShowTask();
    }

    //報酬画面へシーン遷移(現在はタイトル画面に戻るようになっている)
    private void RewardScene()
    {
        GameManager.instance.ChangeScene("MainMenu");
    }
}