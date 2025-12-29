using UnityEngine;

public class ColorChangePanel:MonoBehaviour
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
                //オブジェクトの色を赤に変更
                renderer.material.color = new Color32(255, 190, 190, 255);
                break;

            case 1:
                //オブジェクトの色を水色に変更
                renderer.material.color = new Color32(180, 250, 255, 255);
                break;

            case 2:
                //オブジェクトの色を黄色に変更
                renderer.material.color = new Color32(255, 255, 180, 255);
                break;

            case 3:
                //オブジェクトの色を黄緑に変更
                renderer.material.color = new Color32(190, 255, 200, 255);
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
