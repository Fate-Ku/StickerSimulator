using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class Description : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string descriptionText;   // ボタンごとの説明文

    public GameObject tooltipPanel;  // 説明文パネル
    public TMP_Text tooltipText;     // Text

    private float delayTime = 0.75f;   //表示するまでの秒数

    private float hoverTime = 0f;   // 乗っている時間
    private bool isHovering = false;
    private bool isShown = false;

    void Update()
    {
        if (!isHovering || isShown) return;

        // クリックされたらタイマーリセット
        if (Input.GetMouseButtonDown(0))
        {
            hoverTime = 0f;
            return;
        }

        // 経過時間を足す
        hoverTime += Time.deltaTime;

        // 一定時間たったら表示
        if (hoverTime >= delayTime)
        {
            tooltipPanel.SetActive(true);
            tooltipText.text = descriptionText;
            isShown = true;
        }
    }


    // マウスが乗ったら説明文を表示
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        hoverTime = 0f;
        isShown = false;
    }

    // マウスが離れたら説明文を非表示
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        hoverTime = 0f;
        isShown = false;
        tooltipPanel.SetActive(false);
    }
}
