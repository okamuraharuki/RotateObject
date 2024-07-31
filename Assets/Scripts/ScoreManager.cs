using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    static ScoreManager _instance;
    public static ScoreManager Instance => _instance;
    int _currentScore;
    public int CurrentScore => _currentScore;
    [SerializeField, Tooltip("�^�C�����X�R�A�ɗ^����e�����Ǘ�����W��")] float _timeScoreScaleIndex = 0.7f;
    [SerializeField, Tooltip("�X�R�A���J�E���g�A�b�v���鎞�ԁi�b�j")] float _scoreCountUpTime = 1.5f;
    [SerializeField, Tooltip("�X�R�A���J�E���g�A�b�v����J��")] Ease _easeScoreCountUp = Ease.Linear;
    void Awake()
    {
        //�V���O���g��
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    /// <summary>
    /// �X�R�A���Z�ƃe�L�X�g�ł̉��o
    /// </summary>
    /// <param name="targetTime">���Œ�`���ꂽ�ڕW����</param>
    /// <param name="currentTime">���������̂ɂ�����������</param>
    /// <param name="baseScore">���Œ�`���ꂽ��b�X�R�A</param>
    /// <param name="_scoreText">�X�R�A�𔽉f����e�L�X�g</param>
    /// <returns></returns>
    public int CaluculateAndAddScore(float targetTime, float currentTime, int baseScore, Text _scoreText)
    {
        int score = (int)Mathf.Floor((targetTime / currentTime) * baseScore);
        _scoreText.DOCounter(_currentScore, _currentScore + score, _scoreCountUpTime).SetEase(_easeScoreCountUp);
        _currentScore += score;
        return score;
    }
    //���݂̕ێ����Ă���X�R�A�̃��Z�b�g
    public void ResetScore()
    {
        _currentScore = 0;
    }
    /// <summary>
    /// ���݂̃X�R�A�������L���O�ɓ��邩����
    /// </summary>
    public void ScoreRankCheck()
    {
        List<int> highScoreList = SaveDataManager.Instance.GetSaveData._ranking.ToList();
        highScoreList.Add(_currentScore);
        //�~��
        highScoreList = highScoreList.OrderBy(x => -x).ToList();
        int[] newRanking = new int[SaveDataManager.Instance.GetSaveData._ranking.Length];
        //�V�����z������l���R�s�[���ăZ�[�u�f�[�^�ɕԂ�
        for (int i = 0; i < SaveDataManager.Instance.GetSaveData._ranking.Length; i++)
        {
            newRanking[i] = highScoreList[i];
        }
        SaveDataManager.Instance.GetSaveData._ranking = newRanking;
    }
}
