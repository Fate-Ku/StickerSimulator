using UnityEngine;

public class ChangeColorTool : MonoBehaviour
{
    [SerializeField] private Select select;
    [SerializeField] private ColorChangePanel[] ColorPanels;

    public void OnButtonDown()
    {

            //ボタンが押されたらカラーパレットを表示する
            foreach (var panel in ColorPanels)
        {
            panel.PressedChangeColorButton();
        }

    }

}
