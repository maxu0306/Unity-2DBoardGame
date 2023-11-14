using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game2 : MonoBehaviour
{
    // マップ生成スクリプトをアタッチしたGameObject
    public GameObject chesspiece;
    public GameObject grassPrefab;
    public GameObject waterPrefab;
    public GameObject woodsPrefab;
    public GameObject buffPrefab;
    public GameObject holePrefab;
    public GameObject warp0APrefab;
    public GameObject warp0BPrefab;

    // 各 GameObject の位置を保持するための行列
    private GameObject[,] positions = new GameObject[15, 15];

    // 各プレイヤーの駒を管理するための別々の配列
    // "positions" と "playerBlack" / "playerWhite" に同じオブジェクトが含まれることに注意
    public GameObject[] playerBlack = new GameObject[16];
    public GameObject[] playerWhite = new GameObject[16];

    // 現在のターン
    public string currentPlayer = "white";

    // ゲーム終了の時は、tureに設定
    // ゲーム開始時点では、終了していないため、falseに設定
    private bool gameOver = false;

    // Unity はゲームが開始されるとすぐにこれを呼び出します
    public void Start()
    {

        int[,] mapConfig = new int[,]
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,2,0,2,0,2,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,1,3,1,0,0,0,0,0,0},
            {0,0,0,3,0,1,1,0,1,1,0,0,0,0,0},
            {0,0,3,0,1,1,1,0,1,1,1,3,0,0,0},
            {0,0,0,1,1,0,1,0,1,1,1,1,3,0,0},
            {0,0,1,1,1,1,1,0,1,1,1,0,1,0,0},
            {0,1,1,1,2,1,1,0,1,1,1,1,1,1,0},
            {0,0,1,2,1,1,1,0,1,1,1,1,1,0,0},
            {0,0,0,1,1,1,1,0,1,2,1,1,0,0,0},
            {0,0,0,3,1,1,1,0,1,1,1,3,0,0,0},
            {0,0,0,0,3,1,1,0,1,1,3,0,2,0,0},
            {0,0,0,0,0,0,1,0,1,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,2,0,0,2,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}

        };

        GenerateMap(mapConfig);

        // ②プレイヤーの初期配置を生成 (列・行)
        playerWhite = new GameObject[] { Create("white_rook", 7, 0), Create("white_knight", 8, 0),
            Create("white_bishop", 9, 0), Create("white_queen", 10, 0), Create("white_king", 11, 0),
            Create("white_bishop", 12, 0), Create("white_knight", 13, 0), Create("white_rook", 14, 0),
            Create("white_pawn", 7, 1), Create("white_pawn", 8, 1), Create("white_pawn", 9, 1),
            Create("white_pawn", 10, 1), Create("white_pawn", 11, 1), Create("white_pawn", 12, 1),
            Create("white_pawn", 13, 1), Create("white_pawn", 14, 1) };
        playerBlack = new GameObject[] { Create("black_rook", 7, 14), Create("black_knight",6,14),
            Create("black_bishop",5,14), Create("black_queen",4,14), Create("black_king",3,14),
            Create("black_bishop",2,14), Create("black_knight",1,14), Create("black_rook",0,14),
            Create("black_pawn", 7, 13), Create("black_pawn", 6, 13), Create("black_pawn", 5, 13),
            Create("black_pawn", 4, 13), Create("black_pawn", 3, 13), Create("black_pawn", 2, 13),
            Create("black_pawn", 1, 13), Create("black_pawn", 0, 13) };

        // すべての駒の位置を "positions" ボードに設定
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

    void GenerateMap(int[,] mapConfig)
    {
        for (int x = 0; x < mapConfig.GetLength(0); x++)
        {
            for (int y = 0; y < mapConfig.GetLength(1); y++)
            {
                if (mapConfig[x, y] == 0)
                {
                    Instantiate(grassPrefab, new Vector3(y, x, 0), Quaternion.identity);
                }
                else if (mapConfig[x, y] == 1)
                {
                    Instantiate(waterPrefab, new Vector3(y, x, 0), Quaternion.identity);
                }
                else if (mapConfig[x, y] == 2)
                {
                    Instantiate(woodsPrefab, new Vector3(y, x, 0), Quaternion.identity);
                }
                else if (mapConfig[x, y] == 3)
                {
                    Instantiate(buffPrefab, new Vector3(y, x, 0), Quaternion.identity);
                }
                else if (mapConfig[x, y] == 4)
                {
                    Instantiate(holePrefab, new Vector3(y, x, 0), Quaternion.identity);
                }
                else if (mapConfig[x, y] == 10)
                {
                    Instantiate(warp0APrefab, new Vector3(y, x, 0), Quaternion.identity);
                }
                else if (mapConfig[x, y] == 11)
                {
                    Instantiate(warp0BPrefab, new Vector3(y, x, 0), Quaternion.identity);
                }
            }
        }
    }

    // 駒を生成して初期化
    public GameObject Create(string name, int x, int y)
    {
        //オブジェクトの生成＝Instantiate()
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        //生成したゲームオブジェクトから特定のコンポーネント（Chessman）を取得
        //実質　cm = Chessman
        Chessman cm = obj.GetComponent<Chessman>();
        cm.name = name; // これは Unity が持っている組み込み変数で、前もって宣言する必要はない
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate(); // すべてが設定されているので、これをアクティブにする
        //実質　GameObject＝obj 新しいオブジェクト
        return obj;
    }

    // 駒の位置を設定
    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        // 空白の場所またはそこにあるものを上書き
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    // 特定の位置を空に設定
    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    // 特定の位置の GameObject を取得
    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    // ボード上の位置かどうかを確認
    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    // 現在のプレイヤーを取得
    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    // ゲームが終了しているかどうかを確認
    public bool IsGameOver()
    {
        return gameOver;
    }

    // 次のターンに移動
    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }
    }

    // フレームごとに呼び出される更新関数
    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            // UnityEngine.SceneManagement を使用する必要がある
            SceneManager.LoadScene("GameOver"); // シーンを再読み込みしてゲームをリスタート
        }
    }

    // 勝者を設定してゲーム終了
    public void Winner(string playerWinner)
    {
        gameOver = true;

        // UnityEngine.UI を使用する必要がある
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " の勝利";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }
}
