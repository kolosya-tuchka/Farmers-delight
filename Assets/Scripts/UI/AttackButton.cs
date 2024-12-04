using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Player player;
    WeaponControllerMobile controls;

    void Start()
    {
        player = FindObjectOfType<Player>();
        controls = player.GetComponent<WeaponControllerMobile>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        controls.StartCoroutine(controls.Attack());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        controls.StopAllCoroutines();
    }
}
