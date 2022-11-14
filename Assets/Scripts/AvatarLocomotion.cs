using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandinAvatAR
{
    public class AvatarLocomotion : MonoBehaviour
    {
        [SerializeField] private float maxWalkAngle = 90;
        [SerializeField] private float angleMaxSpeed = 180;
        [SerializeField] private float maxSpeed = 2;
        [SerializeField, Range(0, 1)] private float smoothingRate = 0.2f;


        [Header("debug")]
        [SerializeField]private bool moveFlag;
        [SerializeField]private bool standingTurn;
        [SerializeField]private float angle;

        private Animator animator;
        private Vector3 moveDir;
        private Vector3 prevMoveDir = Vector3.zero;

        private readonly string walkParam = "walk";
        private readonly string angleParam = "angle";

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// ílÇÃÉäÉZÉbÉg
        /// </summary>
        public void Neutral()
        {
            this.moveFlag = false;
            this.standingTurn = false;
            this.angle = 0;
            this.moveDir = Vector3.zero;
        }

        /// <summary>
        /// ó^Ç¶ÇÁÇÍÇΩgoalÇ÷ãﬂÇ√Ç≠
        /// </summary>
        /// <param name="goal"></param>
        public void GetCloseTo(Vector3 goal)
        {
            this.moveFlag = true;
            moveDir = (goal - transform.position).normalized;
        }

        /// <summary>
        /// ó^Ç¶ÇÁÇÍÇΩäpìxÇ…âÒì]ÅBÇªÇÃèÍâÒì]Ç©à⁄ìÆÇµÇ»Ç™ÇÁÇ©ÇÕégÇ§ë§ÇÕà”éØÇµÇ»Ç¢
        /// </summary>
        /// <param name="angleDeg"></param>
        public void Turn(float angleDeg)
        {
            this.angle = angleDeg;
            if (Mathf.Abs(angleDeg) > maxWalkAngle)
                this.standingTurn = true;
        }

        void Update()
        {
            if (this.standingTurn)
            {
                animator.SetBool(walkParam, false);

                animator.SetFloat(angleParam, this.angle);

                transform.RotateAround(transform.position, Vector3.up, Mathf.MoveTowardsAngle(0, this.angle, this.angleMaxSpeed * Time.deltaTime));
            }
            else
            {
                animator.SetBool(walkParam, moveFlag);
                animator.SetFloat(angleParam, this.angle);

                transform.RotateAround(transform.position, Vector3.up, Mathf.MoveTowardsAngle(0, this.angle, this.angleMaxSpeed * Time.deltaTime));

                var moveVec = moveDir * maxSpeed * Time.deltaTime;
                transform.Translate(SpeedFade(moveVec), Space.World);
            }
        }

        private Vector3 SpeedFade(Vector3 src)
        {
            prevMoveDir = (1 - smoothingRate) * src + smoothingRate * prevMoveDir;
            return prevMoveDir;
        }
    }
}