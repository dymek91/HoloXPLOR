using System;
using System.Xml.Serialization;

namespace HoloXPLOR.Data.DataForge
{
    [XmlRoot(ElementName = "BehaviorTree")]
    public partial class BehaviorTree
    {
        [XmlArray(ElementName = "fragments")]
        [XmlArrayItem(Type = typeof(BTElement))]
        [XmlArrayItem(Type = typeof(BTNode))]
        [XmlArrayItem(Type = typeof(BTDecorator))]
        [XmlArrayItem(Type = typeof(BTFlowControl))]
        [XmlArrayItem(Type = typeof(BTEmbedBT))]
        [XmlArrayItem(Type = typeof(BTStateMachine))]
        [XmlArrayItem(Type = typeof(BTStateMachineState))]
        [XmlArrayItem(Type = typeof(BTPriority))]
        [XmlArrayItem(Type = typeof(BTPriorityChild))]
        [XmlArrayItem(Type = typeof(BTPriorityCondition))]
        [XmlArrayItem(Type = typeof(BTPriorityDefault))]
        [XmlArrayItem(Type = typeof(BTShip_Drift))]
        [XmlArrayItem(Type = typeof(BTShip_FlyFlourishSpline))]
        [XmlArrayItem(Type = typeof(BTShip_FlySpline))]
        [XmlArrayItem(Type = typeof(BTShip_Goto))]
        [XmlArrayItem(Type = typeof(BTShip_MaintainVel))]
        [XmlArrayItem(Type = typeof(BTShip_Roll))]
        [XmlArrayItem(Type = typeof(BTShip_Stop))]
        [XmlArrayItem(Type = typeof(BTShip_TrackEntity))]
        [XmlArrayItem(Type = typeof(BTShip_TurnToTarget))]
        [XmlArrayItem(Type = typeof(BTJamThruster))]
        [XmlArrayItem(Type = typeof(BTResetTurrets))]
        [XmlArrayItem(Type = typeof(BTSwitchOffEngine))]
        [XmlArrayItem(Type = typeof(BTWaitForIFCS))]
        [XmlArrayItem(Type = typeof(BTCharacter_ExactGoto))]
        [XmlArrayItem(Type = typeof(BTCharacter_Goto))]
        [XmlArrayItem(Type = typeof(BTCoverExit))]
        [XmlArrayItem(Type = typeof(BTCoverLeanIn))]
        [XmlArrayItem(Type = typeof(BTCoverLeanOut))]
        [XmlArrayItem(Type = typeof(BTFire))]
        [XmlArrayItem(Type = typeof(BTFireControl))]
        [XmlArrayItem(Type = typeof(BTPlayCharacterAnimation))]
        [XmlArrayItem(Type = typeof(BTSetIronsightMode))]
        [XmlArrayItem(Type = typeof(BTSetStance))]
        [XmlArrayItem(Type = typeof(BTEqualsNow))]
        [XmlArrayItem(Type = typeof(BTHasEntityStateValueNow))]
        [XmlArrayItem(Type = typeof(BTHasTagNow))]
        [XmlArrayItem(Type = typeof(BTHasVariableNow))]
        [XmlArrayItem(Type = typeof(BTLessThanEqualsNow))]
        [XmlArrayItem(Type = typeof(BTLessThanNow))]
        [XmlArrayItem(Type = typeof(BTNotHasEntityStateValueNow))]
        [XmlArrayItem(Type = typeof(BTNotHasVariableNow))]
        [XmlArrayItem(Type = typeof(BTTimeGreaterThanNow))]
        [XmlArrayItem(Type = typeof(BTTimeLessThanNow))]
        [XmlArrayItem(Type = typeof(BTAdd))]
        [XmlArrayItem(Type = typeof(BTDot))]
        [XmlArrayItem(Type = typeof(BTMultiply))]
        [XmlArrayItem(Type = typeof(BTNormalize))]
        [XmlArrayItem(Type = typeof(BTSubtract))]
        [XmlArrayItem(Type = typeof(BTCoverCheck))]
        [XmlArrayItem(Type = typeof(BTFindEntitiesWithTags))]
        [XmlArrayItem(Type = typeof(BTFindRandomEntityWithTags))]
        [XmlArrayItem(Type = typeof(BTShip_FindStuntSpline))]
        [XmlArrayItem(Type = typeof(BTTPSQuery))]
        [XmlArrayItem(Type = typeof(BTAddTag))]
        [XmlArrayItem(Type = typeof(BTAttachEntity))]
        [XmlArrayItem(Type = typeof(BTClaimEntity))]
        [XmlArrayItem(Type = typeof(BTClearTimestampVariable))]
        [XmlArrayItem(Type = typeof(BTCompareNow))]
        [XmlArrayItem(Type = typeof(BTCompute))]
        [XmlArrayItem(Type = typeof(BTCreateEntity))]
        [XmlArrayItem(Type = typeof(BTDestroyEntity))]
        [XmlArrayItem(Type = typeof(BTDetachEntity))]
        [XmlArrayItem(Type = typeof(BTDistanceToBoundsEdge))]
        [XmlArrayItem(Type = typeof(BTEraseBBValue))]
        [XmlArrayItem(Type = typeof(BTEraseTag))]
        [XmlArrayItem(Type = typeof(BTEraseVariable))]
        [XmlArrayItem(Type = typeof(BTExecute))]
        [XmlArrayItem(Type = typeof(BTFail))]
        [XmlArrayItem(Type = typeof(BTGenerateRandom2dDirection))]
        [XmlArrayItem(Type = typeof(BTGenerateRandomFloat))]
        [XmlArrayItem(Type = typeof(BTGenerateRandomInt))]
        [XmlArrayItem(Type = typeof(BTGenerateRandomNumber))]
        [XmlArrayItem(Type = typeof(BTGenerateRandomPosition))]
        [XmlArrayItem(Type = typeof(BTGetEntityPos))]
        [XmlArrayItem(Type = typeof(BTGetEntityTransform))]
        [XmlArrayItem(Type = typeof(BTGetSignalParameter))]
        [XmlArrayItem(Type = typeof(BTNoop))]
        [XmlArrayItem(Type = typeof(BTPersonalLog))]
        [XmlArrayItem(Type = typeof(BTPopArrayValue))]
        [XmlArrayItem(Type = typeof(BTPopRandomArrayValue))]
        [XmlArrayItem(Type = typeof(BTReleaseEntity))]
        [XmlArrayItem(Type = typeof(BTReplaceTag))]
        [XmlArrayItem(Type = typeof(BTSendResponseSignal))]
        [XmlArrayItem(Type = typeof(BTSendSignal))]
        [XmlArrayItem(Type = typeof(BTSendSignalToGroup))]
        [XmlArrayItem(Type = typeof(BTSendSubsumptionEvent))]
        [XmlArrayItem(Type = typeof(BTSetActivity))]
        [XmlArrayItem(Type = typeof(BTSetBBValue))]
        [XmlArrayItem(Type = typeof(BTSetBranchTag))]
        [XmlArrayItem(Type = typeof(BTSetSubactivity))]
        [XmlArrayItem(Type = typeof(BTSetTimestampVariable))]
        [XmlArrayItem(Type = typeof(BTSetValue))]
        [XmlArrayItem(Type = typeof(BTSetVariable))]
        [XmlArrayItem(Type = typeof(BTSuccess))]
        [XmlArrayItem(Type = typeof(BTTeleportEntity))]
        [XmlArrayItem(Type = typeof(BTWait))]
        [XmlArrayItem(Type = typeof(BTWaitForSignal))]
        [XmlArrayItem(Type = typeof(BTWaitForSubsumptionEvent))]
        [XmlArrayItem(Type = typeof(BTWaitRandom))]
        [XmlArrayItem(Type = typeof(BTShip_CreateFormation))]
        [XmlArrayItem(Type = typeof(BTShip_DisbandFormation))]
        [XmlArrayItem(Type = typeof(BTShip_FlyInFormation))]
        [XmlArrayItem(Type = typeof(BTAnimate))]
        [XmlArrayItem(Type = typeof(BTTurnToFacePosition))]
        [XmlArrayItem(Type = typeof(BTFindLandingArea))]
        [XmlArrayItem(Type = typeof(BTFindLandingSpline))]
        [XmlArrayItem(Type = typeof(BTFindTakeOffSpline))]
        [XmlArrayItem(Type = typeof(BTGetLandingAreaInfo))]
        [XmlArrayItem(Type = typeof(BTInLandingAreaNow))]
        [XmlArrayItem(Type = typeof(BTIsLanded))]
        [XmlArrayItem(Type = typeof(BTLandingSystemEnable))]
        [XmlArrayItem(Type = typeof(BTLandingSystemLand))]
        [XmlArrayItem(Type = typeof(BTLandingSystemRequestEngagement))]
        [XmlArrayItem(Type = typeof(BTLandingSystemTakeOff))]
        [XmlArrayItem(Type = typeof(BTAdjustCoverStance))]
        [XmlArrayItem(Type = typeof(BTBlindFireFromCover))]
        [XmlArrayItem(Type = typeof(BTClaimCoverSpot))]
        [XmlArrayItem(Type = typeof(BTPeekFromCover))]
        [XmlArrayItem(Type = typeof(BTShootFromCover))]
        [XmlArrayItem(Type = typeof(BTCommunicate))]
        [XmlArrayItem(Type = typeof(BTConverse))]
        [XmlArrayItem(Type = typeof(BTExactMove))]
        [XmlArrayItem(Type = typeof(BTMove))]
        [XmlArrayItem(Type = typeof(BTMoveAndAnimate))]
        [XmlArrayItem(Type = typeof(BTMoveToCover))]
        [XmlArrayItem(Type = typeof(BTStopMovement))]
        [XmlArrayItem(Type = typeof(BTSendTransitionSignal))]
        [XmlArrayItem(Type = typeof(BTExecuteLua))]
        [XmlArrayItem(Type = typeof(BTEnterUsable))]
        [XmlArrayItem(Type = typeof(BTExitUsable))]
        [XmlArrayItem(Type = typeof(BTGetUseChannelPlacement))]
        [XmlArrayItem(Type = typeof(BTHasFreeUseChannelNow))]
        [XmlArrayItem(Type = typeof(BTIsUsingNow))]
        [XmlArrayItem(Type = typeof(BTUseUsable))]
        [XmlArrayItem(Type = typeof(BTShip_FindObstacleToHit))]
        [XmlArrayItem(Type = typeof(BTShip_SelfDestruct))]
        [XmlArrayItem(Type = typeof(BTThrowException))]
        [XmlArrayItem(Type = typeof(BTScreenMessage))]
        [XmlArrayItem(Type = typeof(BTParallelUntilAllComplete))]
        [XmlArrayItem(Type = typeof(BTParallelUntilAnyComplete))]
        [XmlArrayItem(Type = typeof(BTParallelUntilFailure))]
        [XmlArrayItem(Type = typeof(BTSelector))]
        [XmlArrayItem(Type = typeof(BTSequence))]
        [XmlArrayItem(Type = typeof(BTEqualsOnEnter))]
        [XmlArrayItem(Type = typeof(BTHasEntityStateValueOnEnter))]
        [XmlArrayItem(Type = typeof(BTHasTagOnEnter))]
        [XmlArrayItem(Type = typeof(BTHasVariableOnEnter))]
        [XmlArrayItem(Type = typeof(BTLessThanEqualsOnEnter))]
        [XmlArrayItem(Type = typeof(BTLessThanOnEnter))]
        [XmlArrayItem(Type = typeof(BTMonitorEquals))]
        [XmlArrayItem(Type = typeof(BTMonitorHasEntityStateValue))]
        [XmlArrayItem(Type = typeof(BTMonitorHasTag))]
        [XmlArrayItem(Type = typeof(BTMonitorHasVariable))]
        [XmlArrayItem(Type = typeof(BTMonitorLessThan))]
        [XmlArrayItem(Type = typeof(BTMonitorLessThanEquals))]
        [XmlArrayItem(Type = typeof(BTMonitorNotHasEntityStateValue))]
        [XmlArrayItem(Type = typeof(BTMonitorNotHasVariable))]
        [XmlArrayItem(Type = typeof(BTMonitorTimeGreaterThan))]
        [XmlArrayItem(Type = typeof(BTMonitorTimeLessThan))]
        [XmlArrayItem(Type = typeof(BTNotHasEntityStateValueOnEnter))]
        [XmlArrayItem(Type = typeof(BTNotHasVariableOnEnter))]
        [XmlArrayItem(Type = typeof(BTTimeGreaterThanOnEnter))]
        [XmlArrayItem(Type = typeof(BTTimeLessThanOnEnter))]
        [XmlArrayItem(Type = typeof(BTWaitForEquals))]
        [XmlArrayItem(Type = typeof(BTWaitForHasEntityStateValue))]
        [XmlArrayItem(Type = typeof(BTWaitForHasTag))]
        [XmlArrayItem(Type = typeof(BTWaitForHasVariable))]
        [XmlArrayItem(Type = typeof(BTWaitForLessThan))]
        [XmlArrayItem(Type = typeof(BTWaitForLessThanEquals))]
        [XmlArrayItem(Type = typeof(BTWaitForNotHasEntityStateValue))]
        [XmlArrayItem(Type = typeof(BTWaitForNotHasVariable))]
        [XmlArrayItem(Type = typeof(BTWaitForTimeGreaterThan))]
        [XmlArrayItem(Type = typeof(BTWaitForTimeLessThan))]
        [XmlArrayItem(Type = typeof(BTRepeatUntilFails))]
        [XmlArrayItem(Type = typeof(BTRepeatUntilSucceeds))]
        [XmlArrayItem(Type = typeof(BTRepeater))]
        [XmlArrayItem(Type = typeof(BTTimer))]
        [XmlArrayItem(Type = typeof(BTTokenScope))]
        [XmlArrayItem(Type = typeof(BTFailer))]
        [XmlArrayItem(Type = typeof(BTInverter))]
        [XmlArrayItem(Type = typeof(BTSucceeder))]
        [XmlArrayItem(Type = typeof(BTHandleExceptionFail))]
        [XmlArrayItem(Type = typeof(BTHandleExceptionSuccess))]
        [XmlArrayItem(Type = typeof(BTThrowOnFail))]
        [XmlArrayItem(Type = typeof(BTShip_WithAvoidanceMode))]
        [XmlArrayItem(Type = typeof(BTWithBoostMode))]
        [XmlArrayItem(Type = typeof(BTActionSequence))]
        [XmlArrayItem(Type = typeof(BTCallBTChunk))]
        [XmlArrayItem(Type = typeof(BTClearTimestampVariableOnExit))]
        [XmlArrayItem(Type = typeof(BTCompareOnEnter))]
        [XmlArrayItem(Type = typeof(BTEraseBBValueOnExit))]
        [XmlArrayItem(Type = typeof(BTEraseVariableOnExit))]
        [XmlArrayItem(Type = typeof(BTHandleRequestSignal))]
        [XmlArrayItem(Type = typeof(BTMonitorCompare))]
        [XmlArrayItem(Type = typeof(BTReleaseEntityOnExit))]
        [XmlArrayItem(Type = typeof(BTSetControlledEntity))]
        [XmlArrayItem(Type = typeof(BTWaitForCompare))]
        [XmlArrayItem(Type = typeof(BTCoverLeanInOnExit))]
        [XmlArrayItem(Type = typeof(BTExitCoverOnExit))]
        [XmlArrayItem(Type = typeof(BTHasFreeUseChannelOnEnter))]
        [XmlArrayItem(Type = typeof(BTIsUsingOnEnter))]
        [XmlArrayItem(Type = typeof(BTMonitorHasFreeUseChannel))]
        [XmlArrayItem(Type = typeof(BTMonitorIsUsing))]
        [XmlArrayItem(Type = typeof(BTWaitForHasFreeUseChannel))]
        [XmlArrayItem(Type = typeof(BTWaitForIsUsing))]
        [XmlArrayItem(Type = typeof(BTInLandingAreaOnEnter))]
        [XmlArrayItem(Type = typeof(BTMonitorInLandingArea))]
        [XmlArrayItem(Type = typeof(BTWaitForInLandingArea))]
        [XmlArrayItem(Type = typeof(BTReleaseCoverOnExit))]
        public BTElement[] Fragments { get; set; }

        [XmlArray(ElementName = "signalHandlers")]
        [XmlArrayItem(Type = typeof(BTSetOnSignal))]
        public BTSetOnSignal[] SignalHandlers { get; set; }

        [XmlArray(ElementName = "timestampSignals")]
        [XmlArrayItem(Type = typeof(BTTimestampOnSignal))]
        public BTTimestampOnSignal[] TimestampSignals { get; set; }

        [XmlArray(ElementName = "signalQueues")]
        [XmlArrayItem(ElementName = "String", Type=typeof(_String))]
        public _String[] SignalQueues { get; set; }

        [XmlAttribute(AttributeName = "btHandlesScriptedTask")]
        public Boolean BtHandlesScriptedTask { get; set; }

    }
}
