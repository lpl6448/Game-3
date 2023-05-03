using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFrame
{
    //ID number used to get dialogue frame
    private int ID;

    //Fields relevant to rendering dialogue
    private Characters actor;
    private string speaker;
    private Emotions emotion;
    //private Animation charAnimation <-Where an animation request for dialogue would be
    private string line;
    private string response1;
    private string response2;
    private LineType lineType;

    //Holds the ID of next frames of dialogue that correspond to dialogue options
    private int nextFrame1;
    private int nextFrame2;

    //Properties to access frame fields
    public int _ID => ID;
    public Characters Actor => actor;
    public string Speaker => speaker;
    public Emotions Emotion => emotion;
    public string Line => line;
    public string Response1 => response1;
    public string Response2 => response2;
    public LineType LineType => lineType;
    public int NextFrame1 => nextFrame1;
    public int NextFrame2 => nextFrame2;

    public DialogueFrame(int a_ID, string a_speaker, string a_emotion, string a_line, string a_lineType,
                         int a_nextFrame1, int a_nextFrame2=0, string a_response1="", string a_response2="")
    {
        //Define all required argument fields
        ID = a_ID;
        speaker = a_speaker;
        actor = CharacterFromSpeaker(a_speaker);
        emotion = EmotionFromString(a_emotion);
        line = a_line;
        lineType = LineTypeFromString(a_lineType);
        nextFrame1 = a_nextFrame1;
        nextFrame2 = a_nextFrame2;
        response1 = a_response1;
        response2 = a_response2;
    }

    private Emotions EmotionFromString(string sEmotion)
    {
        switch(sEmotion)
        {
            case "Neutral":
                return Emotions.Neutral;
            case "Happy":
                return Emotions.Happy;
            case "Annoyed":
                return Emotions.Annoyed;
            case "Wink":
                return Emotions.Wink;
            case "Angry":
                return Emotions.Angry;
            case "Avoidant":
                return Emotions.Avoidant;
            case "Smile_Neutral":
                return Emotions.Smile_Neutral ;
            case "Smile_Angry":
                return Emotions.Smile_Angry;
            case "Smile_Sleep":
                return Emotions.Smile_Sleep;
            case "Smile_Wink":
                return Emotions.Smile_Wink;
            case "Smile_Interested":
                return Emotions.Smile_Interested;
            case "Frown_Neutral":
                return Emotions.Frown_Neutral;
            case "Frown_Angry":
                return Emotions.Frown_Angry;
            case "Frown_Sleep":
                return Emotions.Frown_Sleep;
            case "Frown_Wink":
                return Emotions.Frown_Wink;
            case "Frown_Interested":
                return Emotions.Frown_Interested;
            default:
                return Emotions.Neutral;
        }
    }

    private LineType LineTypeFromString(string sLine)
    {
        switch(sLine)
        {
            case "Continuous":
                return LineType.Continuous;
            case "Respondable":
                return LineType.Respondable;
            case "AskToGolf":
                return LineType.AskToGolf;
            case "ToGolf":
                return LineType.ToGolf;
            case "WonGolf":
                return LineType.WonGolf;
            case "LostGolf":
                return LineType.LostGolf;
            case "HasLost":
                return LineType.HasLost;
            case "FinishIntro":
                return LineType.FinishIntro;
            case "FinishConclusion":
                return LineType.FinishConclusion;
            case "Conclusion":
                return LineType.Conclusion;
            default:
                return LineType.Continuous;
        }
    }

    private Characters CharacterFromSpeaker(string sSpeaker)
    {
        switch(sSpeaker)
        {
            case "Molly":
                return Characters.Molly;
            case "Marcone":
                return Characters.Marcone;
            case "Lacuna":
            case "Toot-Toot":
                return Characters.LC;
            case "Dresden":
                return Characters.Dresden;
            default:
                return Characters.NONE;
        }
    }
}
