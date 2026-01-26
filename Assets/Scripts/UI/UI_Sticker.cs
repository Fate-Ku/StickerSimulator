using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Sticker : MonoBehaviour
{
    public Image stickerImage;        
    public TMP_Text stickerName;      
    public TMP_Text getStickerMethod;

    private StickerData data;

    public void Setup(StickerData sticker)
    {
        data = sticker;
        if (data == null) return;

        // stickerImage：show lockedImage
        stickerImage.sprite = data.isUnlocked ? data.unlockedImage : data.lockedImage;

        // stickerName：unlocked then show ????
        stickerName.text = data.isUnlocked ? data.stickerName : "???";

        // getStickerMethod：show always
        //getStickerMethod.text = data.getMethod +"で入手できる";
        getStickerMethod.text = data.getMethod +"";

    }

    public string GetCategory()
    {
        return data != null ? data.category : "";
    }

    public bool IsUnlocked()
    {
        return data != null && data.isUnlocked;
    }


}
