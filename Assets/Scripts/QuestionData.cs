using System;
using UnityEngine;

[Serializable]
public class QuestionData
{
    [SerializeField, Tooltip("プレイヤーが動かせるオブジェクト")] GameObject _questionObject;
    public GameObject GetQuestionObject => _questionObject;
    [SerializeField, Tooltip("正解として生成するオブジェクト")] GameObject _answerObject;
    public GameObject GetAnswerObject => _answerObject;
    [SerializeField] string _questionText;
    public string QuestionText => _questionText;
    [SerializeField, Tooltip("この問題の基礎得点")] int _defaultScore;
    public int DefaultScore => _defaultScore;

    [SerializeField, Tooltip("目標タイム(秒)")] float _targetTime;
    public float TargetTime => _targetTime;
}
