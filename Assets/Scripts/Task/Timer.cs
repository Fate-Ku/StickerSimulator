using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer:MonoBehaviour
{
    //カウントダウンタイマー（分）
    public int CountDownMinutes = 1;

    //カウントダウンタイマー（秒）
    public float CountDownSeconds = 30.0f;

    //合計時間（秒）
    private float TotalTime;

    //文字を表示
    private TMP_Text TimeText;

    private void Start()
    {
        TimeText = GetComponent<TMP_Text>();

        //分と秒を合計秒に変換する
        TotalTime = CountDownMinutes * 60 + CountDownSeconds;

    }

    private void Update()
    {
        //カウントを減らす
        TotalTime -= Time.deltaTime;

        //マイナスにならないようにする
        if(TotalTime < 0) { TotalTime = 0; }

        //合計秒を分と秒に変換
        int minutes = (int)TotalTime / 60;
        int seconds = (int)TotalTime % 60;

        //分と秒を分けて表示（:00は2桁）（$"は変数を文字列に組み込む）
        TimeText.text = $"{minutes:00}:{seconds:00}";

        //カウントダウンタイマーがゼロになったときの処理
        if (CountDownSeconds <= 0)
        {
            //シーン遷移
            //SceneManager.LoadScene("報酬画面スクリプト名");

            //一度だけ実行
            enabled = false;
        }
    }
}


