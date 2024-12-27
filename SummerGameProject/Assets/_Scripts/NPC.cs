using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    //Member variables
    public string m_Name = "NULL";
    private int m_Id;


    //TODO - Make class dialogue...
    public Dialogue m_Dialogue;

    public GameObject myManager;
    private DialogueHandler myHandler;
    //Fix
    public bool m_RotFix = false;


    // Start is called before the first frame update
    void Start()
    {
        myHandler = myManager.GetComponent<DialogueHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void interactBegin()
    {
        //Used via interactable class to invoke callback to this function when paired...
        Debug.Log("Interaction began!");


        //Interaction is detected, now we need a couple of things... Player? Dialogue manager? 
        //We could get player using unity just find? perhaps
        GameObject player = GameObject.FindWithTag("Player");
        orientToInterest(player.transform);

        //text.GetComponent<TMP_Text>().SetText(m_Dialogue.sentences[0]);
        //We need to get the players dialogue ui..?

        player.GetComponent<PlayerInput>().SwitchCurrentActionMap("PlayerUI");
        myHandler.SendDialogue(m_Dialogue);

    }

    private void orientToInterest(Transform transform_)
    {
        Vector3 temp = new Vector3(transform_.position.x, this.transform.position.y, transform_.position.z);
        transform.LookAt(temp, Vector3.up);


        //Fix here... For origin issue.
        if (m_RotFix)
        {
            transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x - 90.0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }

}
