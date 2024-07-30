using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class QuestionSceneManager : MonoBehaviour
{
    [SerializeField, Tooltip("チュートリアルのテキストデータリスト")] private List<TutorialText> _tutorialTextList;
    [SerializeField, Tooltip("チュートリアルのオブジェクトデータ")] private QuestionData _tutorialData;
    [SerializeField, Tooltip("問題のデータリスト")] private List<QuestionData> _questionDataList;

    [SerializeField, Tooltip("説明文のテキスト")] Text _explainText;

    [SerializeField, Tooltip("目標タイムテキスト")] Text _baseTimeText;

    [SerializeField, Tooltip("[左クリックで次へ]のテキスト")] Text _nextText;

    [SerializeField, Tooltip("タイマーテキスト")] Text _timerText;
    Stopwatch _stopwatch = new Stopwatch();

    GameObject _questionObject;
    GameObject _answerObject;

    [SerializeField, Tooltip("回転するのオブジェクト生成場所")] private GameObject _questionPosObject;
    [SerializeField, Tooltip("答えのオブジェクト生成場所")] private GameObject _answerPosObject;

    [SerializeField, Tooltip("オブジェクト生成時の許容角度")] private float _instantiateVectorRange = 25f;
    Quaternion _randomQuaternion;

    int _questionIndex = 0;
    int _tutorialIndex = 0;

    bool _isInitializeManager = false;
    bool _isQuestion = false;
    public bool IsQuestion => _isQuestion;

    bool _isInitializeQuestion = false;
    void Start()
    {
        _questionIndex = 0;
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
        {
            if(!_isInitializeQuestion)
            {
                if(!RotationManager.Instance.IsAnswerRotation && _questionIndex < _questionDataList.Count)
                {
                    NextQuestion();
                    _isInitializeQuestion = true;
                }
                else if(!RotationManager.Instance.IsAnswerRotation && _questionIndex >= _questionDataList.Count)
                {
                    _isInitializeManager = false;
                    Clear();
                }
            }
            else if(!RotationManager.Instance.IsCorrect)
            {
                _timerText.text = $"Time {_stopwatch.Elapsed.Minutes.ToString("D2")}:{_stopwatch.Elapsed.Seconds.ToString("D2")}.{(_stopwatch.Elapsed.Milliseconds / 10).ToString("D2")}";
            }
            else
            {
                _stopwatch.Stop();
                _nextText.text = "左クリックで次へ";
                if (Input.GetMouseButtonDown(1))
                {
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
                if (Input.GetMouseButtonDown(1))
                {
                    await _explainText.DOCounter(3, 1, 3).SetEase(Ease.Linear).AsyncWaitForCompletion();
                    _explainText.text = "Start";
                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                    BGMAudioManager.Instance.ChangeClipBGM(3);
                    UnityEngine.Debug.Log("tutorial to question");
                    _isQuestion = true;
                }
            }
        }
    }
    void NextQuestion()
    {
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
        _questionObject = Instantiate(_questionDataList[_questionIndex].GetQuestionObject, _questionPosObject.transform.position, _randomQuaternion);
        _answerObject = Instantiate(_questionDataList[_questionIndex].GetAnswerObject, _answerPosObject.transform.position, _questionDataList[_questionIndex].GetAnswerObject.transform.rotation);
        RotationManager.Instance.Initialize(_answerObject.transform.rotation, _questionObject);
        _stopwatch.Start();
        _questionIndex++;

    }
    public void ResetRotation()
    {
        _questionObject.transform.rotation = _randomQuaternion;
    }
    async void Clear()
    {//仮置き
        UnityEngine.Debug.Log("Clear");
        await FadeManager.Instance.FadeAsync(null);
        SceneChangeManager.Instance.SceneChangeAsync(2);
        await FadeManager.Instance.UnFadeAsync(null);
        UnityEngine.Debug.Log("unfade");
        BGMAudioManager.Instance.ChangeClipBGM(2);
    }

}
