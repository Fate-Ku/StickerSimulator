using UnityEngine;

public class ChangeColorTool:MonoBehaviour
{
    private Renderer color;

    //元の色を保存する変数
    private Color defaultColor;

    //現在の色の段階
    private int colorStep = 0;

    //Updateが呼ばれる前に呼ばれる
    private void Start()
    {
        color = GetComponent<Renderer>();
        //元の色を保存
        defaultColor = color.material.color;
        //色の段階を１にする
        colorStep = 1;

    }

    //色を変更する
    public void ChangeColor()
    {

        switch (colorStep)
        {

            case 0:

                //オブジェクトを元の色にする
                color.material.color = defaultColor;
                //色の段階を1にする
                colorStep = 1;
                break;
            case 1:
                //オブジェクトの色を赤に変更
                GetComponent<Renderer>().material.color = Color.red;
                //色の段階を2にする
                colorStep = 2;
                break;

            case 2:
                //オブジェクトの色を赤に変更
                GetComponent<Renderer>().material.color = Color.blue;
                //色の段階を3にする
                colorStep = 3;
                break;
            case 3:
                //オブジェクトの色を赤に変更
                GetComponent<Renderer>().material.color = Color.green;
                colorStep = 4;
                break;
            case 4:
                //オブジェクトの色を赤に変更
                GetComponent<Renderer>().material.color = Color.yellow;
                //色の段階を0にする
                colorStep = 0;
                break;
        }

    }
}
