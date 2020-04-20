using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    public GameObject dialogue;

    public Text dialogtxt;
    public Text dialogname;
    public Image dialogimg;

    public static DialogManager instance;

    public Queue<string> sentences;

    public Dialog curDial;

    public float typingSpeed=0.02f;

    Coroutine corout = null;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        LoadDialogues(curDial);
    }

    public void EndDialogue()
    {
        dialogue.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
             if(sentences.Count==0)
            {
                if(curDial.next!=null)
                {
                    LoadDialogues(curDial.next);
                }else
                {
                    EndDialogue();
                }

                 }else
            {
               string sente= sentences.Dequeue();

                if (corout != null)
                {
                    StopCoroutine(corout);
                }
                corout = StartCoroutine(Type(sente));
            }

            
        }
    }

    // Update is called once per frame
    void LoadDialogues(Dialog dial)
    {
        curDial = dial;
        sentences.Clear();

        foreach (string sentence in dial.sentences)
        {
            sentences.Enqueue(sentence);
        }
        dialogue.SetActive(true);
        dialogname.text = dial.name;
        dialogimg.sprite = dial.sprite;
        if(corout!=null)
        {
            StopCoroutine(corout);
        }
        
        corout = StartCoroutine(Type(sentences.Dequeue()));
    }

   

    IEnumerator Type(string sent)
    {
        dialogtxt.text = "";

        foreach (char c in sent.ToCharArray())
        {
            dialogtxt.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
