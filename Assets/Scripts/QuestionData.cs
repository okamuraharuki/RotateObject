using System;
using UnityEngine;
using UnityEngine.UI; 

[Serializable]
public class QuestionData
{
    [SerializeField, Tooltip("�v���C���[����������I�u�W�F�N�g")] GameObject _questionObject;
    public GameObject GetQuestionObject => _questionObject;
    [SerializeField, Tooltip("�����Ƃ��Đ�������I�u�W�F�N�g")] GameObject _answerObject;
    public GameObject GetAnswerObject => _answerObject;
    /*
    [SerializeField, Tooltip("���\���̍ۂɐ�����")] Text _questionText;
    public Text QuestionText => _questionText;
    */
    [SerializeField, Tooltip("���̖��̊�b���_")] int _defaultScore;
    public int DefaultScore => _defaultScore;

    [SerializeField, Tooltip("�ڕW�^�C��(�b)")] float _targetTime;
    public float TargetTime => _targetTime;
}
