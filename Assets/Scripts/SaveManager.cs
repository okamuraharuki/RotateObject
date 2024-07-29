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
        //�Z�[�u�t�@�C����T���đ��݂���Ȃ烍�[�h
        _filepath = Application.dataPath + "/" + _fileName;
        // �t�@�C�����Ȃ��Ƃ��A�t�@�C���쐬
        if (!File.Exists(_filepath))
        {
            //�����ݒ�p�̃N���X�쐬
            SaveData initialSaveData = new SaveData();
            Save(initialSaveData);
        }
        // �t�@�C����ǂݍ����data�Ɋi�[
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
        //�Q�[���I�����ɕۑ�
        _saveData._sensitivityIndex = RotationManager.Instance.SensivilityIndex;
        _saveData._muteBGM = BGMAudioManager.Instance.GetIsMuteBGM;
        _saveData._volumeBGM = BGMAudioManager.Instance.GetVolumeBGM;
        _saveData._muteSE = SEAudioManager.Instance.GetIsMuteSE;
        _saveData._volumeSE = SEAudioManager.Instance.GetVolumeSE;
        Save(_saveData);
    }
}
