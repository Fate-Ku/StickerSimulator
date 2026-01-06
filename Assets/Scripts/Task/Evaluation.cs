using TMPro;
using UnityEngine;

public class Evaluation:MonoBehaviour
{
    [SerializeField] private TMP_Text evaluationText;

    //‰‚ß‚ÍD•]‰¿‚É‚µ‚Ä‚¨‚­
    private void Start()
    {
        evaluationText.text = "D";
    }

    //•]‰¿‚ÌŒvZ‚ğs‚¤
    public void EvaluationCalculation()
    {
 
    }

    //•]‰¿‚É‘Î‚·‚é•\¦‚ğs‚¤
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
            case 5:
                return "D";
        }
        return "";
    }
}
