using UnityEngine;

public class LayerDownTool : MonoBehaviour
{
        public void LayerDown()
        {
            //このスクリプトがアタッチされたGameOjectを最背面に移動
            GetComponent<RectTransform>().SetAsFirstSibling();

            //transform.SetAsFirstSibling();
        }
    
}
