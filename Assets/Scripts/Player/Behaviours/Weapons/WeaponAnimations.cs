using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimations : MonoBehaviour
{
    public virtual IEnumerator RotateWeapon(float angle, float time)
    {
        float curAngle = transform.eulerAngles.z;
        float passedTime = 0;
        float speed = 10 / time * angle;
        while (passedTime < time / 6)
        {
            transform.Rotate(speed * Time.deltaTime * Vector3.forward);
            passedTime += Time.deltaTime;
            yield return null;
        }
        passedTime = 0;
        while (passedTime < time / 6)
        {
            transform.Rotate(2 * speed * Time.deltaTime * Vector3.back);
            passedTime += Time.deltaTime;
            yield return null;
        }
        passedTime = 0;
        while (passedTime < time / 6)
        {
            transform.Rotate(speed * Time.deltaTime * Vector3.forward);
            passedTime += Time.deltaTime;
            yield return null;
        }
        transform.eulerAngles = new Vector3(0, 0, curAngle);
    }
}
