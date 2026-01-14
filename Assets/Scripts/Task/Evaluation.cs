using TMPro;
using UnityEngine;

public class Evaluation:MonoBehaviour
{
    [SerializeField] private TMP_Text evaluationText;

    //文字サイズ
    [SerializeField] private int fontSize; 

    //初めはD評価にしておく
    private void Start() { evaluationText.text = "D"; }


    //評価に対する表示を行う
    private string EvaluationText(int evaluation)
    {
        switch (evaluation)
        {
            case 1:
                return "S";
            case 2:
                return "A";
            case 3:
                return "B";
            case 4:
                return "C";
        }
        return "";
    }
}
