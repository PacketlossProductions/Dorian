using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorController : MonoBehaviour
{
    Animator animator;
    Collider2D collider;
    public List<GameObject> switches = new List<GameObject>();
    public bool inverted = false;

    private bool isOpen;
    public bool IsOpen { get => isOpen; set { isOpen = value; animator.SetBool("IsOpen", value); } }

    void Awake()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        bool unlocked = true;
        ButtonController button;
        foreach (GameObject gameObject in switches)
        {
            if(gameObject.TryGetComponent<ButtonController>(out button)) {
                if(!button.state)
                {
                    unlocked = false;
                    break;
                }
            }
        }
        unlocked = unlocked ^ inverted;
        IsOpen = unlocked;
        collider.enabled = !IsOpen;
    }
}
