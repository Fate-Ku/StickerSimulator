using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    public void TaskBTN()
    {
        //GameManager.instance.ChangeScene("Task");
    }

    public void MyBrandBTN()
    {
        GameManager.instance.ChangeScene("MyBrand");

    }

    public void StickerBookBTN()
    {
        GameManager.instance.ChangeScene("StickerBook");

    }

    public void GaChaBTN()
    {
        //GameManager.instance.ChangeScene("GaCha");

    }


    public void QuitGameBTN()
    {
        Application.Quit();
    }

}
