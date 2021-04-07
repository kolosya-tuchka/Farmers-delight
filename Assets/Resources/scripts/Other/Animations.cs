using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Animations : MonoBehaviour
{
    //public SpriteRenderer renderer;
    //public Color hitColor = new Color(.8f, 0, 0, 1), defaultColor = new Color(1, 1, 1, 1);
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public IEnumerator Hit()
    {
        //while (renderer.color != hitColor)
        //{
        //    renderer.color = Color.Lerp(defaultColor, hitColor, 1);
        //    yield return null;
        //}
        //renderer.color = hitColor;

        yield return new WaitForSeconds(.2f);

        //while (renderer.color != defaultColor)
        //{
        //    renderer.color = Color.Lerp(hitColor, defaultColor, 1);
        //    yield return null;
        //}
        //renderer.color = defaultColor;

    }
}
