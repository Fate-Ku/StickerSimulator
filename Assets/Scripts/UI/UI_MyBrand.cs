using UnityEngine;

public class UI_MyBrand : MonoBehaviour
{
    public void BackToMainMenuBTN()
    {
        GameManager.instance.ChangeScene("MainMenu");

    }
    public void StickerBookBTN()
    {
        GameManager.instance.ChangeScene("StickerBook");

    }

    public void RequestList()
    {

    }


}
