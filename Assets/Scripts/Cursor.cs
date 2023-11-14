using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public GameObject redCursorPrefab; // 赤カーソルのプレハブ
    private GameObject currentRedCursor; // 現在表示されている赤カーソル

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリックが押された場合
        {
            // クリックした位置の座標を取得
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int clickedX = Mathf.RoundToInt(clickPosition.x);
            int clickedY = Mathf.RoundToInt(clickPosition.y);

            // 既存の赤カーソルを削除
            DestroyCurrentRedCursor();

            // クリックしたオブジェクト情報を取得
            RaycastHit2D hit2D = Physics2D.Raycast(clickPosition, Vector2.down);
            
            if (hit2D.collider != null)
            {
                SetPosition(hit2D.transform);
                GameObject clickedObject = hit2D.collider.gameObject;

                // クリックしたオブジェクトのスプライトやコンポーネントにアクセスすることも可能です
                SpriteRenderer spriteRenderer = clickedObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    Sprite clickedSprite = spriteRenderer.sprite;
                    Debug.Log("オブジェクト情報: " + clickedSprite.name + " オブジェクト座標: (" + clickedX + ", " + clickedY + ")");
                }
                // クリック位置にカーソルを表示
                Vector3 cursorPosition = new Vector3(clickedX, clickedY, 0);
                currentRedCursor = Instantiate(redCursorPrefab, hit2D.transform.position, Quaternion.identity);
            }
            else
            {
                SetPosition(hit2D.transform);
                GameObject clickedObject = hit2D.collider.gameObject;
                // プレハブに関する処理を追加
                Debug.Log(" オブジェクト座標: (" + clickedX + ", " + clickedY + ")");
                // クリック位置にカーソルを表示
                Vector3 cursorPosition = new Vector3(clickedX, clickedY, 0);
                currentRedCursor = Instantiate(redCursorPrefab, hit2D.transform.position, Quaternion.identity);
            }
        }
    }

    private void SetPosition(Transform target)
    {
        transform.position = target.position;
    }

    private void DestroyCurrentRedCursor()
    {
        if (currentRedCursor != null)
        {
            Destroy(currentRedCursor);
        }
    }
}
