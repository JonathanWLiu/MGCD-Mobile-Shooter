using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour {

    [SerializeField]
    private Animator bodyAnimator;
    [SerializeField]
    private Animator feetAnimator;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool isFeetWalk() {
        return feetAnimator.GetBool("isWalk");
    }
    public void setFeetWalk(bool b)
    {
        feetAnimator.SetBool("isWalk", b);
    }
    public bool isFeetRun()
    {
        return feetAnimator.GetBool("isRun");
    }
    public void setFeetRun(bool b)
    {
        feetAnimator.SetBool("isRun", b);
    }

    public bool isBodyMove()
    {
        return bodyAnimator.GetBool("isMove");
    }
    public void setBodyMove(bool b)
    {
        bodyAnimator.SetBool("isMove", b);
    }

    public void TriggerShoot()
    {
        bodyAnimator.SetTrigger("isShoot");
    }
    public void TriggerReload()
    {
        bodyAnimator.SetTrigger("isReload");
    }
    public void TriggerMelee()
    {
        bodyAnimator.SetTrigger("isMelee");
    }
}
