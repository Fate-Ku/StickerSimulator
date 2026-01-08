using UnityEngine;

[CreateAssetMenu(fileName = "StickerData", menuName = "Sticker System/Sticker Data")]
public class StickerData : ScriptableObject
{
    public string stickerName; 
    public Sprite unlockedImage; 
    public Sprite lockedImage;   // gray picture
    public string category;      // ex: "Animal", "Item", "Special"
    [TextArea] public string getMethod;
    public bool isUnlocked = false;

    //シールのデータを格納（塚田）

    //形
    public enum SealShape
    {
        Circle,
        Triangle,
        Star
    }

    //色

    public enum SealColor
    {
        Red,
        Blue,
        Yellow
    }


}
