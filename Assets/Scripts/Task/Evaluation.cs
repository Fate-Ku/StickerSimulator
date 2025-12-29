using TMPro;
using UnityEngine;

public class Evaluation:MonoBehaviour
{

    

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
        }
        return "";
    }
}
