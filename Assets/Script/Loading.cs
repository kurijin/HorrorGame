using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System;

public class Loading : MonoBehaviour
{
    [SerializeField, Header("進行状況を可視化するもの")] private Slider progressBar; 

    void Awake()
    {
        LoadInGameSceneWithLoadingScreen1().Forget();
    }

    private async UniTask LoadInGameSceneWithLoadingScreen1()
    {
        progressBar.value = 0f;
        
        AsyncOperation async1 = SceneManager.LoadSceneAsync("InGame");
        async1.allowSceneActivation = false; 

        // シーンの読み込みが完了するまで進行状況を取得
        while (!async1.isDone)
        {
            float progress = Mathf.Clamp01(async1.progress / 0.9f);
            progressBar.value = progress;

            if (async1.progress >= 0.9f)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.5)); 
                async1.allowSceneActivation = true; 
            }

            await UniTask.Yield(); 
        }
    }
}
