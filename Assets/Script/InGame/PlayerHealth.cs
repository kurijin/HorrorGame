using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// PlayerのHPとスタミナ管理
/// スタミナがあれば走れるので、Playerの動きのスクリプトで監視
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    // プレイヤーのhpと無敵時間
    [SerializeField, Header("プレイヤーのHP")] private int _hp = 3;
    [SerializeField, Header("プレイヤーの無敵時間")] private int _waitTime = 3;

    // プレイヤーのスタミナとフレーム当たりの増加量
    [SerializeField, Header("プレイヤーのMAXスタミナ")] public float maxStamina = 100f;
    [SerializeField, Header("スタミナの増加率")] private float _increaseStamina = 0.05f;
    private float _stamina;

    //音声
    [SerializeField, Header("攻撃受けた音")] private AudioClip _damageSE;

    public void Start()
    {
        _stamina = maxStamina;
    }

    /// <summary>
    /// 敵から受けるダメージをここで処理
    /// 食らった後無敵時間
    /// </summary>
    /// <param name="power">敵キャラのパワー</param>
    public async void TakeDamage(int power)
    {
        SoundManager.Instance.PlaySE(_damageSE);
        _hp -= power;
        if (_hp <= 0)
        {
            Die();
        }
        await UniTask.Delay(TimeSpan.FromSeconds(_waitTime));
    }

    private void Die()
    {
        //死亡処理
    }

    public int GetHP()
    {
        return _hp;
    }

    public float GetStamina()
    {
        return _stamina;
    }

    /// <summary>
    /// 走ったらスタミナを消費させる
    /// </summary>
    /// <param name="runpower">減少率</param>
    public void ConsumeStamina(float runpower)
    {
        _stamina -= runpower;
        // スタミナが最低値を下回らないようにする
        _stamina = Mathf.Clamp(_stamina, 0, maxStamina);
    }

    void Update()
    {
        // スタミナを増加させる
        _stamina += _increaseStamina;
        // スタミナが最大値を超えないようにする
        _stamina = Mathf.Clamp(_stamina, 0, maxStamina);
    }
}