using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public SpriteRenderer mySpriteRenderer;
    public AnimationData baseAnimation;
    Coroutine previousAnimation;

    public void PlayAnimation(AnimationData data)
    {
        //stop previous animation
        if (previousAnimation != null)
        {
            StopCoroutine(previousAnimation);
        }
        previousAnimation = StartCoroutine(PlayAnimationCoroutine(data));
    }

    public IEnumerator PlayAnimationCoroutine(AnimationData data)
    {
        if (data == null)
        {
            data = baseAnimation;
        }
        
        int spritesAmount = data.sprites.Length;
        int i = 0;
        float waitTime = data.framesOfGap * AnimationData.targetFrameTime;
        
        while(i < spritesAmount)
        {
            mySpriteRenderer.sprite = data.sprites[i++];
            yield return new WaitForSeconds(waitTime);

            if(data.loop && i >=spritesAmount)
            {
                i = 0;
            }
        }

        yield return null;
    }
}
