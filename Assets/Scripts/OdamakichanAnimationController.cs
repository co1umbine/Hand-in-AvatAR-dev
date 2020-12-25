using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace HandinAvatAR
{
    public class OdamakichanAnimationController : MonoBehaviour
    {
        [SerializeField] float maxDistance = 0.5f;
        [SerializeField] float minDistance = 0.2f;

        [SerializeField] Animator animator;

        [SerializeField] Transform rightTarget;
        [SerializeField] Transform leftTarget;

        [SerializeField] TwoBoneIKConstraint rightIK;
        [SerializeField] TwoBoneIKConstraint leftIK;

        [SerializeField] Transform _camera;

        [SerializeField] bool isRight = true;

        [SerializeField] Vector3 offset;

        [SerializeField] float speed = 0.5f;

        Vector3 cameraPlanePos
        {
            get { return new Vector3(_camera.transform.position.x, 0, _camera.transform.position.z); }
        }
        Vector3 odamakichanPlanePos
        {
            get { return new Vector3(transform.position.x, 0, transform.position.z); }
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.touchCount == 1 || Input.GetMouseButton(0))
            {
                AddjustPosition();
                IKTargetUpdate();
            }

        }

        private void IKTargetUpdate()
        {
            if (isRight)
            {
                rightIK.weight = 1;
                leftIK.weight = 0;

                rightTarget.transform.position = _camera.position + _camera.rotation * offset;
                rightTarget.transform.rotation = _camera.rotation * Quaternion.AngleAxis(90, Vector3.up);
            }
            else
            {
                leftIK.weight = 1;
                rightIK.weight = 0;

                leftTarget.transform.position = _camera.position + _camera.rotation * offset;
                leftTarget.transform.rotation = _camera.rotation * Quaternion.AngleAxis(-90, Vector3.up);
            }
        }

        private void AddjustPosition()
        {
            if (IsAppropriateDistance())
            {
                return;
            }
            var dir = cameraPlanePos - odamakichanPlanePos;

            dir *= dir.magnitude >= maxDistance ? 1 : -1;
            transform.position += dir * speed * Time.deltaTime;
        }

        private bool IsAppropriateDistance()
        {

            var dist = (odamakichanPlanePos - cameraPlanePos).magnitude;
            return minDistance < dist && dist < maxDistance;
        }

        public void SetRightIK(bool isRight)
        {
            this.isRight = isRight;
        }
    }
}