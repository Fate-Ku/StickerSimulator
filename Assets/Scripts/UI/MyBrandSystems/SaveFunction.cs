using UnityEngine;
using System.Collections;
using System.IO; //ファイルに書き込むために必要
using System;    //Convertを使うために必要

public class SaveFunction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){  }

    // Update is called once per frame
    void Update()
    {
        //サイズ取得
        Vector2 size = gameObject.GetComponent<RectTransform>().sizeDelta;
        float width = size.x;  //幅
        float height = size.y; //高さ

        //ファイルパスを定義
		string filePath = Application.dataPath + @"\Scripts\File\ObjectPos.txt";

		//ファイルの末尾に値を追加（Convertでfloat型の座標値をString型に変換している）
		File.AppendAllText(filePath, Convert.ToString(x)+","+ Convert.ToString(y)+","+ Convert.ToString(z) + "\n");
    }
}
