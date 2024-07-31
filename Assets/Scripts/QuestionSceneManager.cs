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
    [SerializeField, Tooltip("�`���[�g���A���̃e�L�X�g�f�[�^���X�g")] private List<TutorialText> _tutorialTextList;
    [SerializeField, Tooltip("�`���[�g���A���̃I�u�W�F�N�g�f�[�^")] private QuestionData _tutorialData;
    [SerializeField, Tooltip("���̃f�[�^���X�g")] private List<QuestionData> _questionDataList;

    [SerializeField, Tooltip("���o���̃e�L�X�g")] Text _upperText;
    [SerializeField, Tooltip("�������̃e�L�X�g")] Text _explainText;

    [SerializeField, Tooltip("�ڕW�^�C���e�L�X�g")] Text _baseTimeText;

    [SerializeField, Tooltip("�X�R�A�\���̃e�L�X�g")] Text _scoreText;
    [SerializeField, Tooltip("���Z�����X�R�A�\���̃e�L�X�g")] Text _addScoreText;
    [SerializeField, Tooltip("���Z�����X�R�A�\���̃A�j���C�^�[")] Animator _addScoreAnimator;

    [SerializeField, Tooltip("[���N���b�N�Ŏ���]�̃e�L�X�g")] Text _nextText;

    [SerializeField, Tooltip("�^�C�}�[�e�L�X�g")] Text _timerText;
    Stopwatch _stopwatch = new Stopwatch();

    GameObject _questionObject;
    GameObject _answerObject;

    [SerializeField, Tooltip("��]����̃I�u�W�F�N�g�����ꏊ")] private GameObject _questionPosObject;
    [SerializeField, Tooltip("�����̃I�u�W�F�N�g�����ꏊ")] private GameObject _answerPosObject;

    [SerializeField, Tooltip("�I�u�W�F�N�g�������̋��e�p�x")] private float _instantiateVectorRange = 25f;
    Quaternion _randomQuaternion;

    [SerializeField, Tooltip("�������̃p�[�e�B�N���V�X�e���Q")] List<GameObject> _answerParticleObjectList;

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
        {//���̖��ւ̑J��
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
            }//��������Ă���r��
            else if (!RotationManager.Instance.IsCorrect)
            {
                _timerText.text = $"Time {_stopwatch.Elapsed.Minutes.ToString("D2")}:" +
                    $"{_stopwatch.Elapsed.Seconds.ToString("D2")}." +
                    $"{(_stopwatch.Elapsed.Milliseconds / 10).ToString("D2")}";
            }//������
            else
            {
                if (!_isScored)
                {
                    //UI�؂�ւ�
                    _stopwatch.Stop();
                    _nextText.text = "���N���b�N�Ŏ���";
                    //�p�[�e�B�N�����o��
                    foreach(var particleObject in _answerParticleObjectList)
                    {
                        Instantiate(particleObject);
                    }
                    //�X�R�A�v�Z�Ɖ��Z
                    int addScore = ScoreManager.Instance.CaluculateAndAddScore
                       (_questionDataList[_questionIndex].TargetTime, (float)_stopwatch.Elapsed.Seconds, _questionDataList[_questionIndex].DefaultScore, _scoreText);
                    
                    //���Z�X�R�A�̉��o
                    UnityEngine.Debug.Log("AddScore");
                    _addScoreText.text = $"+{addScore}";
                    
                    _addScoreAnimator.SetBool("IsActive", true);
                    //�������̏�������
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
                if (!_isCountDown && Input.GetMouseButtonDown(1))
                {
                    _isCountDown = true;
                    _nextText.text = "";
                    // �J�E���g�_�E��
                    await CountDown();
                    //�o�蒆��BGM�ɕς���
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
    {//���u��
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
