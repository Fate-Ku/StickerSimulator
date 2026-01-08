using UnityEngine;
using UnityEngine.UI;

public class StarGauge:MonoBehaviour
{
    [SerializeField] private int maxTaskCount;   // このステージのタスク総数

    [SerializeField] private Slider gaugeSlider;
    private int successTaskCount = 0;

    // タスク成功時に呼ぶ
    public void OnTaskSuccess()
    {
        successTaskCount++;

        float progress = (float)successTaskCount / maxTaskCount;
        progress = Mathf.Clamp01(progress);

        UpdateGauge(progress);
    }

    void UpdateGauge(float progress)
    {
        gaugeSlider.value = progress;
    }

}
