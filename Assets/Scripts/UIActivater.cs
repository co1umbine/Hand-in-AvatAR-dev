using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace HandinAvatAR
{
    public class UIActivater : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvas;
        //[SerializeField] InputProvider input;

        [SerializeField] float doubleTouchTime = 0.5f;

        [SerializeField] float uiWaitTime = 3;
        [SerializeField] float fadeOutTime = 1;

        float lastTouch;
        Coroutine fadeOutLoop;

        private void Start()
        {
            canvas.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Touch.activeTouches.Count > 0)
            {

                foreach (var t in Touch.activeTouches)
                {
                    if (t.tapCount == 2)
                    {
                        CanvasActivate();
                        return;
                    }
                }

            }
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