using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 敵の出現を管理するマネージャー
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField, Header("敵A")] private GameObject[] _easyEnemy;
    [SerializeField, Header("敵B")] private GameObject[] _normalEnemy;
    [SerializeField, Header("敵C")] private GameObject[] _hardEnemy;

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

    public void OnPlayerEnterTriggerPlace(GameObject respawnPlace)
    {
        // トリガーポイントに対応するリスポーン場所から敵を出現させる
        SpawnEnemy(respawnPlace.transform.position);
    }

    /// <summary>
    /// 敵を出現させるメソッド
    /// </summary>
    /// <param name="spawnPosition">敵のリスポーン場所</param>
    private void SpawnEnemy(Vector3 spawnPosition)
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
        Instantiate(respawnEnemies[randomIndex], spawnPosition, Quaternion.identity);
    }
}
