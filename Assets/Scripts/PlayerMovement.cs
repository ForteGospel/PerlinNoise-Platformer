using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    CharacterController2D Controller;
    Animator animator;
    [SerializeField] float runSpeed = 40f;
    float horizontal = 0f;
    bool jump;
    
    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if (Input.GetButton("Jump"))
        {
            jump = true;
            animator.SetBool("Jump", true);
        }
        else
        {
            jump = false;
        }

        if (Input.GetButtonDown("Attack"))
        {
            animator.SetTrigger("Attack");
        }
    }

    public void OnLanding()
    {
        animator.SetBool("Jump", false);
    }

    private void FixedUpdate()
    {

        Controller.Move(horizontal * Time.fixedDeltaTime, false, jump);
    }
}
