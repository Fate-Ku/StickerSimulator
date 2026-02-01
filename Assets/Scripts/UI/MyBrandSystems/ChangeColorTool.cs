using System.Linq;
using UnityEngine;

public class ChangeColorTool : MonoBehaviour
{
    [SerializeField] private Select select;
    [SerializeField] private ColorChangePanel[] ColorPanels;


    public void OnButtonDown()
    {
        //ボタンが押されたらカラーパレットを表示する
        for (int i = 0; i < ColorPanels.Length; i++)
        {
            ColorPanels[i].PressedChangeColorButton();
        }

    }

}
