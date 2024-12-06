using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousAnimator : MonoBehaviour
{
    public SpriteRenderer mySpriteRenderer;  // Renderizador do sprite do personagem
    public AnimationData continuousAnimation;  // Animação contínua do personagem
    private Coroutine continuousAnimationRoutine;  // Controle da animação contínua

    private void Start()
    {
        // Inicia a animação contínua assim que o jogo começa
        if (continuousAnimation != null)
        {
            PlayContinuousAnimation();
        }
    }

    public void PlayContinuousAnimation()
    {
        if (continuousAnimationRoutine != null)
        {
            StopCoroutine(continuousAnimationRoutine);
        }

        // Inicia a animação como uma coroutine
        continuousAnimationRoutine = StartCoroutine(PlayAnimationCoroutine(continuousAnimation));
    }

    public void StopAnimation()
    {
        if (continuousAnimationRoutine != null)
        {
            StopCoroutine(continuousAnimationRoutine);
            continuousAnimationRoutine = null;
        }
    }

    private IEnumerator PlayAnimationCoroutine(AnimationData data)
    {
        if (data == null)
        {
            yield break;
        }

        int spritesAmount = data.sprites.Length;
        int i = 0;
        float waitTime = data.framesOfGap * AnimationData.targetFrameTime;

        while (true) // Animação contínua
        {
            mySpriteRenderer.sprite = data.sprites[i++];
            yield return new WaitForSeconds(waitTime);

            if (data.loop && i >= spritesAmount)
            {
                i = 0;
            }
        }
    }
}
