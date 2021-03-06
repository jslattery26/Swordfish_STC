﻿using System.Collections.Generic;
using Passer;
using Passer.Humanoid.Tracking;
using UnityEngine;

namespace Passer.Humanoid {

    [System.Serializable]
    [CreateAssetMenu(menuName = "Humanoid/Pose", fileName = "HumanoidPose",order = 103)]
    public class Pose : ScriptableObject {
        /// <summary>
        /// The bones affected by this pose
        /// </summary>
        public List<BonePose> bonePoses;

        public BonePose CheckBone(Bone boneId, bool useSideBones = false) {
            BonePose bone;
            if (useSideBones) {
                SideBone sideBone = BoneReference.HumanoidSideBone(boneId);
                bone = GetSideBone(sideBone);
            } else
                bone = GetBone(boneId);
            if (bone == null)
                bone = AddBone(boneId, useSideBones);
            return bone;
        }

        public BonePose GetBone(Bone boneId) {
            if (bonePoses != null)
                for (int i = 0; i < bonePoses.Count; i++) {
                    if (bonePoses[i].boneRef.boneId == boneId)
                        return bonePoses[i];
                }

            return null;
        }

        public BonePose GetSideBone(SideBone sideBoneId) {
            if (bonePoses != null)
                for (int i = 0; i < bonePoses.Count; i++) {
                    if (bonePoses[i].boneRef.sideBoneId == sideBoneId)
                        return bonePoses[i];
                }

            return null;
        }

        public BonePose AddBone(Bone _boneId, bool useSideBones = false) {
            BonePose newBonePose = new BonePose();

            SideBone sideBone = BoneReference.HumanoidSideBone(_boneId);
            if (sideBone == SideBone.None) {                
                newBonePose.boneRef = new BoneReference() {
                    type = BoneType.AllBones,
                    boneId = _boneId
                };
            } else {
                newBonePose.boneRef = new BoneReference() {
                    type = BoneType.SideBones,
                    sideBoneId = sideBone
                };
            }
            newBonePose.translation = Vector3.zero;
            newBonePose.rotation = Quaternion.identity;
            bonePoses.Add(newBonePose);

            return newBonePose;
        }

        public void Cleanup() {
            bonePoses.RemoveAll(bonePose => bonePose == null || (!bonePose.setTranslation && !bonePose.setRotation && !bonePose.setScale));
        }

        public void UpdatePose(HumanoidControl humanoid) {
            UpdatePose(humanoid, Bone.Hips);
            UpdatePose(humanoid, Bone.Head);
            UpdatePose(humanoid, Bone.LeftHand);
            UpdatePose(humanoid, Bone.RightHand);
            UpdatePose(humanoid, Bone.LeftFoot);
            UpdatePose(humanoid, Bone.RightFoot);
            Cleanup();
        }
        private void UpdatePose(HumanoidControl humanoid, Bone boneId) {
            BonePose poseTargetBone = CheckBone(boneId);
            poseTargetBone.UpdateTranslation(humanoid);
            poseTargetBone.UpdateRotation(humanoid);
        }

        public void Show(HumanoidControl humanoid, float value = 1) {
            //ShowBlendshapes(humanoid, value);
            ShowBones(humanoid, value);
        }
        public void Show(HumanoidControl humanoid, Side showSide, float value = 1) {
            //ShowBlendshapes(humanoid, value);
            ShowBones(humanoid, showSide, value);
        }
        public void ShowAdditive(HumanoidControl humanoid, Side showSide, float value = 1) {
            //ShowBlendshapes(humanoid, value);
            ShowBonesAdditive(humanoid, showSide, value);
        }

        public void ShowBones(HumanoidControl humanoid, float value) {
            if (bonePoses == null)
                return;
            foreach (BonePose bonePose in bonePoses)
                bonePose.ShowPose(humanoid, value);                   
        }

        public void ShowBones(HumanoidControl humanoid, Side showSide, float value) {
            if (bonePoses == null)
                return;
            foreach (BonePose bonePose in bonePoses)
                bonePose.ShowPose(humanoid, showSide, value);
        }

        public void ShowBonesAdditive(HumanoidControl humanoid, Side showSide, float value) {
            if (bonePoses == null)
                return;
            foreach (BonePose bonePose in bonePoses)
                bonePose.ShowPoseAdditive(humanoid, showSide, value);
        }

        public float GetScore(HumanoidControl humanoid, Side side) {
            if (bonePoses == null)
                return 0;

                float score = 0;
            float n = 0;
            foreach (BonePose bonePose in bonePoses) {
                score += bonePose.GetScore(humanoid, side);
                n++;
            }
            if (n == 0)
                return 0;
            else
                return score / n;
        }
    }


}