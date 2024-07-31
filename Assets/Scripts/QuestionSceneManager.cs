using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class QuestionSceneManager : MonoBehaviour
{
    [SerializeField, Tooltip("チュートリアルのテキストデータリスト")] private List<TutorialText> _tutorialTextList;
    [SerializeField, Tooltip("チュートリアルのオブジェクトデータ")] private QuestionData _tutorialData;
    [SerializeField, Tooltip("問題のデータリスト")] private List<QuestionData> _questionDataList;

    [SerializeField, Tooltip("見出しのテキスト")] Text _upperText;
    [SerializeField, Tooltip("説明文のテキスト")] Text _explainText;

    [SerializeField, Tooltip("目標タイムテキスト")] Text _baseTimeText;

    [SerializeField, Tooltip("スコア表示のテキスト")] Text _scoreText;
    [SerializeField, Tooltip("加算したスコア表示のテキスト")] Text _addScoreText;
    [SerializeField, Tooltip("加算したスコア表示のアニメイター")] Animator _addScoreAnimator;

    [SerializeField, Tooltip("[左クリックで次へ]のテキスト")] Text _nextText;

    [SerializeField, Tooltip("タイマーテキスト")] Text _timerText;
    Stopwatch _stopwatch = new Stopwatch();

    GameObject _questionObject;
    GameObject _answerObject;

    [SerializeField, Tooltip("回転するのオブジェクト生成場所")] private GameObject _questionPosObject;
    [SerializeField, Tooltip("答えのオブジェクト生成場所")] private GameObject _answerPosObject;

    [SerializeField, Tooltip("オブジェクト生成時の許容角度")] private float _instantiateVectorRange = 25f;
    Quaternion _randomQuaternion;

    [SerializeField, Tooltip("正解時のパーティクルシステム群")] List<GameObject> _answerParticleObjectList;

    int _questionIndex = -1;
    int _tutorialIndex = 0;

    bool _isInitializeManager = false;
    bool _isQuestion = false;
    public bool IsQuestion => _isQuestion;

    bool _isInitializeQuestion = false;
    bool _isCountDown = false;
    bool _isScored = false;

    void Start()
    {
        ScoreManager.Instance.ResetScore();
        _questionIndex = -1;
        //チュートリアル移行
        _isQuestion = false;
        //問題生成とRotationManager内で正誤するオブジェクトの初期化する
        _questionObject = Instantiate(_tutorialData.GetQuestionObject, _questionPosObject.transform.position, _randomQuaternion);
        _answerObject = Instantiate(_tutorialData.GetAnswerObject, _answerPosObject.transform.position, _tutorialData.GetAnswerObject.transform.rotation);
        //初期配置の傾きをランダムに抽選、もし指定角度より傾きが少ない場合もう一度抽選
        _randomQuaternion = UnityEngine.Random.rotation;
        while (Quaternion.Angle(_randomQuaternion, _answerObject.transform.rotation) <= _instantiateVectorRange)
        {
            _randomQuaternion = UnityEngine.Random.rotation;
        }
        _questionObject.transform.rotation = _randomQuaternion;

        RotationManager.Instance.Initialize(_answerObject.transform.rotation, _questionObject);

        //チュートリアル用のテキストを表示する
        _explainText.text = _tutorialTextList[_tutorialIndex].StringData;
        _timerText.text = "Time --:--.--";

        //初期化終了
        _isInitializeManager = true;
    }
    async void Update()
    {
        if (!_isInitializeManager)
            return;
        //問題中のへの遷移
        if (_isQuestion)
        {//次の問題への遷移
            if (!_isInitializeQuestion)
            {
                if (!RotationManager.Instance.IsAnswerRotation && _questionIndex < _questionDataList.Count - 1)
                {
                    NextQuestion();
                    _isInitializeQuestion = true;
                }
                else if (!RotationManager.Instance.IsAnswerRotation)
                {
                    _isInitializeManager = false;
                    Clear();
                }
            }//問題を説いている途中
            else if (!RotationManager.Instance.IsCorrect)
            {
                _timerText.text = $"Time {_stopwatch.Elapsed.Minutes.ToString("D2")}:" +
                    $"{_stopwatch.Elapsed.Seconds.ToString("D2")}." +
                    $"{(_stopwatch.Elapsed.Milliseconds / 10).ToString("D2")}";
            }//正解時
            else
            {
                if (!_isScored)
                {
                    //UI切り替え
                    _stopwatch.Stop();
                    _nextText.text = "左クリックで次へ";
                    //パーティクルを出す
                    foreach(var particleObject in _answerParticleObjectList)
                    {
                        Instantiate(particleObject);
                    }
                    //スコア計算と加算
                    int addScore = ScoreManager.Instance.CaluculateAndAddScore
                       (_questionDataList[_questionIndex].TargetTime, (float)_stopwatch.Elapsed.Seconds, _questionDataList[_questionIndex].DefaultScore, _scoreText);
                    
                    //加算スコアの演出
                    UnityEngine.Debug.Log("AddScore");
                    _addScoreText.text = $"+{addScore}";
                    
                    _addScoreAnimator.SetBool("IsActive", true);
                    //正解時の処理完了
                    UnityEngine.Debug.Log("AddScoreComplete");
                    _isScored = true;
                }
                if (Input.GetMouseButtonDown(1))
                {
                    _addScoreAnimator.SetBool("IsActive", false);
                    _addScoreText.text = "";
                    _isInitializeQuestion = false;
                }
            }
        }
        //チュートリアルへの遷移
        else
        {
            if (_tutorialIndex + 1 < _tutorialTextList.Count)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    _tutorialIndex++;
                    _explainText.text = _tutorialTextList[_tutorialIndex].StringData;
                }
            }
            else
            {
                if (!_isCountDown && Input.GetMouseButtonDown(1))
                {
                    _isCountDown = true;
                    _nextText.text = "";
                    // カウントダウン
                    await CountDown();
                    //出題中のBGMに変える
                    BGMAudioManager.Instance.ChangeClipBGM(3);
                    UnityEngine.Debug.Log("tutorial to question");
                    _isQuestion = true;
                }
            }
        }
    }
    async Task CountDown()
    {
        _explainText.color = new Color(256, 0, 0);
        _explainText.text = "3";
        await Task.Delay(1000);
        _explainText.text = "2";
        await Task.Delay(1000);
        _explainText.text = "1";
        await Task.Delay(1000);
        _explainText.text = "Start";
        await Task.Delay(1000);
        //await UniTask.Delay(TimeSpan.FromSeconds(1));
        _explainText.color = new Color(0, 0, 0);
    }
    void NextQuestion()
    {
        _questionIndex++;
        if (_questionIndex >= _questionDataList.Count)
        {
            UnityEngine.Debug.LogWarning("next question is null\nplease check code error");
            return;
        }
        //前の問題を消去
        Destroy(_questionObject);
        Destroy(_answerObject);
        //タイマーリセット
        _stopwatch.Restart();
        //テキストを変更する
        _nextText.text = "";
        _explainText.text = _questionDataList[_questionIndex].QuestionText;
        _timerText.text = "Time 00:00.00";
        _baseTimeText.text = $"{_questionDataList[_questionIndex].TargetTime}秒";
        //初期配置の傾きをランダムに抽選、もし指定角度より傾きが少ない場合もう一度抽選
        _randomQuaternion = UnityEngine.Random.rotation;
        while (Quaternion.Angle(_randomQuaternion, _answerObject.transform.rotation) <= _instantiateVectorRange)
        {
            _randomQuaternion = UnityEngine.Random.rotation;
        }
        //問題生成とRotationManager内で正誤するオブジェクトの初期化する
        _isScored = false;
        _questionObject = Instantiate(_questionDataList[_questionIndex].GetQuestionObject, _questionPosObject.transform.position, _randomQuaternion);
        _answerObject = Instantiate(_questionDataList[_questionIndex].GetAnswerObject, _answerPosObject.transform.position, _questionDataList[_questionIndex].GetAnswerObject.transform.rotation);
        RotationManager.Instance.Initialize(_answerObject.transform.rotation, _questionObject);
        _stopwatch.Start();
    }
    public void ResetRotation()
    {
        _questionObject.transform.rotation = _randomQuaternion;
    }
    async void Clear()
    {//仮置き
        UnityEngine.Debug.Log("Clear");
        ScoreManager.Instance.ScoreRankCheck();
        UnityEngine.Debug.Log("change scene result");
        await FadeManager.Instance.FadeAsync(null);
        SceneChangeManager.Instance.SceneChangeAsync(2);
        await FadeManager.Instance.UnFadeAsync(null);
        UnityEngine.Debug.Log("unfade");
        BGMAudioManager.Instance.ChangeClipBGM(2);
    }

}
