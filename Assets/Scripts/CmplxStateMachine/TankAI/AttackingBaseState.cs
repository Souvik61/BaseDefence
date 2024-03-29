﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace cmplx_statemachine
{
    public class AttackingBaseState : BaseState
    {
        Transform selfTransform;
        TankAIScript3 tankAIScript;
        NewTankScript tankController;

        Collider2D[] colliders;
        UnitComponent selfUnit;

        Vector2 dirToTarget;

        int[] controlBits;

        public AttackingBaseState(CStateMachine sTM,TankAIScript3 tankAI):base(sTM)
        {
            stateName = "ATTK_BASE";
            tankAIScript = tankAI;
            tankController = tankAI.GetComponent<NewTankScript>();
            selfTransform = tankAI.GetComponent<Transform>();
            selfUnit = selfTransform.GetComponent<UnitComponent>();
            controlBits = new int[2];
            colliders = new Collider2D[10];
        }

        public override void OnEnter()
        { 
            tankAIScript.StartCoroutine(MasterCoroutine());
        }

        //------------------------
        //Complex Coroutines
        //------------------------

        IEnumerator MasterCoroutine()
        {
            //Destroy Watchtowers
            //yield return tankAIScript.StartCoroutine(DestroyWatchTowers());
            yield return tankAIScript.StartCoroutine(DestroyNearbyUnits());
            //Destroy Artilery if any
            //Finally Destroy Command Center
            yield return tankAIScript.StartCoroutine(ApproachNearCC());
            yield return tankAIScript.StartCoroutine(DestroyCommCenter());
        }

        /// <summary>
        /// Destroy Watch towers
        /// </summary>
        /// <returns></returns>
        public IEnumerator DestroyWatchTowers()
        {
            List<WatchTowerAIScript> watchTowers = (tankAIScript.targetBase).watchTowers;

            while (true)
            {
                watchTowers = PickAliveWatchTowers(watchTowers);

                if (watchTowers.Count == 0) break;

                var nearest = FindNearestWatchTower(watchTowers);

                dirToTarget = (nearest.transform.position - selfTransform.position).normalized;

                TryFaceTowardsDirection();

                if (IsFacingTarget(nearest.transform))
                {
                    tankController.Shoot();
                }

                yield return null;//Pause here.
            }
        }

        public IEnumerator DestroyNearbyUnits()
        {
            List<GameObject> nearByEnemies;

            while (true)
            {
                nearByEnemies = FindNearbyUnits(3); 

                dirToTarget = (nearByEnemies[0].transform.position - selfTransform.position).normalized;

                TryFaceTowardsDirection();

                if (IsFacingTarget(nearByEnemies[0].transform))
                {
                    tankController.Shoot();
                }

                yield return null;//Pause here.
            }

        }

        public IEnumerator ApproachNearCC()
        {
            Transform a = (tankAIScript.targetBase).nearCCLandingZone;
            float distance = 0;
            do
            { 
                distance = Vector2.Distance(selfTransform.position, a.position);

                FollowWayPoints(a);
                AvoidLocalObstacles();

                //Apply controlls
                tankController.Move(controlBits[0]);
                tankController.Rotate(controlBits[1]);

                yield return null;
            } while (distance > 2);
        }

        IEnumerator DestroyCommCenter()
        {
            Transform ccT = (tankAIScript.targetBase).commandCenter.transform;

            while (ccT.GetComponent<HealthScript>().currentHP > 0)
            {
                dirToTarget = (ccT.position - selfTransform.position).normalized;

                TryFaceTowardsDirection();

                if (IsFacingTarget(ccT))
                {
                    tankController.Shoot();
                }

                yield return null;//Pause here.
            }
        }

        //---------------------------
        //Update functions
        //---------------------------

        void FollowWayPoints(Transform target)
        {
            //Local functions :-O
            bool IsFacingDirection(Vector2 dir, float tolerance)
            {
                float angle = Vector2.Angle(dir, selfTransform.up);
                if (Mathf.Abs(angle) < tolerance)
                {
                    return true;
                }
                return false;
            }

            Vector2 dirT = (target.position - selfTransform.position).normalized;


            if (IsFacingDirection(dirT, 2))
            { controlBits[0] = 1; }

            SetControlBitsTowardsDir(dirT, controlBits);

        }

        void AvoidLocalObstacles()
        {
            SensorArrayScript sensorArray = selfTransform.GetComponentInChildren<SensorArrayScript>();

            SensorArrayScript.CollisionStatus collisionStatus = sensorArray.CheckCollisionArray();

            //Control turn

            if (collisionStatus.LRay)//if lSide hit
            {
                controlBits[1] = -1;
            }

            if (collisionStatus.RRay)//if rSide hit
            {

                controlBits[1] = 1;
            }

            if (collisionStatus.almostBlocked)
            {
                controlBits[1] = 0;
            }

            //Control forward movement

            if (collisionStatus.RRay || collisionStatus.LRay)
            {
                controlBits[0] = 0;
            }

            if (collisionStatus.LARay || collisionStatus.RARay)
            {
                controlBits[0] = 1;
            }

            if (collisionStatus.MRay && (collisionStatus.LRay || collisionStatus.RRay))
            {
                controlBits[0] = 0;
            }

            if (collisionStatus.isFullyBlocked)
            {
                controlBits[0] = 0;
            }

        }

        //-----------------
        //Misc.
        //-----------------

        void SetControlBitsTowardsDir(Vector2 dir, int[] cBits)
        {
            float angle = Vector2.SignedAngle(dir, selfTransform.up);
            if (Mathf.Abs(angle) > 7)
            {
                if (angle > 0)
                { cBits[1] = -1; }
                else { cBits[1] = 1; }
            }
        }

        WatchTowerAIScript FindNearestWatchTower(List<WatchTowerAIScript> list)
        {
            if (list.Count == 0) return null;
            WatchTowerAIScript nearest;
            nearest = list[0];

            float nearestDist = Vector2.Distance(selfTransform.position, nearest.transform.position);
            for (int i = 1; i < list.Count; i++)
            {
                float distA = Vector2.Distance(selfTransform.position, list[i].transform.position);//Find current distance

                if (distA < nearestDist)
                {
                    nearest = list[i];
                    nearestDist = distA;
                }
            }
            return nearest;
        }

        void TryFaceTowardsDirection()
        {
            float angle = Vector2.SignedAngle(dirToTarget, tankController.muzzleTransform.up);
            if (Mathf.Abs(angle) > 2)
            {
                if (angle > 0)
                { tankController.MuzzleRotate(1); }
                else { tankController.MuzzleRotate(-1); }
            }
        }

        bool IsFacingTarget(Transform target)
        {
            Vector2 dirToT = (target.position - selfTransform.position).normalized;
            float angle = Vector2.Angle(dirToT, tankController.muzzleTransform.up);
            if (angle < 2)
            { return true; }
            return false;
        }

        List<WatchTowerAIScript> PickAliveWatchTowers(List<WatchTowerAIScript> list)
        {
            List<WatchTowerAIScript> output = new List<WatchTowerAIScript>();
            foreach (var item in list)
            {
                if (item.GetComponent<HealthScript>().currentHP > 0)
                {
                    output.Add(item);
                }
            }
            return output;
        }

        List<GameObject> FindNearbyUnits(float rad)
        {
            List<GameObject> nearbyEnemies = new List<GameObject>();

            Array.Clear(colliders, 0, colliders.Length);

            Physics2D.OverlapCircleNonAlloc(selfTransform.position, rad, colliders);

            for (int i = 0; i < colliders.Length; i++)
            {
                var item = colliders[i];

                if (item != null)
                {
                    UnitComponent enemUnit = item.GetComponent<UnitComponent>();
                    if (enemUnit != null)
                    {
                        if (enemUnit.teamID != selfUnit.teamID)
                        {
                            nearbyEnemies.Add(item.gameObject);
                        }
                    }
                }
                else
                    break;
            }
            return nearbyEnemies;

        }
    }
}