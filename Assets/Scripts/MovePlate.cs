using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MovePlate : MonoBehaviour
{
    //マップ生成スクリプトをアタッチしたGameObject
    //public GameObject mapGenerator;
    // コントローラーへの参照が必要ないメソッドもあるため、コントローラーへの参照を保持
    public GameObject controller;

    // この移動プレートを生成するためにタップされたチェスピース
    GameObject reference = null;

    // ボード上の位置
    int matrixX;
    int matrixY;

    // false: 移動, true: 攻撃
    public bool attack = false;

    public void Start()
    {

        if (attack)
        {
            // 赤に設定
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        //string clickedObjectName = gameObject.name; // クリックしたオブジェクトの名前を取得
        controller = GameObject.FindGameObjectWithTag("GameController");  // ゲームコントローラーの参照を取得

        // 敵のチェスピースを破棄
        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);  // 座標の駒を取得

            if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");  // 敵のキングが破壊された場合、勝者を設定
            if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");  // 敵のキングが破壊された場合、勝者を設定

            Destroy(cp);  // 駒を破壊
        }

        // チェスピースの元の位置を空に設定
        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().GetXBoard(),
            reference.GetComponent<Chessman>().GetYBoard());

        // チェスピースをこの位置に移動
        reference.GetComponent<Chessman>().SetXBoard(matrixX);
        reference.GetComponent<Chessman>().SetYBoard(matrixY);
        reference.GetComponent<Chessman>().SetCoords();

        // マトリックスを更新
        controller.GetComponent<Game>().SetPosition(reference);

        // 現在のプレイヤーを切り替え
        controller.GetComponent<Game>().NextTurn();

        // 移動プレートを含む移動プレートを破棄（自身も含む）
        reference.GetComponent<Chessman>().DestroyMovePlates();

        // 駒の座標を取得
        //int x = reference.GetComponent<Chessman>().GetXBoard();
        //int y = reference.GetComponent<Chessman>().GetYBoard();

        // デバッグログで情報を確認
        //Debug.Log("オブジェクト用法:：" + clickedObjectName);
        //Debug.Log("座標：（" + x +"," + y + ")");
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
