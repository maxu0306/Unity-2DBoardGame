using UnityEngine;
using System.Collections;

public class Zoom01 : MonoBehaviour
{

    //カメラの設定
    private GameObject Object;
    private Camera cam;

    //カメラ範囲の設定
    public float maxCamSize = 9.0f;
    public float minCamSize = 3.0f;
    public float maxCamX = 81.0f / 16.0f;
    public float minCamX = -81.0f / 16.0f;
    public float maxCamY = 9.0f;
    public float minCamY = -9.0f;

    //直前の指同士の距離
    private float lastDist = 0.0f;

    //指同士の中心座標
    private Vector2 centerPos;

    private Vector3 t1;
    private Vector3 t;

    //一本指タッチしたワールド座標
    private Vector2 lastTouchWorld;
    private Vector2 nowTouchWorld;

    //直前の触れている指の数
    private int lastCount = 0;

    //スピード調整
    public float zoomSpeed = 1.0f;
    public float moveSpeed = 1.0f;

    //カメラサイズの差分
    float sizeDiff;

    //カメラが画面外を映さないように処理
    void CameraSlide()
    {
        if (cam.transform.position.x > maxCamX - cam.orthographicSize * 9 / 16)
        {
            cam.transform.position = new Vector3(maxCamX - cam.orthographicSize * 9 / 16, cam.transform.position.y, cam.transform.position.z);
        }
        if (cam.transform.position.x < minCamX + cam.orthographicSize * 9 / 16)
        {
            cam.transform.position = new Vector3(minCamX + cam.orthographicSize * 9 / 16, cam.transform.position.y, cam.transform.position.z);
        }
        if (cam.transform.position.y > maxCamY - cam.orthographicSize)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, maxCamY - cam.orthographicSize, cam.transform.position.z);
        }
        if (cam.transform.position.y < minCamY + cam.orthographicSize)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, minCamY + cam.orthographicSize, cam.transform.position.z);
        }

    }

    void Start()
    {
        // カメラを設定
        Object = GameObject.Find("Main Camera");
        cam = Object.GetComponent<Camera>();
        cam.orthographicSize = 9f;

        // カメラの初期位置を設定
        cam.transform.position = new Vector3(7f, 7f, cam.transform.position.z);
    }


    void Update()
    {
        //タッチが0
        if (Input.touchCount == 0)
        {
            lastCount = 0;
        }
        //スワイプ操作（直前のtouchCountを1以下にすることでピンチ操作後に反応しないようにしています。）
        else if (Input.touchCount == 1 && lastCount <= 1)
        {
            //タッチ位置取得
            Touch touch3 = Input.GetTouch(0);

            //触れたとき
            if (touch3.phase == TouchPhase.Began)
            {
                t = touch3.position;
                lastTouchWorld = cam.ScreenToWorldPoint(t);
            }
            //動いたら
            else if (touch3.phase == TouchPhase.Moved)
            {
                
                //前回の座標を保存
                nowTouchWorld = lastTouchWorld;
                //今の座標を保存
                t = touch3.position;
                nowTouchWorld = cam.ScreenToWorldPoint(t);
                //カメラを移動
                cam.transform.position = new Vector3(cam.transform.position.x + (lastTouchWorld.x - nowTouchWorld.x) * moveSpeed, cam.transform.position.y + (lastTouchWorld.y - nowTouchWorld.y) * moveSpeed, cam.transform.position.z);

                CameraSlide();
                
            }

            lastCount = 1;
        }
        //ピンチ操作
        else if (Input.touchCount >= 2)
        {
            //タッチ位置取得
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            //二本目が触れたとき
            if (touch2.phase == TouchPhase.Began)
            {
                //指同士の距離と中心座標を取得
                lastDist = Vector2.Distance(touch1.position, touch2.position);
                centerPos = (touch1.position + touch2.position) * 0.5f;
                centerPos = cam.ScreenToWorldPoint(centerPos);
            }

            //どちらかの指が動いたら
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                //指同士の距離を計算
                float newDist = Vector2.Distance(touch1.position, touch2.position);

                //ズーム処理
                sizeDiff = cam.orthographicSize * cam.orthographicSize * (newDist - lastDist) / 10000.0f * zoomSpeed;
                cam.orthographicSize -= sizeDiff;


                //カメラが拡大（縮小）しすぎた場合の制限
                if (cam.orthographicSize > maxCamSize)
                {
                    cam.orthographicSize = maxCamSize;
                }
                else if (cam.orthographicSize < minCamSize)
                {
                    cam.orthographicSize = minCamSize;
                }
                //指同士の中心位置がピンチ操作の中心となるようにカメラを平行移動
                else
                {
                    cam.transform.position = new Vector3(centerPos.x + cam.orthographicSize / (cam.orthographicSize + sizeDiff) * (cam.transform.position.x - centerPos.x), centerPos.y + cam.orthographicSize / (cam.orthographicSize + sizeDiff) * (cam.transform.position.y - centerPos.y), cam.transform.position.z);
                }
                CameraSlide();
            }
            lastCount = 2;
        }
    }
}