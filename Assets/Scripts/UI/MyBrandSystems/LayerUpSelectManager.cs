using UnityEngine;

public class LayerUpSelectManager : MonoBehaviour
{
    public LayerUpTool currentTarget;

    public void BringCurrentToFront()
    {
        if (currentTarget != null)
        {
            currentTarget.LayerUp();
        }
    }
}
