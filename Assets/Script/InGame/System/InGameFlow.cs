using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// ゲームの大まかな流れを処理するスクリプト
/// クリア、ポーズ、ゲームオーバー、クリア時に読み込まれる
/// </summary>
public class InGameFlow : MonoBehaviour
{
    public static InGameFlow Instance { get; private set; }

    //UI参照
    [SerializeField,Header("PlayerUI")] private GameObject _playerUI;
    [SerializeField,Header("スタートUI")] private GameObject _startUI;
    [SerializeField,Header("遊び方UI")] private GameObject _howToPlayUI;
    [SerializeField,Header("ポーズ画面UI")] private GameObject _pauseUI;
    [SerializeField,Header("ゲームオーバーUI")] private GameObject _gameOverUI;
    [SerializeField,Header("ゲームクリアUI")] private GameObject _gameClearUI;

    //UI時に使用するシステムの参照
    [SerializeField,Header("プレイヤーのインプットアクション")] private PlayerInput _playerInputSystem;
    [SerializeField,Header("敵管理のゲームオブジェクト")] private GameObject _enemyManager;

    //　プレイヤーの動きを制御するもの
    private PlayerInput _inputActions;

    //ポーズ画面を開くアクションの追加のもの
    private InputAction _pauseAction;

    //スタートUIが開かれたかどうかを確認するもの
    private bool _isStart;

    //画面上にアクションにより消せるUIが出た時の確認するもの
    private bool _isOK = false;

    //ポーズ中かどうかを確認する
    private bool _isPausing = false;

    [SerializeField,Header("通常BGM")] private AudioClip _normalBGM;

    private void Awake()
    {      
        if (Instance == null)
        {
            Instance = this;
        }

        //プレイヤーの動きを制御
        _playerInputSystem.enabled = false;

        //Pauseのインプットアクションは手動で追加  
        _inputActions = GetComponent<PlayerInput>();      
        _pauseAction = _inputActions.actions.FindActionMap("UI").FindAction("Pause");
        _pauseAction.performed += OnPause; 
        _pauseAction.Disable();
    }

    //時間を空けてStartUIを表示
    private async void Start()
    {
        SoundManager.Instance.PlayBGM(_normalBGM);
        switch (CheckPointManager.Instance.Check)
        {
            case 0:
                await UniTask.Delay(TimeSpan.FromSeconds(2));
                _startUI.SetActive(true);
                _isStart = true;  
                break;
            case 1:
                GameStart();
                break;

            default:
                GameStart();
                break;
        }
    }

    private void  Update()
    {
        if(!_startUI.activeSelf && _isStart)
        {
            _isStart = false;
            HowtoUI().Forget();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Retry();
        }
    }

    private async UniTask HowtoUI()
    {
        //遊び方のUI表示
        _howToPlayUI.SetActive(true);
        await WaitForInput();
        _howToPlayUI.SetActive(false);
        CheckPointManager.Instance.Check = 1;
        GameStart();
    }

    private void GameStart()
    {
        // ゲーム開始
        _pauseAction.Enable();
        _playerUI.SetActive(true);
        _playerInputSystem.enabled = true;
        _enemyManager.SetActive(true);
    }

    public void GameOver()
    {
        Debug.Log("a");
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void End()
    {
        Debug.Log("a");
    }


    /// <summary>
    /// クリックイベントされたら_isOK=trueにして,OKボタン押されるまで待機するメソッド
    /// </summary>
    private async UniTask WaitForInput()
    {
        while (!_isOK)
        {
            await UniTask.Yield(PlayerLoopTiming.Update);  
        }
        _isOK = false; 
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!_isOK)  
        {
            _isOK = true; 
        }
    }

    /// <summary>
    /// escキーを押すたびにポーズかどうかを切り替える
    /// </summary>
    public void OnPause(InputAction.CallbackContext context)
    {
        _isPausing = !_isPausing;
        _playerInputSystem.enabled = !_isPausing;
        _pauseUI.SetActive(_isPausing);
    }
}
