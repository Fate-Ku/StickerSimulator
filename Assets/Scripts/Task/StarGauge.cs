using UnityEngine;
using UnityEngine.UI;

public class StarGauge : MonoBehaviour
{
    [SerializeField] private int maxTaskCount;   // このステージのタスク総数

    [SerializeField] private Slider gaugeSlider;

    [SerializeField] private StarAnimator starAnimator; //  2026.1.23 added by Fate

    private int successTaskCount = 0;

    // タスク成功時に呼ぶ
    public void OnTaskSuccess()
    {
        successTaskCount++;

        float progress = (float)successTaskCount / maxTaskCount;
        progress = Mathf.Clamp01(progress);

        UpdateGauge(progress);

        CalculateStarCount(successTaskCount); //  2026.1.23 added by Fate
    }

    void UpdateGauge(float progress)
    {
        gaugeSlider.value = progress;
    }

    //  2026.1.23 added by Fate
    public void ShowResultStars(int starCount)
    {
        starAnimator.SetStarCount(starCount);
    }

    // count star  
    int CalculateStarCount(int taskCount)
    {
        int starCount = 0;

        // judge star ( max : 3 star)
        if (taskCount >= maxTaskCount)          // all task completed
            starCount = 3;
        else if (taskCount >= maxTaskCount * 2 / 3)
            starCount = 2;
        else if (taskCount >= maxTaskCount / 3)
            starCount = 1;
        else
            starCount = 0;

  
        return starCount;
    }
    //  2026.1.23 added by Fate

}