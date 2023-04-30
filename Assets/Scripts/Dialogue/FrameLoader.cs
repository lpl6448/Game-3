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

    //Lists that will contain all dialogue frames
    private List<DialogueFrame> introDL = new List<DialogueFrame>();
    private List<DialogueFrame> mollyDL = new List<DialogueFrame>();
    private List<DialogueFrame> marconeDL = new List<DialogueFrame>();
    private List<DialogueFrame> lcDL = new List<DialogueFrame>();

    //Properties to access DialogueFrame lists
    public List<DialogueFrame> IntroDL => introDL;
    public List<DialogueFrame> MollyDL => mollyDL;
    public List<DialogueFrame > MarconeDL => marconeDL;
    public List<DialogueFrame> LCDL => lcDL;

    /// <summary>
    /// MockFrame takes in the raw json and turns the data into a primitive dialogue frame object
    /// Will be used to create actual dialogue frames
    /// </summary>
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
    private FrameList ltList = new FrameList();
    

    /// <summary>
    /// Returns a list of dialgue frames based on what frame list is requested
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public List<DialogueFrame> LoadDialogue(DLists list)
    {
        //Fill selected list with values pulled from JSON files and return the handled info as a list of Dialogue Frames
        switch(list)
        {
            case DLists.Intro:
                introList = JsonUtility.FromJson<FrameList>(introDialogueJSON.text);
                foreach (MockFrame mf in introList.list)
                {
                    introDL.Add(new DialogueFrame(mf.ID, mf.speaker, mf.emotion, mf.line, mf.lineType, mf.nextFrame1, mf.nextFrame2));
                }
                return introDL;
            case DLists.Molly:
                mollyList = JsonUtility.FromJson<FrameList>(mollyDialogueJSON.text);
                foreach (MockFrame mf in mollyList.list)
                {
                    mollyDL.Add(new DialogueFrame(mf.ID, mf.speaker, mf.emotion, mf.line, mf.lineType, mf.nextFrame1, mf.nextFrame2));
                }
                return mollyDL;
            case DLists.Marcone:
                marconeList = JsonUtility.FromJson<FrameList>(marconeDialogueJSON.text);
                foreach (MockFrame mf in marconeList.list)
                {
                    marconeDL.Add(new DialogueFrame(mf.ID, mf.speaker, mf.emotion, mf.line, mf.lineType, mf.nextFrame1, mf.nextFrame2));
                }
                return marconeDL;
            case DLists.LT:
                ltList = JsonUtility.FromJson<FrameList>(lcDialogueJSON.text);
                foreach (MockFrame mf in ltList.list)
                {
                    lcDL.Add(new DialogueFrame(mf.ID, mf.speaker, mf.emotion, mf.line, mf.lineType, mf.nextFrame1, mf.nextFrame2));
                }
                return lcDL;
            default: return new List<DialogueFrame>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
