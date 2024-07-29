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
    [SerializeField, Tooltip("�I�u�W�F�N�g�������̂̋��e�p�x")] private float _instantiateVectorRange = 25f;
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
        //�������I��
        _isInitialized = true;
        //�`���[�g���A���ڍs
        _isQuestion = true;
    }
    void Update()
    {
        if (!_isInitialized)
            return;
        //��蒆�̂ւ̑J��
        if (_isQuestion)
        {
        }
        //�`���[�g���A���ւ̑J��
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
        //�O�̖�肪����ꍇ����
        if (_questionIndex > 0)
        {
            Destroy(_questionObject);
            Destroy(_answerObject);
        }

        _isCorrect = false;
        //�����z�u�̌X���������_���ɒ��I�A�����w��p�x���X�������Ȃ��ꍇ������x���I
        Quaternion randomQuaternion = UnityEngine.Random.rotation;
        while(Quaternion.Angle(randomQuaternion, _answerObject.transform.rotation) <= _instantiateVectorRange)
        {
            randomQuaternion = UnityEngine.Random.rotation;
        }
        //��萶����RotationManager���Ő��낷��I�u�W�F�N�g�̏���������
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
    {//���u��
        Debug.Log("Clear");

    }

}
