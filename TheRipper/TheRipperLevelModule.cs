using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace TheRipper
{
    public class TheRipperLevelModule : LevelModule
    {
        public bool triggerRequired = false;

        public override IEnumerator OnLoadCoroutine()
        {
            EventManager.onCreatureSpawn += EventManagerOnonCreatureSpawn;
            return base.OnLoadCoroutine();
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
                if (ragdollPart.gameObject.GetComponent<FragileRagdollPart>() == null)
                {
                    UnityEngine.Object.Destroy(ragdollPart.gameObject.GetComponent<FragileRagdollPart>());
                }
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
                if (ragdollPart.gameObject.GetComponent<FragileRagdollPart>() == null)
                {
                    var fragileRagdollPart = ragdollPart.gameObject.AddComponent<FragileRagdollPart>();
                    fragileRagdollPart.Init(PlayerControl.GetHand(ragdollhand.side), triggerRequired);
                }
            }
            catch (Exception exception)
            {
                Debug.Log(exception.Message);
            }
        }
    }
}