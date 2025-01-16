using Kibo.NPCs.Behaviour;
using UnityEngine;

namespace Kibo.NPCs
{
    public class NPC : MonoBehaviour
    {
        [Tooltip("Optional")]
        [SerializeField] private IdleBehaviour idleBehaviour;
        [Tooltip("Optional")]
        [SerializeField] private LookAtPlayerBehaviour lookAtPlayerBehaviour;
        [Header("Mesh Bones")]
        [Tooltip("Optional")]
        [SerializeField] private Transform head;

        #region Unity Messages
        private void Start()
        {
            if (lookAtPlayerBehaviour)
            {
                lookAtPlayerBehaviour.PlayerApproachedEvent.AddListener(OnPlayerApproached);
                lookAtPlayerBehaviour.PlayerDepartedEvent.AddListener(OnPlayerDeparted);
            }
        }

        private void OnEnable()
        {
            if ((lookAtPlayerBehaviour == null || lookAtPlayerBehaviour.Player == null) && idleBehaviour) idleBehaviour.enabled = true;
            if (lookAtPlayerBehaviour) lookAtPlayerBehaviour.enabled = true;
        }

        private void OnDisable()
        {
            if (idleBehaviour) idleBehaviour.enabled = false;
            if (lookAtPlayerBehaviour) lookAtPlayerBehaviour.enabled = false;
        }

        private void Update()
        {
            FaceTarget();
        }
        #endregion

        #region Player Event Listeners
        private void OnPlayerApproached()
        {
            if (idleBehaviour) idleBehaviour.enabled = false;
        }

        private void OnPlayerDeparted()
        {
            if (lookAtPlayerBehaviour.Player) return;

            if (idleBehaviour) idleBehaviour.enabled = true;
        } 
        #endregion

        private void FaceTarget()
        {
            Vector3 target;
            if (lookAtPlayerBehaviour && lookAtPlayerBehaviour.Player) target = lookAtPlayerBehaviour.Player.position;
            else if (idleBehaviour && idleBehaviour.HasTarget) target = idleBehaviour.TargetPosition.Value;
            else return;

            Vector3 bodyForward = Vector3.ProjectOnPlane(target - transform.position, transform.up);
            if (bodyForward == Vector3.zero) return;

            transform.forward = bodyForward;
            if (head)
            {
                bool lookDirectly = idleBehaviour && idleBehaviour.enabled && idleBehaviour.TargetStation;
                head.forward = lookDirectly ? target - head.position : bodyForward;
            }
        }
    }
}