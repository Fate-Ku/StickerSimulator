using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer:MonoBehaviour
{
    //カウントダウンタイマー（分）
    public int CountDownMinutes = 1;

    //カウントダウンタイマー（秒）
    public float CountDownSeconds = 30;

    private Text TimeText;

    private void Start()
    {
        TimeText = GetComponent<Text>();
    }

    private void Update()
    {
        //カウントダウンを減らす
        CountDownSeconds -= Time.deltaTime;

        var span = new TimeSpan(0, 0, (int)CountDownSeconds);

        TimeText.text = span.ToString(@"mm\:ss");

        //カウントダウンタイマーがゼロになったときの処理
        if(CountDownSeconds <= 0)
        {
            //シーン遷移
        }
    }
}


