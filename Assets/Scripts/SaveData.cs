
[System.Serializable]
public class SaveData
{
    const int _rankingCnt = 3;
    public int[] _ranking = new int[_rankingCnt];
    public float _sensitivityIndex = 0.5f;
    public float _volumeSE = 0.5f;
    public float _volumeBGM = 0.2f;
    public bool _muteSE = false;
    public bool _muteBGM = false;
}
