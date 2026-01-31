using UnityEngine;

public class SelectStickerTypeTool : MonoBehaviour
{
    [SerializeField] private Select select;

    //形状シールのグループを入れる配列
    [Header(header: "ShapeSticker")]
    public GameObject[] shapeStickers;

    //動物シールのグループを入れる配列
    [Header("AnimalSticker")]
    public GameObject[] animalStickers;

    //花シールのグループを入れる配列
    [Header("FlowerSticker")]
    public GameObject[] flowerStickers;

    //天気シールのグループを入れる配列
    [Header("WeatherSticker")]
    public GameObject[] weatherStickers;

    //音楽シールのグループを入れる配列
    [Header("MusicSticker")]
    public GameObject[] musicStickers;

    //食べ物シールのグループを入れる配列
    [Header("FoodSticker")]
    public GameObject[] foodStickers;

    //シール編集エリア
    [SerializeField] private Collider2D stickerArea; 

    void Start()
    {
        //ゲーム開始時に形状シールを表示させる
        ShowGroup(shapeStickers); 

        //動物シールは非表示にする
        //(sticker→現在処理中のGameObject)(animalStickers→配列名)
        foreach (var sticker in animalStickers)
            if (sticker != null)
                sticker.SetActive(false);

        //花シールは非表示にする
        //(sticker→現在処理中のGameObject)(flowerStickers→配列名)
        foreach (var sticker in flowerStickers)
            if (sticker != null)
                sticker.SetActive(false);

        //天気シールは非表示にする
        foreach (var sticker in weatherStickers)
            if (sticker != null)
                sticker.SetActive(false);

        //音楽シールは非表示にする
        foreach (var sticker in musicStickers)
            if (sticker != null)
                sticker.SetActive(false);

        //食べ物シールは非表示にする
        foreach (var sticker in foodStickers)
            if (sticker != null)
                sticker.SetActive(false);
    }

    //形状シールボタンが押された
    public void OnShapeButton()
    {

        //選択状態解除
        select.targetObject = null;
        select.targetRenderer = null;

        //形状シールを表示し、別のシールを非表示にする
        ShowGroup(shapeStickers);
        HideGroup(animalStickers);
        HideGroup(flowerStickers);
        HideGroup(weatherStickers);
        HideGroup(musicStickers);
        HideGroup(foodStickers);
    }

    //動物ボタンが押された
    public void OnAnimalButton()
    {
        //選択状態解除
        select.targetObject = null;
        select.targetRenderer = null;

        //動物シールを表示し、別のシールを非表示にする
        ShowGroup(animalStickers);
        HideGroup(shapeStickers);
        HideGroup(flowerStickers);
        HideGroup(weatherStickers);
        HideGroup(musicStickers);
        HideGroup(foodStickers);
    }

    //花ボタンが押された
    public void OnFlowerButton()
    {
        //選択状態解除
        select.targetObject = null;
        select.targetRenderer = null;

        //動物シールを表示し、別のシールを非表示にする
        ShowGroup(flowerStickers);
        HideGroup(shapeStickers);
        HideGroup(animalStickers);
        HideGroup(weatherStickers);
        HideGroup(musicStickers);
        HideGroup(foodStickers);
    }

    //天気ボタンが押された
    public void OnWeatherButton()
    {
        //選択状態解除
        select.targetObject = null;
        select.targetRenderer = null;

        //動物シールを表示し、別のシールを非表示にする
        ShowGroup(weatherStickers);
        HideGroup(shapeStickers);
        HideGroup(animalStickers);
        HideGroup(flowerStickers);
        HideGroup(musicStickers);
        HideGroup(foodStickers);
    }

    //音楽ボタンが押された
    public void OnMusicButton()
    {
        //選択状態解除
        select.targetObject = null;
        select.targetRenderer = null;

        //動物シールを表示し、別のシールを非表示にする
        ShowGroup(musicStickers);
        HideGroup(shapeStickers);
        HideGroup(animalStickers);
        HideGroup(flowerStickers);
        HideGroup(weatherStickers);
        HideGroup(foodStickers);
    }

    //食べ物ボタンが押された
    public void OnFoodButton()
    {
        //選択状態解除
        select.targetObject = null;
        select.targetRenderer = null;

        //動物シールを表示し、別のシールを非表示にする
        ShowGroup(foodStickers);
        HideGroup(shapeStickers);
        HideGroup(animalStickers);
        HideGroup(flowerStickers);
        HideGroup(weatherStickers);
        HideGroup(musicStickers);
    }

    //特定グループだけを表示・非表示切り替え
    private void ShowGroup(GameObject[] group)
    {
        foreach (var sticker in group)
        {
            if (sticker != null)
                sticker.SetActive(true);
        }
    }

    //グループを非表示（シール編集エリア外のシールのみ）
    private void HideGroup(GameObject[] group)
    {
        foreach (var sticker in group)
        {
            if (sticker == null) continue;

            //選択状態なら解除する・色を戻す
            if (select.targetObject == sticker.transform)
            {
                select.targetObject = null;
                select.targetRenderer = null;
              
            }

            //シール編集エリア内にオブジェクトがあるか？
            bool inside = stickerArea.OverlapPoint(sticker.transform.position);

            //シール編集エリア内なら表示したままにする
            if (inside)
            {
                sticker.SetActive(true);
            }
            //シール編集エリア外なら非表示にする
            else
            {
                sticker.SetActive(false);
            }
        }
    }
}

