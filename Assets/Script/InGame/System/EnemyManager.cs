using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 敵の出現を管理するマネージャー
/// 敵の出現トリガーをONにするのは色々なスクリプトから呼ぶ(→フラグが立たないとトリガーは出現しない.)
/// spotID == EnemyIDで敵が死んだ時このスクリプトのチェックポイントEnemyが呼び出されてそれとともにid(spotID,enemyID)が渡される
/// それによってどこの敵かを考え、セーブポイントを作る。
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField, Header("敵A")] private GameObject[] _easyEnemy;
    [SerializeField, Header("敵B")] private GameObject[] _normalEnemy;
    [SerializeField, Header("敵C")] private GameObject[] _hardEnemy;
    [SerializeField, Header("プレイヤー")] private GameObject _player;
    [SerializeField, Header("敵の出現トリガー")] private GameObject[] _enemyTrigger;

    private int _currentLevel;
    private float _elapsedTime;

    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // 難易度を取得
        _currentLevel = DifficultyManager.Instance.Level;
    }

    /// <summary>
    /// spotによってはあるフラグを通過したらトリガーを有効にする
    /// リスタート時,そのトリガーで一回出てたら削除する
    /// </summary>
    /// <param name="spotnumber">トリガースポットの地点</param>

    public void ActiveTrigger(int spotnumber)
    {
        _enemyTrigger[spotnumber].SetActive(true);
    }
    public void DeleteTrigger(int spotnumber)
    {
        Destroy(_enemyTrigger[spotnumber]);
    }

    //時間経過やアクションにより難易度を変更さしたい場合に使用
    ///  private void Update()
    ///  {
    ///     _currentLevel = DifficultyManager.Instance.Level;
    ///     _elapsedTime += Time.deltaTime;
    ///  }




    /// <summary>
    /// プレイヤーがトリガーポイントに到達したらそこからこのスクリプトが呼ばれる。
    /// </summary>
    /// <param name="respawnPlace">敵のリスポーンする場所</param>

    public void OnPlayerEnterTriggerPlace(GameObject respawnPlace,int id)
    {
        // トリガーポイントに対応するリスポーン場所から敵を出現させる
        SpawnEnemy(respawnPlace.transform.position, id);
    }


    /// <summary>
    /// 敵を出現させるメソッド
    /// </summary>
    /// <param name="spawnPosition">敵のリスポーン場所</param>
    /// <param name="id">スポットごとのID</param>
    private void SpawnEnemy(Vector3 spawnPosition, int id)
    {
        GameObject[] respawnEnemies;

        // 難易度に応じた敵のグループを選択
        switch (_currentLevel)
        {
            case 0:
                respawnEnemies = _easyEnemy
                                .Concat(_normalEnemy)
                                .Concat(_hardEnemy)
                                .ToArray();
                break;
            case 1:
                respawnEnemies = _easyEnemy;
                break;
            case 2:
                respawnEnemies = _normalEnemy;
                break;
            case 3:
                respawnEnemies = _hardEnemy;
                break;
            default:
                respawnEnemies = _normalEnemy;
                break;
        }

        // 各敵グループに格納されている敵からランダムに選んで出現させる
        int randomIndex = Random.Range(0, respawnEnemies.Length);  
        GameObject spawnedEnemy = Instantiate(respawnEnemies[randomIndex], spawnPosition, Quaternion.identity);

        Enemy enemyComponent = spawnedEnemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.SetID(id);
        }
    }

    /// <summary>
    /// プレイヤーを渡すもの
    /// </summary>
    /// <returns></returns>
    public GameObject GetPlayer()
    {
        return _player;
    }

    /// <summary>
    /// どこの敵が消えたかによってチェックポイントを更新
    /// </summary>
    /// <param name="ID">トリガーポイントのid ,敵id</param>
    public void EnemyCheckpoint(int ID)
    {
        switch(ID)
        {
            case 1:
                CheckPointManager.Instance.Check = 2;
                break;
            case 2:
                CheckPointManager.Instance.Check = 3;
                break;
            case 3:
                CheckPointManager.Instance.Check = 4;
                break;
            default:
                break;
        }

    }
}
