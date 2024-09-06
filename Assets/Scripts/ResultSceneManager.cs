using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField] Text _1stScoreNumText;
    [SerializeField] Text _2ndScoreNumText;
    [SerializeField] Text _3rdScoreNumText;
    [SerializeField] Text _currentScoreNumText;
    [SerializeField] Text _highScoreText;
    [SerializeField] Text _nextToClickText;
    [SerializeField] Vector3 _particleSpawnLowerLeft = new Vector3(-4, -4, -6);
    [SerializeField] Vector3 _particleSpawnUpperRight = new Vector3(4, 4, -2);
    [SerializeField, Tooltip("正解時のパーティクル")] GameObject _answerParticleObject;
    [SerializeField] float _spawnTime = 1.5f;
    float _currentTime = 0;
    bool _finishEffect = false;
    bool _sceneLoading = false;
    async private void Start()
    {
        _nextToClickText.text = "";
        _highScoreText.text = "";
        await _currentScoreNumText.DOCounter(0, ScoreManager.Instance.CurrentScore, 0.5f).AsyncWaitForCompletion();
        SEAudioManager.Instance.PlayClipSE(3);

        _3rdScoreNumText.DOCounter(0, SaveDataManager.Instance.GetSaveData._ranking[2], 0.5f);
        _2ndScoreNumText.DOCounter(0, SaveDataManager.Instance.GetSaveData._ranking[1], 0.5f);
        await _1stScoreNumText.DOCounter(0, SaveDataManager.Instance.GetSaveData._ranking[0], 0.5f).AsyncWaitForCompletion();

        if (SaveDataManager.Instance.GetSaveData._ranking.Contains(ScoreManager.Instance.CurrentScore))
        {
            _highScoreText.color = new Color(0.5f, 1, 0.5f, 0);
            _highScoreText.text = "ランキング更新！";
            _highScoreText.DOFade(1f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
        _nextToClickText.color = new Color(0, 0, 0, 0);
        _nextToClickText.text = "左クリックでタイトルへ";
        _nextToClickText.DOFade(1f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        _finishEffect = true;
    }
    private void Update()
    {
        _currentTime += Time.deltaTime;
        if(_currentTime > _spawnTime)
        {
            _currentTime = 0;
            //パーティクル生成
            ParticleSpawn();
        }
        if (_finishEffect && Input.GetMouseButtonDown(0))
        {
            ChangeTitle();
        }
    }
    async void ChangeTitle()
    {
        _sceneLoading = true;
        BGMAudioManager.Instance.StopBGM();
        await FadeManager.Instance.FadeAsync(null);
        SceneChangeManager.Instance.SceneChangeAsync(0);
        await FadeManager.Instance.UnFadeAsync(null);
        Debug.Log("unfade");
        BGMAudioManager.Instance.ChangeClipBGM(0);
    }
    /// <summary>
    /// 再起関数で約１．５秒に一回パーティクルを生成
    /// </summary>
    async Task ParticleSpawn()
    {
        if(_sceneLoading)
        {
            return;
        }
        await Task.Delay(1500);
        GameObject particleObject = _answerParticleObject;
        ParticleSystem particleSystem = particleObject.GetComponent<ParticleSystem>();
        particleSystem.startColor = Random.ColorHSV() * 1.5f;
        Instantiate(_answerParticleObject,
            new Vector3(Random.Range(_particleSpawnLowerLeft.x, _particleSpawnUpperRight.x),
            Random.Range(_particleSpawnLowerLeft.y, _particleSpawnUpperRight.y),
            Random.Range(_particleSpawnLowerLeft.z, _particleSpawnUpperRight.z)),
            Quaternion.identity);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_particleSpawnLowerLeft - _particleSpawnUpperRight, _particleSpawnUpperRight - _particleSpawnLowerLeft);
    }
}
