using System;
using UnityEngine;
[Serializable]
public class TutorialText
{
    [SerializeField, TextArea(1,2)] string _textData;
    public string StringData => _textData;
}
