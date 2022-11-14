using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UniRx;

namespace HandinAvatAR
{
    public class AvatarHandsControl : MonoBehaviour
    {
        public enum HandType
        {
            free,
            right,
            left,
        }

        [SerializeField] private TwoBoneIKConstraint rightIK;
        [SerializeField] private TwoBoneIKConstraint leftIK;

        [SerializeField] private Transform rightHandTarget;
        [SerializeField] private Transform leftHandTarget;

        [SerializeField] private Vector3 offset;
        [SerializeField] private float handHoldingTime = 0.5f;

        private Coroutine rightHandLoop;
        private Coroutine leftHandLoop;


        void Start()
        {
            rightIK.weight = 0;
            leftIK.weight = 0;
        }

        public HandType Hand;

        private void Refresh()
        {
            if(rightHandLoop != null)
            {
                StopCoroutine(rightHandLoop);
                rightHandLoop = null;
            }
            if(leftHandLoop != null)
            {
                StopCoroutine(leftHandLoop);
                leftHandLoop = null;
            }
        }

        public void Free()
        {
            Refresh();
            Hand = HandType.free;
            rightHandLoop = StartCoroutine(IKWeightToZero(rightIK));
            leftHandLoop = StartCoroutine(IKWeightToZero(leftIK));
        }

        public void HoldLeft()
        {
            Refresh();
            Hand = HandType.left;
            leftHandLoop = StartCoroutine(IKWeightToOne(leftIK));
            rightHandLoop = StartCoroutine(IKWeightToZero(rightIK));
        }

        public void HoldRight()
        {
            Refresh();
            Hand = HandType.right;
            rightHandLoop = StartCoroutine(IKWeightToOne(rightIK));
            leftHandLoop = StartCoroutine(IKWeightToZero(leftIK));
        }

        private IEnumerator IKWeightToOne(TwoBoneIKConstraint IK)
        {
            float t = handHoldingTime * IK.weight/1;
            while(IK.weight < 1)
            {
                IK.weight = -(Mathf.Cos(t * Mathf.PI / handHoldingTime) - 1) / 2;
                if (t / handHoldingTime > 1) break;
                t += Time.deltaTime;
                yield return null;
            }
            IK.weight = 1;
        }
        private IEnumerator IKWeightToZero(TwoBoneIKConstraint IK)
        {
            float t = handHoldingTime * (1-IK.weight)/1;
            while (IK.weight > 0)
            {
                IK.weight = (Mathf.Cos(t * Mathf.PI / handHoldingTime) + 1) / 2;
                if (t / handHoldingTime > 1) break;
                t += Time.deltaTime;
                yield return null;
            }
            IK.weight = 0;
        }


        void Update()
        {
            var camera = Camera.main.transform;
            rightHandTarget.position = camera.position + camera.rotation* offset;
            rightHandTarget.rotation = camera.rotation * Quaternion.AngleAxis(90, Vector3.up);
            leftHandTarget.position = camera.position + camera.rotation * offset;
            leftHandTarget.rotation = camera.rotation * Quaternion.AngleAxis(-90, Vector3.up);
        }
    }
}