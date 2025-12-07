using UnityEngine;

public class SelectStickerTypeTool : MonoBehaviour
{
    //形状シールのグループを入れる配列
    [Header(header: "ShapeSticker")]
    public GameObject[] shapeStickers;

    //動物シールのグループを入れる配列
    [Header("AnimalSticker")]
    public GameObject[] animalStickers;

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
    }

    //形状シールボタンが押された
    public void OnShapeButton()
    {
        //選択していたシールの選択状態（色）を基に戻す
        if (Select.targetRenderer != null)
            Select.targetRenderer.color = Select.defaultColor;

        //選択状態解除
        Select.targetObject = null;
        Select.targetRenderer = null;

        //形状シールを表示し、動物シールを非表示にする
        ShowGroup(shapeStickers);
        HideGroup(animalStickers);
    }

    // 動物ボタンが押された
    public void OnAnimalButton()
    {
        //選択していたシールの選択状態（色）を基に戻す
        if (Select.targetRenderer != null)
            Select.targetRenderer.color = Select.defaultColor;

        //選択状態解除
        Select.targetObject = null;
        Select.targetRenderer = null;

        //動物シールを表示し、形状シールを非表示にする
        ShowGroup(animalStickers);
        HideGroup(shapeStickers);
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
            if (Select.targetObject == sticker.transform)
            {
                Select.targetObject = null;
                Select.targetRenderer = null;
                Select.targetRenderer.color = Select.defaultColor;
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

