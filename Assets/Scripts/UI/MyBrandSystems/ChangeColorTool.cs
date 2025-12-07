using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeColorTool : MonoBehaviour
{
    //元の色を保存する変数
    private Color defaultColor;

    //現在の色の段階
    private int colorStep = 0;

    //シールの種類が切り替わったか確認用
    private Renderer prevRenderer = null;

    //色を変更する
    public void ChangeColor()
    {

        //選択状態でなければ処理しない
        if (Select.targetObject == null) { return; }

        //選択中のオブジェクトのRendererを取得
        Renderer renderer = Select.targetObject.GetComponent<Renderer>();

        //選択しているオブジェクトが切り替わったら
        if (renderer != prevRenderer)
        {
            //元の色を保存
            defaultColor = renderer.material.color;

            //選択中のオブジェクトに設定
            prevRenderer = renderer;

            colorStep = 0;
        }

        //現在の色の段階によって色を変える
        switch (colorStep)
        {
            case 0:

                //オブジェクトを元の色にする
                renderer.material.color = defaultColor;
                //色の段階を1にする
                colorStep = 1;
                break;
            case 1:
                //オブジェクトの色を黄色に変更（乗算で色計算しているので黄色になりません）
                renderer.material.color = new Color32(255, 255, 180, 255);
                //色の段階を2にする
                colorStep = 2;
                break;

            case 2:
                //オブジェクトの色を水色に変更
                renderer.material.color = new Color32(180, 250, 255, 255);
                //色の段階を3にする
                colorStep = 3;
                break;
            case 3:
                //オブジェクトの色を黄緑に変更
                renderer.material.color = new Color32(190, 255, 200, 255);
                colorStep = 4;
                break;
            case 4:
                //オブジェクトの色を赤に変更
                renderer.material.color = new Color32(255, 190, 190, 255);
                //色の段階を0にする
                colorStep = 0;
                break;
        }

    }
}
