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
    [SerializeField, Tooltip("タイムがスコアに与える影響を管理する係数")] float _timeScoreScaleIndex = 0.7f;
    [SerializeField, Tooltip("スコアがカウントアップする時間（秒）")] float _scoreCountUpTime = 1.5f;
    [SerializeField, Tooltip("スコアがカウントアップする遷移")] Ease _easeScoreCountUp = Ease.Linear;
    void Awake()
    {
        //シングルトン
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
    /// スコア加算とテキストでの演出
    /// </summary>
    /// <param name="targetTime">問題で定義された目標時間</param>
    /// <param name="currentTime">問題を解くのにかかった時間</param>
    /// <param name="baseScore">問題で定義された基礎スコア</param>
    /// <param name="_scoreText">スコアを反映するテキスト</param>
    /// <returns></returns>
    public int CaluculateAndAddScore(float targetTime, float currentTime, int baseScore, Text _scoreText)
    {
        int score = (int)Mathf.Floor((targetTime / currentTime) * baseScore);
        _scoreText.DOCounter(_currentScore, _currentScore + score, _scoreCountUpTime).SetEase(_easeScoreCountUp);
        _currentScore += score;
        return score;
    }
    //現在の保持しているスコアのリセット
    public void ResetScore()
    {
        _currentScore = 0;
    }
    /// <summary>
    /// 現在のスコアがランキングに入るか判定
    /// </summary>
    public void ScoreRankCheck()
    {
        List<int> highScoreList = SaveDataManager.Instance.GetSaveData._ranking.ToList();
        highScoreList.Add(_currentScore);
        //降順
        highScoreList = highScoreList.OrderBy(x => -x).ToList();
        int[] newRanking = new int[SaveDataManager.Instance.GetSaveData._ranking.Length];
        //新しく配列を作り値をコピーしてセーブデータに返す
        for (int i = 0; i < SaveDataManager.Instance.GetSaveData._ranking.Length; i++)
        {
            newRanking[i] = highScoreList[i];
        }
        SaveDataManager.Instance.GetSaveData._ranking = newRanking;
    }
}
