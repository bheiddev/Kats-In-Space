using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Typewriter : MonoBehaviour
{
    public TMP_Text TextComponent;

    private static Typewriter       instance;
    private List<TypewriterMessage> messages = new List<TypewriterMessage>();

    private TypewriterMessage       currentMsg = null;
    private int                     msgIndex = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (messages.Count > 0 && currentMsg != null)
        {
            currentMsg.Update();
            TextComponent.text = currentMsg.GetMsg();
        }
    }

    public static void Add(string msg, Action callback = null)
    {
        TypewriterMessage typeMsg = new TypewriterMessage(msg, callback);
        instance.messages.Add(typeMsg);
    }

    public static void Activate()
    {
        instance.currentMsg = instance.messages[0];
    }

    public void WriteNextMessageInQueue()
    {
        //if active, show the entire string
        if (currentMsg != null && currentMsg.IsActive())
        {
            TextComponent.text = currentMsg.GetFullMsgAndCallback();
            currentMsg = null;
            return;
        }

        msgIndex++;

        if (msgIndex >= messages.Count)
        {
            currentMsg = null;
            TextComponent.text = "";
            return;
        }

        currentMsg = messages[msgIndex];
    }
}

[System.Serializable]
public class TypewriterMessage
{
    private float   timer = 0;
    private int     charIndex = 0;
    private float   timePerChar = 0.05f;

    [SerializeField]
    public string   currentMsg = null;
    private string  displayMsg = null;

    private Action  onActionCallback = null;

    public TypewriterMessage(string msg, Action callback = null)
    {
        onActionCallback = callback;
        currentMsg = msg;
    }

    public void Callback()
    {
        if (onActionCallback != null) onActionCallback();
    }

    public string GetFullMsgAndCallback()
    {
        if (onActionCallback !=null) onActionCallback();

        return currentMsg;
    }

    public string GetFullMsg()
    {
        return currentMsg;
    }

    public string GetMsg()
    {
        return displayMsg;
    }

    public void Update()
    {
        if (string.IsNullOrEmpty(currentMsg))
            return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            //keep revealing more of the message
            timer += timePerChar;
            charIndex++;

            //assign the message to the TypeWriter
            displayMsg = currentMsg.Substring(0, charIndex);
            displayMsg += "<color=#00000000>" + currentMsg.Substring(charIndex) + "</color>";

            //if we have reached the end of the sentence, stop typing
            if (charIndex >= currentMsg.Length)
            {
                Callback();
                currentMsg = null;
            }
        }
    }

    public bool IsActive()
    {
        if (string.IsNullOrEmpty(currentMsg))
            return false;

        return charIndex < currentMsg.Length;
    }
}
