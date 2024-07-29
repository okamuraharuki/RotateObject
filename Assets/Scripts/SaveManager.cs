using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    static SaveManager _instance;
    public static SaveManager Instance => _instance;
    SaveData _saveData;
    public SaveData GetSaveData => _saveData;
    string _filepath;
    string _fileName = "SaveData.json";
    private void Awake()
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
        //セーブファイルを探して存在するならロード
        _filepath = Application.dataPath + "/" + _fileName;
        // ファイルがないとき、ファイル作成
        if (!File.Exists(_filepath))
        {
            //初期設定用のクラス作成
            SaveData initialSaveData = new SaveData();
            Save(initialSaveData);
        }
        // ファイルを読み込んでdataに格納
        _saveData = Load(_filepath);

    }
    void Save(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData);
        StreamWriter wr = new StreamWriter(_filepath, false);
        wr.WriteLine(json);
        wr.Close();
    }

    SaveData Load(string filePath)
    {
        StreamReader rd = new StreamReader(filePath);
        string json = rd.ReadToEnd();
        rd.Close();

        return JsonUtility.FromJson<SaveData>(json);
    }

    void OnDestroy()
    {
        //ゲーム終了時に保存
        _saveData._sensitivityIndex = RotationManager.Instance.SensivilityIndex;
        _saveData._muteBGM = BGMAudioManager.Instance.GetIsMuteBGM;
        _saveData._volumeBGM = BGMAudioManager.Instance.GetVolumeBGM;
        _saveData._muteSE = SEAudioManager.Instance.GetIsMuteSE;
        _saveData._volumeSE = SEAudioManager.Instance.GetVolumeSE;
        Save(_saveData);
    }
}
