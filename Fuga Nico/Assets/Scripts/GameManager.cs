using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static List<int> collectedItems = new List<int>();
    static float moveSpeed = 3.5f;
    static float moveAccuracy = 0.15f;
    public AnimationData[] playerAnimations;
    int activeLocalScene = 0;

    public IEnumerator MoveToPoint(Transform myObject, Vector2 point)
    {
        //calculate position difference
        Vector2 positionDifference = point - (Vector2)myObject.position;
        //flip object
        if (myObject.GetComponentInChildren<SpriteRenderer>() && positionDifference.x != 0)
        {
            myObject.GetComponentInChildren<SpriteRenderer>().flipX = positionDifference.x > 0;
        }
        //stop when we are near the point
        while (positionDifference.magnitude > moveAccuracy)
        {
            //move in direction frame
            myObject.Translate(moveSpeed * positionDifference.normalized * Time.deltaTime);
            //recalculate position difference
            positionDifference = point - (Vector2)myObject.position;
            yield return null;
        }
        //snap to point
        myObject.position = point;

        //tell ClickManager that the player has arrived
        if (myObject == FindObjectOfType<ClickMove>().player || activeLocalScene == 0)
            FindObjectOfType<ClickMove>().playerWalking = false;
        yield return null;
    }
}
