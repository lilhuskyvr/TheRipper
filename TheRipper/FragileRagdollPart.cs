using ThunderRoad;
using UnityEngine;

namespace TheRipper
{
    public class FragileRagdollPart : MonoBehaviour
    {
        private RagdollPart _ragdollPart;
        private bool _lastIsPressing;
        private PlayerControl.Hand _playerControlHand;
        private bool _initialized;

        private bool _triggerRequired;

        //only to use without trigger
        private bool _isWeakened;

        private void Start()
        {
            _ragdollPart = GetComponent<RagdollPart>();
        }

        public void Init(PlayerControl.Hand playerControlHand, bool triggerRequired)
        {
            _playerControlHand = playerControlHand;
            _triggerRequired = triggerRequired;

            if (!triggerRequired)
            {
                WeakenRagdollPart();
            }

            _initialized = true;
        }

        private void WeakenRagdollPart()
        {
            if (!_ragdollPart.name.Contains("Spine"))
                _ragdollPart.characterJoint.breakForce = 1500;
            _isWeakened = true;
        }

        private void Update()
        {
            if (_ragdollPart == null)
                return;
            if (_ragdollPart.isSliced)
                Destroy(this);
            if (_triggerRequired)
            {
                if (!_initialized)
                    return;
                if (_playerControlHand.usePressed && !_lastIsPressing)
                {
                    WeakenRagdollPart();
                }
                else if (!_playerControlHand.usePressed && _lastIsPressing)
                {
                    _ragdollPart.ResetCharJointBreakForce();
                }

                _lastIsPressing = _playerControlHand.usePressed;
            }
            else
            {
                if (_isWeakened)
                    return;
                WeakenRagdollPart();
            }
        }

        private void OnDestroy()
        {
            _ragdollPart.ResetCharJointBreakForce();
        }
    }
}