using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RotationManager : MonoBehaviour
{
    static RotationManager _instance;
    public static RotationManager Instance => _instance;

    [SerializeField, Tooltip("オブジェクトの回転速度の基礎値（0.2-2.0倍で変わる）")] float _basicSensivility = 1.5f;
    [SerializeField, Tooltip("sensivilityIndexをいじるスライダー")] Slider _sensivilitySlider;
    float _sensivilityIndex = 0;
    public float SensivilityIndex => _sensivilityIndex;

    [SerializeField] GameObject _initialAnswerObject;
    [SerializeField] GameObject _initialRotationObject;

    GameObject _rotationObject;
    Quaternion _answerRotation;

    [SerializeField, Tooltip("正誤判定のオブジェクトの許容角度")] private float _answerVectorRange = 15f;
    [SerializeField, Tooltip("正解時の、物体が回転しきるまでの時間")] float _timeRotation = 2f;
    [SerializeField, Tooltip("正解時の、物体の回転量の推移")] Ease _easeRotation = Ease.Linear;

    QuestionSceneManager _questionSceneManager;

    bool _isCorrect = false;
    public bool IsCorrect => _isCorrect;
    bool _initializeObject = false;
    public bool InitializeObject => _initializeObject;
    bool _isMovable = false;

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
    }
    private void Start()
    {
        //セーブデータからスライダーと設定を初期化
        _sensivilityIndex = SaveManager.Instance.GetSaveData._sensitivityIndex;
        _sensivilitySlider.value = _sensivilityIndex;
        //タイトルシーンに合わせたオブジェクトの初期化
        Initialize(_initialAnswerObject.transform.rotation, _initialRotationObject);
    }
    /// <summary>
    /// シーンの変更時に呼んで初期化する
    /// </summary>
    /// <param name="answerRotation">正解のオブジェクトのrotation</param>
    public void Initialize(Quaternion answerRotation, GameObject rotationObject)
    {
        _questionSceneManager = FindAnyObjectByType<QuestionSceneManager>();
        _answerRotation = answerRotation;
        _rotationObject = rotationObject;
        _isCorrect = false;
        _initializeObject = true;
    }

    private async void Update()
    {
        if(_initializeObject)
        {
            if(!_isCorrect)
            {
                //右クリックを押した際にカーソルがイメージの上になければ物体を動かせる状態に変更
                if (Input.GetMouseButtonDown(0) && CheckNoImageOnCurSor())
                {
                    _isMovable = true;
                }//物体を動かせる状態かつ、右クリックを離す時
                else if (_isMovable && Input.GetMouseButtonUp(0))
                {
                    //物体を動かせる状態を解除
                    _isMovable = false;
                    //正誤判定
                    if (CheckCorrect())
                    {
                        _isCorrect = true;
                        //音を鳴らす
                        SEAudioManager.Instance.PlayClipSE(0);
                        //正解への回転開始
                        await AnswerRotation();
                        //出題中のオブジェクトなら動作を止める
                        if (_questionSceneManager)
                        {
                            _initializeObject = false;
                        }
                    }
                }
            }
            else
            {//クリア時に右クリックがされたら回転の演出を瞬時に完了させる
                if(Input.GetMouseButtonDown(0))
                {
                    KillAnswerRotation();
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (_isMovable && !_isCorrect)
        {
            //マウスの移動量を元に物体を回転
            float mouseXMove = Input.GetAxis("Mouse X");
            float mouseYMove = Input.GetAxis("Mouse Y");
            _rotationObject.transform.Rotate(mouseYMove * _basicSensivility * _sensivilityIndex / 5, -1 * mouseXMove * _basicSensivility * _sensivilityIndex / 5, 0f, Space.World);
        }
    }
    /// <summary>
    /// カーソルがイメージの上あるか判定
    /// </summary>
    /// <returns>イメージの上になければtrue, そうでなければfalse</returns>
    bool CheckNoImageOnCurSor()
    {
        //RaycastAllの引数
        PointerEventData pointData = new PointerEventData(EventSystem.current);
        //RaycastAllの結果格納用List
        List<RaycastResult> RayResult = new List<RaycastResult>();
        //PointerEventDataにマウスの位置をセット
        pointData.position = Input.mousePosition;
        //RayCast（スクリーン座標）
        EventSystem.current.RaycastAll(pointData, RayResult);
        return RayResult.Count == 0;
    }
    /// <summary>
    /// 正誤判定　
    /// </summary>
    /// <returns>正解ならtrue, そうでなければfalse</returns>
    bool CheckCorrect()
    {
        if (Quaternion.Angle(_answerRotation, _rotationObject.transform.rotation) <= _answerVectorRange)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 設定された正解のQuaternionに物体を回転させる
    /// </summary>
    async Task AnswerRotation()
    {
        Debug.Log("start answer rotation");
        await _rotationObject.transform.DORotate(_answerRotation.eulerAngles, _timeRotation).SetEase(_easeRotation).AsyncWaitForCompletion();
        if(!_questionSceneManager)
        {
            _isCorrect = false;
        }
        Debug.Log("finish answer rotation");
        return;
    }
    void KillAnswerRotation()
    {
        _rotationObject.transform.DOComplete();
        Debug.Log("do completed answer rotation");
    }
    /// <summary>
    /// 基礎回転量の倍率を変更する / 0.2-2.0倍
    /// </summary>
    /// <param name="value">1-10</param>
    public void ChangeSensivility(float value)
    {
        _sensivilityIndex = value;
    }
}
