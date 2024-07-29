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

    [SerializeField, Tooltip("�I�u�W�F�N�g�̉�]���x�̊�b�l�i0.2-2.0�{�ŕς��j")] float _basicSensivility = 1.5f;
    [SerializeField, Tooltip("sensivilityIndex��������X���C�_�[")] Slider _sensivilitySlider;
    float _sensivilityIndex = 0;
    public float SensivilityIndex => _sensivilityIndex;

    [SerializeField] GameObject _initialAnswerObject;
    [SerializeField] GameObject _initialRotationObject;

    GameObject _rotationObject;
    Quaternion _answerRotation;

    [SerializeField, Tooltip("���딻��̃I�u�W�F�N�g�̋��e�p�x")] private float _answerVectorRange = 15f;
    [SerializeField, Tooltip("�������́A���̂���]������܂ł̎���")] float _timeRotation = 2f;
    [SerializeField, Tooltip("�������́A���̂̉�]�ʂ̐���")] Ease _easeRotation = Ease.Linear;

    QuestionSceneManager _questionSceneManager;

    bool _isCorrect = false;
    public bool IsCorrect => _isCorrect;
    bool _initializeObject = false;
    public bool InitializeObject => _initializeObject;
    bool _isMovable = false;

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
    }
    private void Start()
    {
        //�Z�[�u�f�[�^����X���C�_�[�Ɛݒ��������
        _sensivilityIndex = SaveManager.Instance.GetSaveData._sensitivityIndex;
        _sensivilitySlider.value = _sensivilityIndex;
        //�^�C�g���V�[���ɍ��킹���I�u�W�F�N�g�̏�����
        Initialize(_initialAnswerObject.transform.rotation, _initialRotationObject);
    }
    /// <summary>
    /// �V�[���̕ύX���ɌĂ�ŏ���������
    /// </summary>
    /// <param name="answerRotation">�����̃I�u�W�F�N�g��rotation</param>
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
                //�E�N���b�N���������ۂɃJ�[�\�����C���[�W�̏�ɂȂ���Ε��̂𓮂������ԂɕύX
                if (Input.GetMouseButtonDown(0) && CheckNoImageOnCurSor())
                {
                    _isMovable = true;
                }//���̂𓮂������Ԃ��A�E�N���b�N�𗣂���
                else if (_isMovable && Input.GetMouseButtonUp(0))
                {
                    //���̂𓮂������Ԃ�����
                    _isMovable = false;
                    //���딻��
                    if (CheckCorrect())
                    {
                        _isCorrect = true;
                        //����炷
                        SEAudioManager.Instance.PlayClipSE(0);
                        //�����ւ̉�]�J�n
                        await AnswerRotation();
                        //�o�蒆�̃I�u�W�F�N�g�Ȃ瓮����~�߂�
                        if (_questionSceneManager)
                        {
                            _initializeObject = false;
                        }
                    }
                }
            }
            else
            {//�N���A���ɉE�N���b�N�����ꂽ���]�̉��o���u���Ɋ���������
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
            //�}�E�X�̈ړ��ʂ����ɕ��̂���]
            float mouseXMove = Input.GetAxis("Mouse X");
            float mouseYMove = Input.GetAxis("Mouse Y");
            _rotationObject.transform.Rotate(mouseYMove * _basicSensivility * _sensivilityIndex / 5, -1 * mouseXMove * _basicSensivility * _sensivilityIndex / 5, 0f, Space.World);
        }
    }
    /// <summary>
    /// �J�[�\�����C���[�W�̏゠�邩����
    /// </summary>
    /// <returns>�C���[�W�̏�ɂȂ����true, �����łȂ����false</returns>
    bool CheckNoImageOnCurSor()
    {
        //RaycastAll�̈���
        PointerEventData pointData = new PointerEventData(EventSystem.current);
        //RaycastAll�̌��ʊi�[�pList
        List<RaycastResult> RayResult = new List<RaycastResult>();
        //PointerEventData�Ƀ}�E�X�̈ʒu���Z�b�g
        pointData.position = Input.mousePosition;
        //RayCast�i�X�N���[�����W�j
        EventSystem.current.RaycastAll(pointData, RayResult);
        return RayResult.Count == 0;
    }
    /// <summary>
    /// ���딻��@
    /// </summary>
    /// <returns>�����Ȃ�true, �����łȂ����false</returns>
    bool CheckCorrect()
    {
        if (Quaternion.Angle(_answerRotation, _rotationObject.transform.rotation) <= _answerVectorRange)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// �ݒ肳�ꂽ������Quaternion�ɕ��̂���]������
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
    /// ��b��]�ʂ̔{����ύX���� / 0.2-2.0�{
    /// </summary>
    /// <param name="value">1-10</param>
    public void ChangeSensivility(float value)
    {
        _sensivilityIndex = value;
    }
}
