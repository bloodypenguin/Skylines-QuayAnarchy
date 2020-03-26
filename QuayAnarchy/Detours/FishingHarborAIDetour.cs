using ColossalFramework.Math;
using QuayAnarchy.Redirection;
using UnityEngine;

namespace QuayAnarchy.Detours
{
    [TargetType(typeof(FishingHarborAI))]
    public class FishingHarborAIDetour : FishingHarborAI
    {
        public override ToolBase.ToolErrors CheckBuildPosition(
            ushort relocateID,
            ref Vector3 position,
            ref float angle,
            float waterHeight,
            float elevation,
            ref Segment3 connectionSegment,
            out int productionRate,
            out int constructionCost)
        {
            ToolBase.ToolErrors toolErrors1 = ToolBase.ToolErrors.None;
            Vector3 pos;
            Vector3 dir;
            bool isQuay;
            if (this.m_info.m_placementMode == BuildingInfo.PlacementMode.Shoreline && BuildingTool.SnapToCanal(position, out pos, out dir, out isQuay, 40f, false))
            {
                angle = Mathf.Atan2(dir.x, -dir.z);
                pos += dir * this.m_quayOffset;
                position.x = pos.x;
                position.z = pos.z;
                if (!isQuay)
                    toolErrors1 |= ToolBase.ToolErrors.ShoreNotFound;
            }
            ToolBase.ToolErrors toolErrors2 = toolErrors1 | base.CheckBuildPosition(relocateID, ref position, ref angle, waterHeight, elevation, ref connectionSegment, out productionRate, out constructionCost);
            //begin mod
            //end mod
            return toolErrors2;
        }
    }
}