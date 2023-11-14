using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Chessman : MonoBehaviour
{
    // Unityシーン内のオブジェクトへの参照
    public GameObject controller; // ゲームコントローラーオブジェクト 飾り
    public GameObject movePlate;  // 移動プレートのプレハブ

    // チェスボード上の駒の位置（初期値）
    private int xBoard = -1;
    private int yBoard = -1;

    // 駒の所属プレイヤー（"black"または"white"）
    private string player;

    // 駒のスプライトへの参照（各駒ごとに指定）
    // 黒駒のスプライト
    public Sprite black_queen, black_knight, black_bishop, black_king, black_rook, black_pawn;
    // 白駒のスプライト
    public Sprite white_queen, white_knight, white_bishop, white_king, white_rook, white_pawn;


    // 駒の初期化
    public void Activate()
    {
        // ゲームコントローラーを取得
        controller = GameObject.FindGameObjectWithTag("GameController");

        // 駒の初期位置を設定
        SetCoords();

        // 駒の名前に基づいて適切なスプライトを選択し、所属プレイヤー(black OR white)を設定
        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    // 駒の位置を調整
    public void SetCoords()
    {
        // チェスボード座標をUnity座標に変換
        float x = xBoard;
        float y = yBoard;

        // Unity座標を設定
        this.transform.position = new Vector3(x, y, -1.0f);
    }

    // 駒のX座標を取得
    public int GetXBoard()
    {
        return xBoard;
    }

    // 駒のY座標を取得
    public int GetYBoard()
    {
        return yBoard;
    }

    // 駒のX座標を設定
    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    // 駒のY座標を設定
    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    public void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Buff"))
            {
                BuffChessChange(); // 駒のチェンジ
                break;
            }
            else if ((collider.CompareTag("Warp0A") || collider.CompareTag("Warp0B")) && !isWarping)
            {
                Teleport_AB(collider);
                ChangeTile();
                break;
            }
            else if ((collider.CompareTag("Warp0C") || collider.CompareTag("Warp0D")) && !isWarping_CD)
            {
                Teleport_CD(collider);
                ChangeTile();
                break;
            }
            else if ((collider.CompareTag("Warp0E") || collider.CompareTag("Warp0F")) && !isWarping_EF)
            {
                Teleport_EF(collider);
                ChangeTile();
                break;
            }
        }
    }

    private GameObject warpingFrom = null; // ワープ地形から出た地形の参照

    // 他のメンバ変数や関数の宣言
    private bool isWarping = false; // ワープ中かどうかを示すフラグ

    private void Teleport_AB(Collider2D collider)
    {
        // ワープ先のワープ地形のタグを取得
        // Warp0A➡Warp0B　Warp0B➡Warp0A　と対応している
        string warpDestinationTag = collider.CompareTag("Warp0A") ? "Warp0B" : "Warp0A";

        // ワープ先の地形を取得
        GameObject warpDestination = GameObject.FindGameObjectWithTag(warpDestinationTag);
        Game sc = controller.GetComponent<Game>();  // ゲームコントローラーの参照を取得

        if (warpDestination != null)
        {
            // ワープ元の地形のColliderを取得
            Collider2D originalTile = Physics2D.OverlapPoint(transform.position);

            // ワープ元がワープ地形の場合
            if (originalTile.CompareTag("Warp0A"))
            {
                // ワープ先とワープ元が同じでない場合にワープ処理を行う
                if (warpDestination != warpingFrom)
                {
                    // Unity座標も更新
                    Chessman chessman = GetComponent<Chessman>();
                    // ワープ元の座標
                    int startX = (int)transform.position.x;
                    int startY = (int)transform.position.y;
                    // ワープ先の座標
                    int endX = (int)warpDestination.transform.position.x;
                    int endY = (int)warpDestination.transform.position.y;

                    // Unity座標も更新
                    chessman.SetXBoard(endX);
                    chessman.SetYBoard(endY);
                    chessman.SetCoords();

                    // ワープ元から出たことを記録
                    warpingFrom = originalTile.gameObject;
                }
            }
            else if (originalTile.CompareTag("Warp0B"))
            {
                // ワープ先とワープ元が同じでない場合にワープ処理を行う
                if (warpDestination != warpingFrom)
                {
                    // Unity座標も更新
                    Chessman chessman = GetComponent<Chessman>();
                    // ワープ元の座標
                    int startX = (int)transform.position.x;
                    int startY = (int)transform.position.y;
                    // ワープ先の座標
                    int endX = (int)warpDestination.transform.position.x;
                    int endY = (int)warpDestination.transform.position.y;

                    // Unity座標も更新
                    chessman.SetXBoard(endX);
                    chessman.SetYBoard(endY);
                    chessman.SetCoords();

                    // ワープ元から出たことを記録
                    warpingFrom = originalTile.gameObject;
                }
            }
        }
    }

    private GameObject warpingFrom_CD = null; // ワープ地形から出た地形の参照

    // 他のメンバ変数や関数の宣言
    private bool isWarping_CD = false; // ワープ中かどうかを示すフラグ

    private void Teleport_CD(Collider2D collider)
    {
        // ワープ先のワープ地形のタグを取得
        // Warp0A➡Warp0B　Warp0B➡Warp0A　と対応している
        string warpDestinationTag = collider.CompareTag("Warp0C") ? "Warp0D" : "Warp0C";

        // ワープ先の地形を取得
        GameObject warpDestination = GameObject.FindGameObjectWithTag(warpDestinationTag);
        Game sc = controller.GetComponent<Game>();  // ゲームコントローラーの参照を取得

        if (warpDestination != null)
        {
            // ワープ元の地形のColliderを取得
            Collider2D originalTile = Physics2D.OverlapPoint(transform.position);

            // ワープ元がワープ地形の場合
            if (originalTile.CompareTag("Warp0C"))
            {
                // ワープ先とワープ元が同じでない場合にワープ処理を行う
                if (warpDestination != warpingFrom_CD)
                {
                    // Unity座標も更新
                    Chessman chessman = GetComponent<Chessman>();
                    // ワープ元の座標
                    int startX = (int)transform.position.x;
                    int startY = (int)transform.position.y;
                    // ワープ先の座標
                    int endX = (int)warpDestination.transform.position.x;
                    int endY = (int)warpDestination.transform.position.y;

                    // Unity座標も更新
                    chessman.SetXBoard(endX);
                    chessman.SetYBoard(endY);
                    chessman.SetCoords();

                    // ワープ元から出たことを記録
                    warpingFrom_CD = originalTile.gameObject;
                }
            }
            else if (originalTile.CompareTag("Warp0D"))
            {
                // ワープ先とワープ元が同じでない場合にワープ処理を行う
                if (warpDestination != warpingFrom_CD)
                {
                    // Unity座標も更新
                    Chessman chessman = GetComponent<Chessman>();
                    // ワープ元の座標
                    int startX = (int)transform.position.x;
                    int startY = (int)transform.position.y;
                    // ワープ先の座標
                    int endX = (int)warpDestination.transform.position.x;
                    int endY = (int)warpDestination.transform.position.y;

                    // Unity座標も更新
                    chessman.SetXBoard(endX);
                    chessman.SetYBoard(endY);
                    chessman.SetCoords();

                    // ワープ元から出たことを記録
                    warpingFrom_CD = originalTile.gameObject;
                }
            }
        }
    }

    private GameObject warpingFrom_EF = null; // ワープ地形から出た地形の参照

    // 他のメンバ変数や関数の宣言
    private bool isWarping_EF = false; // ワープ中かどうかを示すフラグ
    private void Teleport_EF(Collider2D collider)
    {
        // ワープ先のワープ地形のタグを取得
        // Warp0A➡Warp0B　Warp0B➡Warp0A　と対応している
        string warpDestinationTag = collider.CompareTag("Warp0E") ? "Warp0F" : "Warp0E";

        // ワープ先の地形を取得
        GameObject warpDestination = GameObject.FindGameObjectWithTag(warpDestinationTag);
        Game sc = controller.GetComponent<Game>();  // ゲームコントローラーの参照を取得

        if (warpDestination != null)
        {
            // ワープ元の地形のColliderを取得
            Collider2D originalTile = Physics2D.OverlapPoint(transform.position);

            // ワープ元がワープ地形の場合
            if (originalTile.CompareTag("Warp0E"))
            {
                // ワープ先とワープ元が同じでない場合にワープ処理を行う
                if (warpDestination != warpingFrom_EF)
                {
                    // Unity座標も更新
                    Chessman chessman = GetComponent<Chessman>();
                    // ワープ元の座標
                    int startX = (int)transform.position.x;
                    int startY = (int)transform.position.y;
                    // ワープ先の座標
                    int endX = (int)warpDestination.transform.position.x;
                    int endY = (int)warpDestination.transform.position.y;

                    // Unity座標も更新
                    chessman.SetXBoard(endX);
                    chessman.SetYBoard(endY);
                    chessman.SetCoords();

                    // ワープ元から出たことを記録
                    warpingFrom_EF = originalTile.gameObject;
                }
            }
            else if (originalTile.CompareTag("Warp0F"))
            {
                // ワープ先とワープ元が同じでない場合にワープ処理を行う
                if (warpDestination != warpingFrom_EF)
                {
                    // Unity座標も更新
                    Chessman chessman = GetComponent<Chessman>();
                    // ワープ元の座標
                    int startX = (int)transform.position.x;
                    int startY = (int)transform.position.y;
                    // ワープ先の座標
                    int endX = (int)warpDestination.transform.position.x;
                    int endY = (int)warpDestination.transform.position.y;

                    // Unity座標も更新
                    chessman.SetXBoard(endX);
                    chessman.SetYBoard(endY);
                    chessman.SetCoords();

                    // ワープ元から出たことを記録
                    warpingFrom_EF = originalTile.gameObject;
                }
            }
        }
    }

    // マウスボタンが離された時の処理
    private void OnMouseUp()
    {
        // ゲームが終了しておらず、駒の所属プレイヤーのターンの場合
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            // 前に選択された駒に関連する移動プレートを破棄
            DestroyMovePlates();

            // 駒の移動範囲
            InitiateMovePlates();
        }
    }

    // 移動プレートの破棄
    public void DestroyMovePlates()
    {
        // 既存の移動プレートを破棄
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]); // 注意: "Destroy" は非同期であるため注意が必要
        }
    }

    // 駒の名前に応じて移動プレートを生成
    public void InitiateMovePlates()
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position); // 駒の位置で全てのColliderを取得

        // 落とし穴地形
        bool isOnHole = false; // フラグ
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Hole"))
            {
                isOnHole = true;
                break; // Holeが見つかったらループを終了
            }
        }

        // 駒とHoleタグのプレハブが衝突している場合、処理を終了
        if (isOnHole)
        {
            return;
        }

        switch (this.name)
        {
            // クイーンの場合、斜めと直線の移動プレートを生成
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);    // 右方向への直線
                LineMovePlate(0, 1);    // 上方向への直線
                LineMovePlate(1, 1);    // 右上方向への斜め
                LineMovePlate(-1, 0);   // 左方向への直線
                LineMovePlate(0, -1);   // 下方向への直線
                LineMovePlate(-1, -1);  // 左下方向への斜め
                LineMovePlate(-1, 1);   // 右下方向への斜め
                LineMovePlate(1, -1);   // 左上方向への斜め
                break;

            // ナイトの場合、L字型の移動プレートを生成
            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;

            // ビショップの場合、斜めの移動プレートを生成
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);    // 右上方向への斜め
                LineMovePlate(1, -1);   // 右下方向への斜め
                LineMovePlate(-1, 1);   // 左上方向への斜め
                LineMovePlate(-1, -1);  // 左下方向への斜め
                break;

            // キングの場合、周囲の移動プレートを生成
            case "black_king":
            case "white_king":
                SurroundMovePlate();
                break;

            // ルークの場合、直線の移動プレートを生成
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);    // 右方向への直線
                LineMovePlate(0, 1);    // 上方向への直線
                LineMovePlate(-1, 0);   // 左方向への直線
                LineMovePlate(0, -1);   // 下方向への直線
                break;

            // ポーンの場合、特定の位置に移動プレートを生成
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1);  // 前方への移動
                PawnMovePlate(xBoard, yBoard + 1); // 上方向移動および攻撃
                PawnMovePlate(xBoard - 1, yBoard); // 左方向移動および攻撃
                PawnMovePlate(xBoard + 1, yBoard); // 右方向移動および攻撃

                break;
            case "white_pawn":
                PawnMovePlate(xBoard, yBoard + 1);  // 前方への移動
                PawnMovePlate(xBoard, yBoard - 1); // 下方向移動および攻撃
                PawnMovePlate(xBoard - 1, yBoard); // 左方向移動および攻撃
                PawnMovePlate(xBoard + 1, yBoard); // 右方向移動および攻撃
                break;
        }
    }

    // 斜め・直線の移動プレート生成
    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        // Gameの参照を取得
        Game sc = controller.GetComponent<Game>();

        // 現在の座標に移動先の座標を追加
        int x = xBoard + xIncrement;  // X座標の計算
        int y = yBoard + yIncrement;  // Y座標の計算

        // 障害物に当たったかどうかを示すフラグ
        bool hit2D = false; //障害物に当たっていない

        // 移動先がボード上にあり、移動先に駒が存在しない限り繰り返す
        while (sc.PositionOnBoard(x, y))
        {
            // ターゲット座標にあるタイルのColliderを取得
            Collider2D tileCollider = Physics2D.OverlapPoint(new Vector2(x, y));

            //tileCollider.GetComponent<BoxCollider2D>() != null
            if (tileCollider != null && tileCollider.CompareTag("Chessman")
                || tileCollider != null && tileCollider.CompareTag("Water")
                || tileCollider != null && tileCollider.CompareTag("Woods"))
            {
                // Box Collider 2Dがアタッチされたプレハブに衝突した場合、障害物に当たったフラグを設定してループを抜ける
                hit2D = true;//障害物に当たった場合
                break;
            }
            else if (tileCollider != null && tileCollider.CompareTag("Buff")
                || tileCollider != null && tileCollider.CompareTag("Hole")
                || tileCollider != null && tileCollider.CompareTag("Warp0A")
                || tileCollider != null && tileCollider.CompareTag("Warp0B")
                || tileCollider != null && tileCollider.CompareTag("Warp0C")
                || tileCollider != null && tileCollider.CompareTag("Warp0D")
                || tileCollider != null && tileCollider.CompareTag("Warp0E")
                || tileCollider != null && tileCollider.CompareTag("Warp0F"))
            {
                // BuffとHoleタグのプレハブまで移動範囲を表示
                MovePlateSpawn(x, y);
                break;
            }

            if (!hit2D)
            {
                // 障害物に当たっていない場合、移動プレートを生成
                MovePlateSpawn(x, y);
            }

            x = x + xIncrement;  // X座標を更新
            y = y + yIncrement;  // Y座標を更新
        }

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null)
        {
            // ターゲット座標にあるタイルのColliderを取得
            Collider2D tileCollider = Physics2D.OverlapPoint(new Vector2(x, y));

            // 移動先がボード上にあり、移動先に駒が存在する場合、かつ条件に合致する場合、攻撃可能な場所の移動プレートを生成
            if (sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }


    // L字型の移動プレート生成　ナイト
    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);  // 右上方向
        PointMovePlate(xBoard - 1, yBoard + 2);  // 左上方向
        PointMovePlate(xBoard + 2, yBoard + 1);  // 上方向
        PointMovePlate(xBoard + 2, yBoard - 1);  // 右下方向
        PointMovePlate(xBoard + 1, yBoard - 2);  // 右下方向
        PointMovePlate(xBoard - 1, yBoard - 2);  // 左下方向
        PointMovePlate(xBoard - 2, yBoard + 1);  // 左上方向
        PointMovePlate(xBoard - 2, yBoard - 1);  // 左下方向
    }


    // 周囲の移動プレート生成 キング
    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);       // 上方向
        PointMovePlate(xBoard, yBoard - 1);       // 下方向
        PointMovePlate(xBoard - 1, yBoard + 0);   // 左方向
        PointMovePlate(xBoard - 1, yBoard - 1);   // 左下方向
        PointMovePlate(xBoard - 1, yBoard + 1);   // 左上方向
        PointMovePlate(xBoard + 1, yBoard + 0);   // 右方向
        PointMovePlate(xBoard + 1, yBoard - 1);   // 右下方向
        PointMovePlate(xBoard + 1, yBoard + 1);   // 右上方向
    }


    // 特定位置への移動プレート生成
    public void PointMovePlate(int x, int y)
    {
        //実質　Game＝sc
        Game sc = controller.GetComponent<Game>();  // ゲームコントローラーの参照を取得
        if (sc.PositionOnBoard(x, y))  // ボード上に指定された座標が存在する場合
        {
            GameObject cp = sc.GetPosition(x, y);  // 指定された座標の駒を取得
            Collider2D tileCollider = Physics2D.OverlapPoint(new Vector2(x, y));  // 指定された座標のColliderを取得
            //cp = Chesspiece
            if (cp == null)  // 指定された座標に駒が存在しない場合
            {
                MovePlateSpawn(x, y);  // 移動プレートを生成
            }
            else if (cp.GetComponent<Chessman>().player != player)  // 指定された座標に敵の駒が存在する場合
            {
                MovePlateAttackSpawn(x, y);  // 攻撃可能な場所の移動プレートを生成
            }
        }
    }

    // ポーンの移動プレート生成
    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();  // ゲームコントローラーの参照を取得
        Collider2D tileCollider = Physics2D.OverlapPoint(new Vector2(x, y));  // 指定された座標のColliderを取得

        if (sc.PositionOnBoard(x, y))  // ボード上に指定された座標が存在する場合
        {
            // 指定された座標に駒が存在しない場合
            if (sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);
            }
            // 右方向の座標に敵の駒が存在する場合
            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);  // 攻撃可能な場所の移動プレートを生成
            }
            // 左方向の座標に敵の駒が存在する場合
            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);  // 攻撃可能な場所の移動プレートを生成
            }
        }
    }

    // 移動プレートの生成
    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        // ボード上の座標をUnityの座標に変換
        float x = matrixX;
        float y = matrixY;
        // ターゲット座標にあるタイルのColliderを取得
        Collider2D tileCollider = Physics2D.OverlapPoint(new Vector2(matrixX, matrixY));

        // 移動プレートを生成
        if (tileCollider.CompareTag("Tile") || tileCollider.CompareTag("Woods") && this.name.Contains("pawn")
            || tileCollider.CompareTag("Hole") || tileCollider.CompareTag("Buff") || tileCollider.CompareTag("Warp0A")
            || tileCollider.CompareTag("Warp0B") || tileCollider.CompareTag("Warp0C") || tileCollider.CompareTag("Warp0D")
            || tileCollider.CompareTag("Warp0E") || tileCollider.CompareTag("Warp0F"))
        {
            // 実際のUnity座標を設定して移動プレートを生成
            GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

            MovePlate mpScript = mp.GetComponent<MovePlate>();  // 移動プレートスクリプトの参照を取得
            mpScript.SetReference(gameObject);  // 移動プレートスクリプトにこの駒の参照を設定
            mpScript.SetCoords(matrixX, matrixY);  // 移動プレートスクリプトに座標情報を設定
        }
    }

    // 攻撃可能な場所の移動プレート生成
    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        // ボード上の座標をUnityの座標に変換
        float x = matrixX;
        float y = matrixY;

        // ボード上の座標をUnityの座標に変換
        Vector2 worldPosition = new Vector2(matrixX, matrixY);

        // ターゲット座標にあるすべてのCollider2Dを取得
        Collider2D[] tileColliders = Physics2D.OverlapPointAll(worldPosition);

        foreach (Collider2D tileCollider in tileColliders)
        {
            if (tileCollider.CompareTag("Tile") || tileCollider.CompareTag("Woods") && this.name.Contains("pawn")
                || tileCollider.CompareTag("Hole") || tileCollider.CompareTag("Buff"))
            {
                // 実際のUnity座標を設定して攻撃可能な場所の移動プレートを生成
                GameObject mp = Instantiate(movePlate, new Vector3(matrixX, matrixY, -3.0f), Quaternion.identity);

                MovePlate mpScript = mp.GetComponent<MovePlate>();  // 移動プレートスクリプトの参照を取得
                mpScript.attack = true;  // 攻撃可能な場所のフラグを設定
                mpScript.SetReference(gameObject);  // 移動プレートスクリプトにこの駒の参照を設定
                mpScript.SetCoords(matrixX, matrixY);  // 移動プレートスクリプトに座標情報を設定
            }
        }
    }

    // バフ地形
    public void BuffChessChange()
    {
        if (this.name.Contains("pawn"))
        {
            // ポーンの場合、ビショップに変更
            switch (player)
            {
                case "black":
                    this.name = "black_rook";
                    this.GetComponent<SpriteRenderer>().sprite = black_rook;
                    player = "black";
                    // バフ地形を通常の地形に変更
                    ChangeTile();
                    break;
                case "white":
                    this.name = "white_rook";
                    this.GetComponent<SpriteRenderer>().sprite = white_rook;
                    player = "white";
                    // バフ地形を通常の地形に変更
                    ChangeTile();
                    break;
            }
        }
    }

    // 地形を座標で特定して通常の地形に変更する関数
    private void ChangeTile()
    {
        // 駒の位置に存在するバフ地形のColliderを取得
        Collider2D tileCollider = Physics2D.OverlapPoint(transform.position);

        Game sc = controller.GetComponent<Game>();  // ゲームコントローラーの参照を取得

        if (tileCollider.CompareTag("Buff"))
        {
            // バフ地形を破棄して消失させる
            Destroy(tileCollider.gameObject);

            // バフ地形の位置に新しい地形（grassプレハブなど）を生成
            Instantiate(sc.grassPrefab, tileCollider.transform.position, Quaternion.identity);
            //break; // 特定のバフ地形が見つかったらループを終了
        }
        else if (tileCollider.CompareTag("Warp0A") || tileCollider.CompareTag("Warp0B"))
        {
            // ワープ元の地形を取得
            GameObject warp0A = GameObject.FindGameObjectWithTag("Warp0A");
            GameObject warp0B = GameObject.FindGameObjectWithTag("Warp0B");

            // GameObject[] tilesToWarp = GameObject.FindGameObjectsWithTag("Tile"); // Tileタグのオブジェクトを取得

            if (warp0A != null)
            {
                Destroy(warp0A.gameObject);
                Instantiate(sc.grassPrefab, warp0A.transform.position, Quaternion.identity);
            }
            if (warp0B != null)
            {
                Destroy(warp0B.gameObject);
                Instantiate(sc.grassPrefab, warp0B.transform.position, Quaternion.identity);
            }
        }

        else if (tileCollider.CompareTag("Warp0C") || tileCollider.CompareTag("Warp0D"))
        {
            GameObject warp0C = GameObject.FindGameObjectWithTag("Warp0C");
            GameObject warp0D = GameObject.FindGameObjectWithTag("Warp0D");

            if (warp0C != null)
            {
                Destroy(warp0C.gameObject);
                Instantiate(sc.grassPrefab, warp0C.transform.position, Quaternion.identity);
            }
            if (warp0D != null)
            {
                Destroy(warp0D.gameObject);
                Instantiate(sc.grassPrefab, warp0D.transform.position, Quaternion.identity);
            }

        }

        else if (tileCollider.CompareTag("Warp0E") || tileCollider.CompareTag("Warp0F"))
        {
            GameObject warp0E = GameObject.FindGameObjectWithTag("Warp0E");
            GameObject warp0F = GameObject.FindGameObjectWithTag("Warp0F");
            if (warp0E != null)
            {
                Destroy(warp0E.gameObject);
                Instantiate(sc.grassPrefab, warp0E.transform.position, Quaternion.identity);
            }
            if (warp0F != null)
            {
                Destroy(warp0F.gameObject);
                Instantiate(sc.grassPrefab, warp0F.transform.position, Quaternion.identity);
            }
        }
    }
}