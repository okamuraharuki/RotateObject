using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class QuestionSceneManager : MonoBehaviour
{
    bool _isPlayingQ = false;
    public bool IsPlayingQ => _isPlayingQ;
    Action _nextAction;
    Action _clearAction;
    [SerializeField] private List<TutorialText> _tutorialTextList;
    [SerializeField] private List<QuestionData> _questionDataList;
    GameObject _questionObject;
    RotationManager _objectRotation;
    GameObject _answerObject;
    [SerializeField] private Vector3 _questionPos;
    [SerializeField] private Vector3 _answerPos;
    [SerializeField, Tooltip("オブジェクト生成時のの許容角度")] private float _instantiateVectorRange = 25f;
    int _questionIndex = 0;
    int _turtrialIndex = 0;
    bool _isInitialized = false;
    bool _isQuestion = false;
    bool _isRotation = false;
    bool _isCorrect = false;
    public bool IsCorrect => _isCorrect;
    void Start()
    {
        _questionIndex = 0;

        _nextAction += NextQuestion;
        _clearAction += Clear;
        //初期化終了
        _isInitialized = true;
        //チュートリアル移行
        _isQuestion = true;
    }
    void Update()
    {
        if (!_isInitialized)
            return;
        //問題中のへの遷移
        if (_isQuestion)
        {
        }
        //チュートリアルへの遷移
        else
        {
            if (_turtrialIndex < _tutorialTextList.Count)
            {

            }
            else
            {
                _isQuestion = true;
            }
        }
    }
    void NextQuestion()
    {
        if(_questionIndex >= _questionDataList.Count)
        {
            Debug.Log("next question is null\nplease check code error");
            return;
        }
        //前の問題がある場合消去
        if (_questionIndex > 0)
        {
            Destroy(_questionObject);
            Destroy(_answerObject);
        }

        _isCorrect = false;
        //初期配置の傾きをランダムに抽選、もし指定角度より傾きが少ない場合もう一度抽選
        Quaternion randomQuaternion = UnityEngine.Random.rotation;
        while(Quaternion.Angle(randomQuaternion, _answerObject.transform.rotation) <= _instantiateVectorRange)
        {
            randomQuaternion = UnityEngine.Random.rotation;
        }
        //問題生成とRotationManager内で正誤するオブジェクトの初期化する
        _questionObject = Instantiate(_questionDataList[_questionIndex].GetQuestionObject, _questionPos, randomQuaternion);
        _answerObject = Instantiate(_questionDataList[_questionIndex].GetAnswerObject, _answerPos, _questionDataList[_questionIndex].GetAnswerObject.transform.rotation);
        RotationManager.Instance.Initialize(_answerObject.transform.rotation, _questionObject);
        _questionIndex++;

    }
    void NextTextChange()
    {
        if(_isQuestion)
        {

        }
        else
        {

        }
    }
    void Clear()
    {//仮置き
        Debug.Log("Clear");

    }

}
