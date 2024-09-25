using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Playerの動きをするもの,InputActionで処理
/// 移動は前と後ろ,カメラで当たりを見渡す。
/// 歩く時に音をつける。
/// </summary>
public class Player : MonoBehaviour
{
    // キャラクターの移動に関するもの
    private CharacterController _characterController;
    private Vector2 _moveInput;
    private Vector3 _moveDirection;
    private float _moveSpeed; //現在の移動速度
    private bool isRunnig = false;

    [SerializeField, Header("スタミナ減少率")] private float _consumeStamina = 0.1f;
    [SerializeField, Header("プレイヤーの歩行速度")] private float _walkSpeed = 3f;
    [SerializeField, Header("プレイヤーの走る速度")] private float _runSpeed = 5f;
    [SerializeField, Header("無敵時間中の速度")] private float _speedInvincible = 6f;

    // 重力に関するパラメータ
    private Vector3 _velocity; 
    [SerializeField, Header("重力の強さ")] private float gravity = -9.81f;
    
    // カメラに関するパラメータ
    private Vector2 _lookInput;
    [SerializeField, Header("顔の前についているカメラ")] private Camera _camera;
    [SerializeField, Header("感度")] private float _lookSensitivity = 0.5f;
    [SerializeField, Header("上下で見れる角度")] private float _maxLookAngle = 60f;
    private float _verticalRotation = 0f;

    // アニメーション
    private Animator _animator;

    // 音声に関するもの
    [SerializeField, Header("歩行音")] private AudioClip _walkSound;
    [SerializeField, Header("走る音")] private AudioClip _runSound;

    /// <summary>
    /// walkのサウンド、runのサウンドどちらが流れているかを監視
    /// </summary>
    private bool isPlayingWalkSound = false;
    private bool isPlayingRunSound = false;
    private PlayerHealth _playerHealth;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (isRunnig && _playerHealth.GetStamina() > 0)
        {
            _moveSpeed = _runSpeed;
            _playerHealth.ConsumeStamina(_consumeStamina);
        }
        else if(_playerHealth.isInvincible)
        {
            _moveSpeed = _speedInvincible;
        }
        else
        {
            _moveSpeed = _walkSpeed;
        }

        // 前後の移動に限定
        _moveDirection = transform.forward * _moveInput.y;

        if (_moveDirection.magnitude > 0.1f)
        {
            /// 後ろに下がる時はその方向を見たまま下がる。
            if (_moveInput.y < 0)
            {
                transform.forward = -_moveDirection;
            }
            else
            {
                transform.forward = _moveDirection;
            }

            _animator.SetFloat("Speed", _moveSpeed);
        }
        else
        {
            _animator.SetFloat("Speed", 0);
            SoundManager.Instance.StopSE();
            isPlayingWalkSound = false;
            isPlayingRunSound = false;
        }

        if (_characterController.isGrounded)
        {
            _velocity.y = 0f;
        }

        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move((_moveDirection * _moveSpeed + _velocity) * Time.deltaTime);  
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // InputActionからMoveを受け取る
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                isRunnig = true;
                break;

            case InputActionPhase.Canceled:
                isRunnig = false;
                break;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // InputActionからLookを受け取る
        _lookInput = context.ReadValue<Vector2>();

        // 左右の回転
        float horizontalRotation = _lookInput.x * _lookSensitivity;
        transform.Rotate(0, horizontalRotation, 0);

        // 上下の回転
        _verticalRotation -= _lookInput.y * _lookSensitivity;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_maxLookAngle, _maxLookAngle);

        _camera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }

    /// <summary>
    /// 歩行音アニメーションイベントで呼ぶもの
    /// </summary>
    public void WalkSound()
    {
        if (!isPlayingWalkSound)
        {
            SoundManager.Instance.StopSE();
            SoundManager.Instance.PlaySE(_walkSound);
            isPlayingWalkSound = true;
            isPlayingRunSound = false;
        }
    }

    public void RunSound()
    {
        if (!isPlayingRunSound)
        {
            SoundManager.Instance.StopSE();
            SoundManager.Instance.PlaySE(_runSound);
            isPlayingRunSound = true;
            isPlayingWalkSound = false;
        }
    }
}
