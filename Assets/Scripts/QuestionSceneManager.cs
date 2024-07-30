using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class QuestionSceneManager : MonoBehaviour
{
    [SerializeField, Tooltip("�`���[�g���A���̃e�L�X�g�f�[�^���X�g")] private List<TutorialText> _tutorialTextList;
    [SerializeField, Tooltip("�`���[�g���A���̃I�u�W�F�N�g�f�[�^")] private QuestionData _tutorialData;
    [SerializeField, Tooltip("���̃f�[�^���X�g")] private List<QuestionData> _questionDataList;

    [SerializeField, Tooltip("�������̃e�L�X�g")] Text _explainText;

    [SerializeField, Tooltip("�ڕW�^�C���e�L�X�g")] Text _baseTimeText;

    [SerializeField, Tooltip("[���N���b�N�Ŏ���]�̃e�L�X�g")] Text _nextText;

    [SerializeField, Tooltip("�^�C�}�[�e�L�X�g")] Text _timerText;
    Stopwatch _stopwatch = new Stopwatch();

    GameObject _questionObject;
    GameObject _answerObject;

    [SerializeField, Tooltip("��]����̃I�u�W�F�N�g�����ꏊ")] private GameObject _questionPosObject;
    [SerializeField, Tooltip("�����̃I�u�W�F�N�g�����ꏊ")] private GameObject _answerPosObject;

    [SerializeField, Tooltip("�I�u�W�F�N�g�������̋��e�p�x")] private float _instantiateVectorRange = 25f;
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
        //�`���[�g���A���ڍs
        _isQuestion = false;
        //��萶����RotationManager���Ő��낷��I�u�W�F�N�g�̏���������
        _questionObject = Instantiate(_tutorialData.GetQuestionObject, _questionPosObject.transform.position, _randomQuaternion);
        _answerObject = Instantiate(_tutorialData.GetAnswerObject, _answerPosObject.transform.position, _tutorialData.GetAnswerObject.transform.rotation);
        //�����z�u�̌X���������_���ɒ��I�A�����w��p�x���X�������Ȃ��ꍇ������x���I
        _randomQuaternion = UnityEngine.Random.rotation;
        while (Quaternion.Angle(_randomQuaternion, _answerObject.transform.rotation) <= _instantiateVectorRange)
        {
            _randomQuaternion = UnityEngine.Random.rotation;
        }
        _questionObject.transform.rotation = _randomQuaternion;

        RotationManager.Instance.Initialize(_answerObject.transform.rotation, _questionObject);

        //�`���[�g���A���p�̃e�L�X�g��\������
        _explainText.text = _tutorialTextList[_tutorialIndex].StringData;
        _timerText.text = "Time --:--.--";

        //�������I��
        _isInitializeManager = true;
    }
    async void Update()
    {
        if (!_isInitializeManager)
            return;
        //��蒆�̂ւ̑J��
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
                _nextText.text = "���N���b�N�Ŏ���";
                if (Input.GetMouseButtonDown(1))
                {
                    _isInitializeQuestion = false;
                }
            }
        }
        //�`���[�g���A���ւ̑J��
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
        //�O�̖�������
        Destroy(_questionObject);
        Destroy(_answerObject);
        //�^�C�}�[���Z�b�g
        _stopwatch.Restart();
        //�e�L�X�g��ύX����
        _nextText.text = "";
        _explainText.text = _questionDataList[_questionIndex].QuestionText;
        _timerText.text = "Time 00:00.00";
        _baseTimeText.text = $"{_questionDataList[_questionIndex].TargetTime}�b";
        //�����z�u�̌X���������_���ɒ��I�A�����w��p�x���X�������Ȃ��ꍇ������x���I
        _randomQuaternion = UnityEngine.Random.rotation;
        while (Quaternion.Angle(_randomQuaternion, _answerObject.transform.rotation) <= _instantiateVectorRange)
        {
            _randomQuaternion = UnityEngine.Random.rotation;
        }
        //��萶����RotationManager���Ő��낷��I�u�W�F�N�g�̏���������
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
    {//���u��
        UnityEngine.Debug.Log("Clear");
        await FadeManager.Instance.FadeAsync(null);
        SceneChangeManager.Instance.SceneChangeAsync(2);
        await FadeManager.Instance.UnFadeAsync(null);
        UnityEngine.Debug.Log("unfade");
        BGMAudioManager.Instance.ChangeClipBGM(2);
    }

}
