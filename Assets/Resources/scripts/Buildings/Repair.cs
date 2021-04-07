using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Repair : MonoBehaviour
{
    public float repairTime, repairProgress;
    public int recovery;
    [SerializeField] bool canRepair;
    Image image;
    DefObj obj;
    void Start()
    {
        obj = GetComponentInParent<DefObj>();
        image = transform.parent.Find("HP").Find("RepairImage").GetComponent<Image>();
        repairProgress = 0;
    }

    
    void Update()
    {
        if (canRepair && Input.GetKey(KeyCode.F))
        {
            repairProgress += Time.deltaTime;
            if (Mathf.Abs(repairProgress - repairTime) <= Time.deltaTime)
            {
                if (obj.hp.maxHP - obj.hp.healPoints <= recovery) obj.hp.healPoints = obj.hp.maxHP;
                else obj.hp.healPoints += recovery;
                repairProgress = 0;
            }
        }
        else repairProgress = 0;
        image.fillAmount = repairProgress / repairTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) canRepair = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) canRepair = false;
    }
}
