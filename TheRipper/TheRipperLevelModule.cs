using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace TheRipper
{
    public class TheRipperLevelModule : LevelModule
    {
        public override IEnumerator OnLoadCoroutine(Level level)
        {
            EventManager.onCreatureSpawn += EventManagerOnonCreatureSpawn;
            return base.OnLoadCoroutine(level);
        }

        private void EventManagerOnonCreatureSpawn(Creature creature)
        {
            foreach (var part in creature.ragdoll.parts)
            {
                foreach (var handle in part.handles)
                {
                    handle.Grabbed += HandleOnGrabbed;
                    handle.UnGrabbed += HandleOnUnGrabbed;
                }
            }
        }

        private void HandleOnUnGrabbed(RagdollHand ragdollhand, Handle handle, EventTime eventtime)
        {
            try
            {
                var ragdollPart = handle.GetComponentInParent<RagdollPart>();
                ragdollPart.ResetCharJointBreakForce();
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
            }
        }

        private void HandleOnGrabbed(RagdollHand ragdollhand, Handle handle, EventTime eventtime)
        {
            try
            {
                var ragdollPart = handle.GetComponentInParent<RagdollPart>();
                if (!ragdollPart.name.Contains("Spine"))
                    ragdollPart.characterJoint.breakForce = 1500;
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
            }
        }
    }
}