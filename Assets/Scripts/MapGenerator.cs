using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    [SerializeField] TextAsset mapText;
    [SerializeField] GameObject[] prefabs;

    const int WIDTH =15;
    const int HEIGHT =15;

    //追加　マップのサイズ用変数
    float mapSize;


    enum MAP_TYPE
    {
        GROUND,
        WATER,
        WOODS
    }

    MAP_TYPE[,] mapTable;
    public void GenerateMap()
    {
        _loadMapData();
        _createMap();
    }

    void _loadMapData()
    {
        string[] mapLines = mapText.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        int row = mapLines.Length;
        int col = mapLines[0].Split(new char[] { ',' }).Length;
        mapTable = new MAP_TYPE[col,row];

         for (int y = 0; y < row; y++)
        {
            string[] mapValues = mapLines[y].Split(new char[] { ',' });
            for (int x = 0; x < col; x++)
            {
                mapTable[x, y] = (MAP_TYPE)int.Parse(mapValues[x]);
            }
        }

        foreach(string mapLine in mapLines)
        {
            string[] mapValues = mapLine.Split(new char[] { ',' });
            foreach (string mapValue in mapValues)
            {
                Debug.Log(mapValue);
            }
        }
    }

    void _createMap()
    {
        //追加サイズを取得する
        mapSize = prefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;

        //mapTableの行のループ
        for (int y = 0;y < mapTable.GetLength(1); y++)
        {
            //mapTableの列のループ
            for(int x = 0;x < mapTable.GetLength(0); x++)
            {
                //現在のポジション
                Vector2Int pos = new Vector2Int(x, y);
                //Vector2Int pos = new Vector2Int(x - WIDTH / 2, y - HEIGHT / 2);
                //prefabsの中のmapTable[x,y]に当たるものを生成
                GameObject _map = Instantiate(prefabs[(int)mapTable[x,y]]);
                //追加　グラウンドを敷き詰める
                GameObject _ground = Instantiate(prefabs[(int)MAP_TYPE.GROUND]);

                //生成したゲームオブジェクトの位置を設定
                _map.transform.position = _screenPos(pos);
                _ground.transform.position = _screenPos(pos);
                //_map.transform.position = new Vector2(x - WIDTH / 2, y - HEIGHT / 2);
            }
        }
    }
    //追加　サイズを考慮したポジションを取得する関数
    public Vector2 _screenPos(Vector2Int _pos)
    {
        return new Vector2(_pos.x * mapSize, _pos.y * mapSize);
    }
}