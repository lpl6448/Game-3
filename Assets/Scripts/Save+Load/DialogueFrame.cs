using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFrame
{
    //ID number used to get dialogue frame
    private int ID;

    //Fields relevant to rendering dialogue
    private Characters speaker;
    private Emotions emotion;
    //private Animation charAnimation <-Where an animation request for dialogue would be
    private string line;
    private string response1;
    private string response2;
    private LineType lineType;

    //Holds the ID of next frames of dialogue that correspond to dialogue options
    private int nextFrame1;
    private int nextFrame2;

    public DialogueFrame(int a_ID, Characters a_speaker, Emotions a_emotion, string a_line, LineType a_lineType,
                         int a_nextFrame1, int a_nextFrame2=0, string a_response1="", string a_response2="")
    {
        //Define all required argument fields
        ID = a_ID;
        speaker = a_speaker;
        emotion = a_emotion;
        line = a_line;
        lineType = a_lineType;
        nextFrame1 = a_nextFrame1;

        //Assigned nextFrame2 the same as 1 if the dialogue option isn't unique
        if(a_nextFrame2 == -1)
            nextFrame2 = a_nextFrame1;
        //Fill response text fields as long as line type allows and neither response is empty
        if(lineType!=LineType.Continuous && (a_response1!=""&&a_response2!=""))
        {
            response1 = a_response1;
            response2 = a_response2;
        }
    }
}
