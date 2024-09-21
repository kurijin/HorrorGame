using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Playerの動きをするもの,InputActionで処理
/// 移動は前と後ろ,カメラで当たりを見渡す。
/// 歩く時に音をつける。
/// </summary>
public class Player : MonoBehaviour
{
    //キャラクターの移動に関するもの
    private CharacterController _characterController;
    private Vector2 _moveInput; 
    private Vector3 _moveDirection;
    [SerializeField] private float _moveSpeed = 5f;

    //カメラに関するパラメータ
    private Vector2 _lookInput;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _lookSensitivity = 0.5f; //カーソル感度
    [SerializeField] private float _maxLookAngle = 60f; //上下の見れる角度の制限
    private float _verticalRotation = 0f;

    //アニメーション
    private Animator _animator;

    //音声に関するもの
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private AudioClip _walkSound;


    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        //前後の移動に限定
        _moveDirection = transform.forward * _moveInput.y;

        if (_moveDirection.magnitude > 0.1f)
        {
            ///後ろに下がる時はその方向を見たまま下がる。
            if(_moveInput.y < 0)
            {
                transform.forward = -_moveDirection; 
            }
            else
            {
                transform.forward = _moveDirection; 
            }
            _animator.SetFloat("Speed", _moveDirection.magnitude); 
            _soundManager.PlaySE(_walkSound);
        }
        else
        {
            _animator.SetFloat("Speed", 0); 
            _soundManager.StopSE();
        }

        _characterController.Move(_moveDirection * _moveSpeed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //InputActionからMoveを受け取る
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //InputActionからLookを受け取る
        _lookInput = context.ReadValue<Vector2>();

        //左右
        float horizontalRotation = _lookInput.x * _lookSensitivity;
        transform.Rotate(0, horizontalRotation, 0);

        //上下
        _verticalRotation -= _lookInput.y * _lookSensitivity;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -_maxLookAngle, _maxLookAngle);

        _camera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }

}
