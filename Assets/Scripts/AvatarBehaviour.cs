using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandinAvatAR
{
    public class AvatarBehaviour : MonoBehaviour
    {
        [SerializeField] private float maxRadius = 0.5f;
        [SerializeField] private float armLength = 0.5f;
        [SerializeField, Range(0, 1)] private float relaxAreaRate = 0.7f;
        [SerializeField] private float handChangeAngle = 20;
        [SerializeField] private Transform spot;

        [Header("debug")]
        [SerializeField] private float distance;
        [SerializeField] private float angle;
        private Camera _camera;
        private AvatarLocomotion locomotion;
        private AvatarHandsControl hands;
        private PositionPredictor spotPredictor;

        Vector3 cameraPlanePos
        {
            get {
                if (_camera == null)
                    _camera = Camera.main;
                return new Vector3(_camera.transform.position.x, 0, _camera.transform.position.z); 
            }
        }
        Vector3 avatarPlanePos
        {
            get { return new Vector3(transform.position.x, 0, transform.position.z); }
        }

        float facingAngle
        {
            get { return Vector3.SignedAngle(new Vector3(transform.forward.x, 0, transform.forward.z), cameraPlanePos-avatarPlanePos, Vector3.up); 
            }
        }


        void Start()
        {
            locomotion = GetComponent<AvatarLocomotion>();
            hands = GetComponent<AvatarHandsControl>();
            spotPredictor = new PositionPredictor(spot);
        }


        void Update()
        {
            var spotPredict = spotPredictor.Update();
            var locateDistance = (avatarPlanePos - spotPredict).magnitude;
            LocomotionSwitch(locateDistance);

            var distance = (avatarPlanePos - cameraPlanePos).magnitude;
            HandsSwitch(locateDistance);

            // debug
            this.distance = locateDistance;
            this.angle = this.facingAngle;

        }

        private void LocomotionSwitch(float distance)
        {
            locomotion.Neutral();
            if (distance > maxRadius * relaxAreaRate)
            {
                locomotion.GetCloseTo(new Vector3(spotPredictor.Predict.x, transform.position.y, spotPredictor.Predict.z));
            }
            locomotion.Turn(facingAngle);
            
        }
        private void HandsSwitch(float distance)
        {
            if(distance > armLength)
            {
                if(hands.Hand != AvatarHandsControl.HandType.free)
                    hands.Free();
                return;
            }

            if(hands.Hand == AvatarHandsControl.HandType.free)
            {
                if(facingAngle >= 0)
                {
                    hands.HoldRight();
                    return;
                }
                else
                {
                    hands.HoldLeft();
                    return;
                }
            }
            else if (hands.Hand == AvatarHandsControl.HandType.right)
            {
                if(facingAngle <= -handChangeAngle)
                {
                    hands.HoldLeft();
                    return;
                }
            }
            else if(hands.Hand == AvatarHandsControl.HandType.left)
            {
                if(facingAngle >= handChangeAngle)
                {
                    hands.HoldRight();
                    return;
                }
            }
        }
    }
}