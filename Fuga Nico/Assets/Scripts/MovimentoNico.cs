using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoNico : MonoBehaviour
{
    public float correr;
    Vector2 lastClickedPos;
    bool movimento;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            lastClickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movimento = true;
        }

        if(movimento && (Vector2)transform.position != lastClickedPos)
        {
            float step = correr * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, lastClickedPos, step);
        }

        else
        {
            movimento = false;
        }
    }
}
