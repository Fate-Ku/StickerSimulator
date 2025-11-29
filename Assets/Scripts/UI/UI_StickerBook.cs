using UnityEngine;

public class UI_StickerBook : MonoBehaviour
{

    public void BackToMainMenuBTN()
    {
        GameManager.instance.ChangeScene("MainMenu");

    }

    public void MyBrandBTN()
    {
        GameManager.instance.ChangeScene("MyBrand");

    }

}
