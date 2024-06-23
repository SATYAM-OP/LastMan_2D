using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] private DynamicJoystick joystick;

    private Vector2 movement;
    private Animator PlayerAnim;
    public Animator WeaponAnim;

    private void Start()
    {
        PlayerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Input
        movement = joystick.Direction.normalized;


        bool isIdle = movement.x == 0 && movement.y == 0;

        if (isIdle)
        {
            WeaponAnim.SetBool("isMoving", false);
            PlayerAnim.SetBool("isMoving", false);
        }
        else
        {
            //weapon
            WeaponAnim.SetFloat("HorizontalMovement", movement.x);

            WeaponAnim.SetBool("isMoving", true);
            WeaponAnim.SetFloat("VerticalMovement", movement.y);
            //player
            PlayerAnim.SetFloat("HorizontalMovement", movement.x);

            PlayerAnim.SetBool("isMoving", true);
            PlayerAnim.SetFloat("VerticalMovement", movement.y);
        }

      /*  Debug.Log(PlayerAnim.GetFloat("HorizontalMovement")+ " v " +PlayerAnim.GetFloat("VerticalMovement"));

        if (movement.x >=-0.5f && movement.x <=-0.3 && movement.y >=-0.3f && movement.y <=0)
        {
            Debug.Log(movement.x);
        }*/
    }

}
