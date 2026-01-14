using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public void TaskBTN()
    {
        GameManager.instance.ChangeScene("Task");
    }

    public void MyBrandBTN()
    {
        GameManager.instance.ChangeScene("MyBrand");

    }

    public void StickerBookMenuBTN()
    {
        GameManager.instance.ChangeScene("StickerBookMenu");

    }

    public void StickerBookBTN()
    {
        GameManager.instance.ChangeScene("StickerBook");

    }

    public void ZukanBTN()
    {
        GameManager.instance.ChangeScene("Zukan");
    }

    public void GaChaBTN()
    {
        //GameManager.instance.ChangeScene("GaCha");

    }

    public void MainMenuBTN()
    {
        GameManager.instance.ChangeScene("MainMenu");

    }


    public void QuitGameBTN()
    {
        Application.Quit();
    }
}
