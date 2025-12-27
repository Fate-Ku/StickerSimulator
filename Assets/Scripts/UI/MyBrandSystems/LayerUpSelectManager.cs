using UnityEngine;

public class LayerUpSelectManager : MonoBehaviour
{
    public static LayerUpSelectManager Instance;
    public LayerUpTool currentTarget;

    void Awake()
    {
        Instance = this;
    }

    public void BringCurrentToFront()
    {
        Debug.Log("BringCurrentToFront called");

        if (currentTarget != null)
        {
            currentTarget.LayerUp();
        }

        else
        {
            Debug.Log("currentTarget is null");
        }
    }
}
