using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandinAvatAR
{
    public class UIActivater : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvas;
        [SerializeField] float doubleTouchTime = 0.5f;

        [SerializeField] float uiWaitTime = 3;
        [SerializeField] float fadeOutTime = 1;

        float lastTouch;
        Coroutine fadeOutLoop;

        int prevTouchCount = 0;

        private void Start()
        {
            canvas.gameObject.SetActive(false);
        }

        private void Update()
        {
            if((Input.touchCount == 0 && prevTouchCount > 0)|| Input.GetMouseButtonUp(0))
            {

                if(Time.time - lastTouch < doubleTouchTime)
                {
                    CanvasActivate();
                }

                lastTouch = Time.time;
            }
            prevTouchCount = Input.touchCount;
        }

        private void CanvasActivate()
        {
            if(fadeOutLoop != null)
            {
                StopCoroutine(fadeOutLoop);
            }

            canvas.gameObject.SetActive(true);
            canvas.alpha = 1;
            fadeOutLoop = StartCoroutine(CanvasFadeOut());
        }

        private IEnumerator CanvasFadeOut()
        {
            yield return new WaitForSecondsRealtime(uiWaitTime);

            while (canvas.alpha > 0.01f)
            {
                canvas.alpha = canvas.alpha - Time.deltaTime / fadeOutTime;
                yield return null;
            }

            canvas.gameObject.SetActive(false);
        }
    }
}