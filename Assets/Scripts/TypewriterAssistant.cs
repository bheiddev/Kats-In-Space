using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypewriterAssistant : MonoBehaviour
{
    public NarrationBlockSO narration;

    private void Start()
    {
        if (narration != null)
        {
            foreach (string line in narration.lines)
            {
                Typewriter.Add(line);
            }
        }
        else
        {
            //string array example
            Typewriter.Add("Message 1: This is a test to experiment with how much text I can show.");
            Typewriter.Add("Message 2: I love tests. This text may wrap around.");
            Typewriter.Add("Message 3: Thank for watching this tutorial! Cheers.");
        }

        //activate the typewriter
        Typewriter.Activate();
    }
}
