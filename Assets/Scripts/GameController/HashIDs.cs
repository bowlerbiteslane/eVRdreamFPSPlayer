using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
    // Here we store the hash tags for various strings used in our animators.
    public int dyingState;
    public int locomotionState;
    public int shoutState;
    public int deadBool;    


    void Awake()
    {
        //Animator States
        dyingState = Animator.StringToHash("Base Layer.Dying");
        locomotionState = Animator.StringToHash("Base Layer.Locomotion");
        shoutState = Animator.StringToHash("Shouting.Shout");
        
        //Animator Parameters
        deadBool = Animator.StringToHash("Dead");
    }
}