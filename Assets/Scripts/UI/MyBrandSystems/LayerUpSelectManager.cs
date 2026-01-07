using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LayerUpSelectManager : MonoBehaviour
{
    public static LayerUpSelectManager Instance;
    public LayerUpTool currentTarget;
    
    //SelectToolの変数宣言
    [SerializeField] private Select select;

    void Awake()
    {
        Instance = this;
    }

    public void BringCurrentToFront()
    {
        Debug.Log("BringCurrentToFront called");

        if (select.targetobject == null) return;

        if (select.targetobject != null)select.targetobject.LayerUp();

        else
        {
            Debug.Log("currentTarget is null");
        }
    }
}
