using UnityEngine;
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
    [SerializeField,Header("スタートUI")] private GameObject _startUI;
    [SerializeField,Header("遊び方UI")] private GameObject _howToPlayUI;
    [SerializeField,Header("ポーズ画面UI")] private GameObject _pauseUI;
    [SerializeField,Header("ゲームオーバーUI")] private GameObject _gameOverUI;
    [SerializeField,Header("ゲームクリアUI")] private GameObject _gameClearUI;

    //UI時に使用するシステムの参照
    [SerializeField,Header("プレイヤーのインプットアクション")] private PlayerInput _playerInputSystem;
    [SerializeField,Header("敵管理のゲームオブジェクト")] private GameObject _enemyManager;

    [SerializeField,Header("UIのinputsystemの参照")] private InputActionAsset inputActions;


    private InputAction _submitAction;
    private InputAction _pauseAction;
    private bool _isStart = true;

    private bool _isOK = false;
    private bool _isPausing = false;

    private void Awake()
    {        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); 
        }
        else
        {
            Destroy(this.gameObject);
        }
        //プレイヤーの動きを制御
        _playerInputSystem.enabled = false;

        //Pauseのインプットアクションは手動で追加
        _pauseAction = inputActions.FindActionMap("UI").FindAction("Pause"); 
        _pauseAction.performed += OnPause; 
    }

    //時間を空けてStartUIを表示
    private async void Start()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        _startUI.SetActive(true);
        _isStart = false; 
    }

    private void  Update()
    {
        if(!_startUI.activeSelf && !_isStart)
        {
            _isStart = true;
            HowtoUI().Forget();
        }
    }

    private async UniTask HowtoUI()
    {
        //遊び方のUI表示
        _howToPlayUI.SetActive(true);
        await WaitForInput();
        _howToPlayUI.SetActive(false);

        // ゲーム開始
        _pauseAction.Enable();
        _playerInputSystem.enabled = true;
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
    /// トグルで処理
    /// </summary>
    public void OnPause(InputAction.CallbackContext context)
    {
        _isPausing = !_isPausing;
        _playerInputSystem.enabled = !_isPausing;
        _pauseUI.SetActive(_isPausing);
    }
}
