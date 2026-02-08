using UnityEngine;

public class ColorChangePanel : MonoBehaviour
{
    public GameObject ColorPanel;
    [SerializeField] private Select select;

    //非表示か？
    bool Visible;

    //初めは非表示にしておく
    private void Start()
    {
        ColorPanel.SetActive(false);
        Visible = false;
    }

    //ボタンが押された
    public void OnButtonDown(int ButtonType)
    {
        //選択状態でなければ処理しない
        if (select.targetObject == null) { return; }

        //選択中のオブジェクトのRendererを取得
        Renderer renderer = select.targetObject.GetComponent<Renderer>();


        //それぞれのボタンの色に変更する
        switch (ButtonType)
        {
            case 0:
                //オブジェクトの色をパステルレッドに変更
                renderer.material.color = new Color32(255, 126, 126, 255);
                break;

            case 1:
                //オブジェクトの色をパステル水色に変更
                renderer.material.color = new Color32(180, 250, 255, 255);
                break;

            case 2:
                //オブジェクトの色をパステルイエローに変更
                renderer.material.color = new Color32(255, 255, 180, 255);
                break;

            case 3:
                //オブジェクトの色を黄緑に変更
                renderer.material.color = new Color32(190, 255, 200, 255);
                break;

            case 4:
                //オブジェクトの色をパステルピンクに変更
                renderer.material.color = new Color32(255, 157, 235, 255);
                break;

            case 5:
                //オブジェクトの色をパステルパープルに変更
                renderer.material.color = new Color32(206, 150, 255, 255);
                break;

            case 6:
                //オブジェクトの色をパステルブルーに変更
                renderer.material.color = new Color32(172, 184, 255, 255);
                break;

            case 7:
                //オブジェクトの色をパステルオレンジに変更
                renderer.material.color = new Color32(255, 191, 153, 255);
                break;

            case 8:
                //オブジェクトの色をデフォルトに変更
                DefaultColorChange info = select.targetObject.GetComponent<DefaultColorChange>();
                if (info == null) return;

                if (info.type == StickerType.Shape)
                {
                    renderer.material.color = new Color32(254, 144, 231, 255); // ピンク
                }
                else if (info.type == StickerType.Animal)
                {
                    renderer.material.color = Color.white;
                }
                break;

            case 9:
                //オブジェクトの色を赤に変更
                renderer.material.color = new Color32(255, 73, 70, 255);
                break;

            case 10:
                //オブジェクトの色をオレンジに変更
                renderer.material.color = new Color32(255, 160, 37, 255);
                break;

            case 11:
                //オブジェクトの色を黄色に変更
                renderer.material.color = new Color32(255, 250, 36, 255);
                break;

            case 12:
                //オブジェクトの色を黄緑に変更
                renderer.material.color = new Color32(163, 255, 0, 255);
                break;

            case 13:
                //オブジェクトの色を青に変更
                renderer.material.color = new Color32(60, 96, 255, 255);
                break;

            case 14:
                //オブジェクトの色を紫に変更
                renderer.material.color = new Color32(194, 60, 255, 255);
                break;

            case 15:
                //オブジェクトの色をマゼンタに変更
                renderer.material.color = new Color32(255, 60, 238, 255);
                break;

            case 16:
                //オブジェクトの色をパステルグレーに変更
                renderer.material.color = new Color32(186, 186, 186, 255);
                break;

            case 17:
                //オブジェクトの色を白に変更
                renderer.material.color = new Color32(255, 255, 255, 255);
                break;

            case 18:
                //オブジェクトの色を水色に変更
                renderer.material.color = new Color32(0, 240, 255, 255);
                break;

            case 19:
                //オブジェクトの色を黒に変更
                renderer.material.color = new Color32(42, 42, 42, 255);
                break;

            case 20:
                //オブジェクトの色をパステルブラウンに変更
                renderer.material.color = new Color32(162, 115, 97, 255);
                break;

            case 21:
                //オブジェクトの色を橙色に変更
                renderer.material.color = new Color32(255, 220, 184, 255);
                break;

            case 22:
                //オブジェクトの色を紫色に変更
                renderer.material.color = new Color32(103, 45, 184, 255);
                break;

            case 23:
                //オブジェクトの色をブラウンに変更
                renderer.material.color = new Color32(103, 45, 0, 255);
                break;


            case 24:
                //オブジェクトの色を深緑に変更
                renderer.material.color = new Color32(42, 111, 0, 255);
                break;

            case 25:
                //オブジェクトの色を深緑に変更
                renderer.material.color = new Color32(103, 45, 184, 255);
                break;

            case 26:
                //オブジェクトの色を深青に変更
                renderer.material.color = new Color32(0, 30, 156, 255);
                break;

            case 27:
                //オブジェクトの色を深赤に変更
                renderer.material.color = new Color32(112, 4, 0, 255);
                break;


            case 28:
                //オブジェクトの色を深オレンジに変更
                renderer.material.color = new Color32(184, 99, 0, 255);
                break;

            case 29:
                //オブジェクトの色を深黄色に変更
                renderer.material.color = new Color32(184, 177, 0, 255);
                break;

            case 30:
                //オブジェクトの色を深水色に変更
                renderer.material.color = new Color32(0, 111, 111, 255);
                break;
        }
    }

    //色変更ツールボタンが押されたら表示する
    public void PressedChangeColorButton()
    {
        //非表示なら表示する
        if (Visible == false)
        {
            ColorPanel.SetActive(true);
            Visible = true;
        }
        //表示されてたら非表示にする
        else
        {
            ColorPanel.SetActive(false);
            Visible = false;
        }


    }
}