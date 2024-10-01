using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
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
    [SerializeField,Header("アイテムゲットUI")] private GameObject _itemGetUI;
    [SerializeField,Header("アイテム画像")] private Image _itemImage;
    [SerializeField,Header("アイテム名")] private Text _itemName;
    [SerializeField,Header("アイテムメッセージ")] private Text _itemMessage;
    [SerializeField,Header("メッセージUI")] private GameObject _messageUI;
    [SerializeField,Header("写すメッセージ")] private Text _showMessage;
    [SerializeField,Header("ポーズ画面UI")] private GameObject _pauseUI;
    [SerializeField,Header("ゲームオーバーUI")] private GameObject _gameOverUI;

    //UI時に使用するシステムの参照
    [SerializeField,Header("プレイヤーのインプットアクション")] private PlayerInput _playerInputSystem;
    [SerializeField,Header("敵管理のゲームオブジェクト")] private GameObject _enemyManager;

    //ポーズメニューのインプットシステム取得
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
            CheckPointManager.Instance.Check = 1;
            Retry();
        }
    }

    private async UniTask HowtoUI()
    {
        //遊び方のUI表示
        _howToPlayUI.SetActive(true);
        _isOK = false;
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
        ItemManager.Instance.LoadItemList();
    }

    public async UniTask ShowMessage(string message)
    {
        _isOK = false;
        Time.timeScale = 0f;
        _messageUI.SetActive(true);
        _showMessage.text = message;
        await WaitForInput();
        _messageUI.SetActive(false);
        Time.timeScale = 1f;
    }
    public async UniTask ItemGet(string ItemName,Sprite ItemImage,string message)
    {
        _isOK = false;
        Time.timeScale = 0f;
        _itemMessage.text = message;
        _itemImage.sprite = ItemImage;
        _itemGetUI.SetActive(true);
        await WaitForInput();
        _itemGetUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        _gameOverUI.SetActive(true);
    }

    public void Retry()
    {
        ItemManager.Instance.ClearItemList();  
        _pauseAction.performed -= OnPause;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void End()
    {
        CheckPointManager.Instance.Check = 0;
        ItemManager.Instance.ClearItemList();  
        Destroy(ItemManager.Instance.gameObject);
        SceneManager.LoadScene("Title");
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
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!_isOK && context.phase == InputActionPhase.Performed)  
        {
            _isOK = true;
        }
    }

    /// <summary>
    /// escキーを押すたびにポーズかどうかを切り替える
    /// プレイヤーと敵の動きを止める
    ///  処理が重くならないように,敵はEnemyManagerを確認してもし敵が出現しているならFindをする
    /// </summary>
    public void OnPause(InputAction.CallbackContext context)
    {
        _isPausing = !_isPausing;
        if(_isPausing)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        _playerInputSystem.enabled = !_isPausing;
        _pauseUI.SetActive(_isPausing);
        _playerUI.SetActive(!_isPausing);
    }

    //ゲームクリアフラグを取った時のみ呼び出す
    public void PauseOUT()
    {
        _pauseAction.performed -= OnPause;
    }
}
