using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLoader : MonoBehaviour
{
    [SerializeField] private TextAsset introDialogueJSON;
    [SerializeField] private TextAsset mollyDialogueJSON;
    [SerializeField] private TextAsset marconeDialogueJSON;
    [SerializeField] private TextAsset lcDialogueJSON;

    [System.Serializable]
    public class MockFrame
    {
        //ID number used to get dialogue frame
        public int ID;

        //Fields relevant to rendering dialogue
        public string speaker;
        public string emotion;
        //private string charAnimation <-Where an animation request for dialogue would be
        public string line;
        public string response1;
        public string response2;
        public string lineType;

        //Holds the ID of next frames of dialogue that correspond to dialogue options
        public int nextFrame1;
        public int nextFrame2;
    }

    [System.Serializable]
    public class FrameList
    {
        public List<MockFrame> list;
    }

    private FrameList introList = new FrameList();
    private FrameList mollyList = new FrameList();
    private FrameList marconeList = new FrameList();
    private FrameList lcList = new FrameList();
    

    // Start is called before the first frame update
    void Start()
    {
        introList = JsonUtility.FromJson<FrameList>(introDialogueJSON.text);
        mollyList = JsonUtility.FromJson<FrameList>(mollyDialogueJSON.text);
        marconeList = JsonUtility.FromJson<FrameList>(marconeDialogueJSON.text);
        lcList = JsonUtility.FromJson<FrameList>(lcDialogueJSON.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
