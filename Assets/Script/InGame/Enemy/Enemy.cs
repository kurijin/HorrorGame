using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// 敵キャラの基底クラス
/// </summary>
public class Enemy : MonoBehaviour
{
    private GameObject _player;
    private PlayerHealth _playerHealth;
    [SerializeField, Header("敵の移動速度")] private float _speed = 3f;
    [SerializeField, Header("敵の消える時間")] private float _vanishTime;
    [SerializeField, Header("攻撃後の停止時間（秒）")] private float _stopDuration = 2f;  // 攻撃後の停止時間
    [SerializeField, Header("敵のBGM")] private AudioClip _bgm;
    [SerializeField, Header("敵のSE")] private AudioClip _se;

    private NavMeshAgent _myAgent;
    private Animator _animator;
    private float _elapsedTime;
    private bool _isAttacking = false;  // 攻撃中かどうかを判定するフラグ

    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _playerHealth = _player.GetComponent<PlayerHealth>();
        _myAgent = GetComponent<NavMeshAgent>();
        _myAgent.speed = _speed;
        //_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_isAttacking)
        {
            _myAgent.SetDestination(_player.transform.position);
        }

        _elapsedTime += Time.deltaTime;

        // 一定時間経過したら敵を消す
        if (_elapsedTime > _vanishTime)
        {
            Vanish();
        }
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }

    private async void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            await Attack();
        }
    }

    public virtual async UniTask Attack()
    {
        _myAgent.isStopped = true;
        _isAttacking = true;

        //_animator.SetTrigger("Attack");

        _playerHealth.TakeDamage(1);
        await UniTask.Delay(TimeSpan.FromSeconds(_stopDuration));

        _myAgent.isStopped = false;
        _isAttacking = false;
    }
}
