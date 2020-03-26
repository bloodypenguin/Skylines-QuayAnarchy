using System.Reflection;
using ColossalFramework;
using ColossalFramework.Math;
using QuayAnarchy.Redirection;
using UnityEngine;

namespace QuayAnarchy.Detours
{
    [TargetType(typeof(HarborAI))]
    public class HarborAIDetour : TransportStationAI
    {
        [RedirectMethod]
        public override ToolBase.ToolErrors CheckBuildPosition(ushort relocateID, ref Vector3 position, ref float angle, float waterHeight, float elevation, ref Segment3 connectionSegment, out int productionRate, out int constructionCost)
        {
            if (this.m_transportLineInfo == null)
                return base.CheckBuildPosition(relocateID, ref position, ref angle, waterHeight, elevation, ref connectionSegment, out productionRate, out constructionCost);
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
                if (!isQuay && !this.m_supportCanals)
                    toolErrors1 |= ToolBase.ToolErrors.ShoreNotFound;
            }
            ToolBase.ToolErrors toolErrors2 = toolErrors1 | base.CheckBuildPosition(relocateID, ref position, ref angle, waterHeight, elevation, ref connectionSegment, out productionRate, out constructionCost);
            //begin mod
            //end mod
            Vector3 position1 = Building.CalculatePosition(position, angle, this.m_connectionOffset);
            Vector3 position2 = position1;
            uint laneID;
            byte offset;
            if (ShipDockAI.FindConnectionPath(ref position2, out laneID, out offset, false))
            {
                position1.y = position2.y;
                connectionSegment.a = position1;
                connectionSegment.b = position2;
                if (!Singleton<TerrainManager>.instance.HasWater(Segment2.XZ(connectionSegment), 50f, false))
                    toolErrors2 |= ToolBase.ToolErrors.CannotConnect;
            }
            else
                toolErrors2 |= ToolBase.ToolErrors.CannotConnect;
            GuideController properties = Singleton<GuideManager>.instance.m_properties;
            if (properties != null)
            {
                if ((toolErrors2 & ToolBase.ToolErrors.CannotConnect) != ToolBase.ToolErrors.None)
                    Singleton<BuildingManager>.instance.m_harborPlacement.Activate(properties.m_harborPlacement);
                else
                    Singleton<BuildingManager>.instance.m_harborPlacement.Deactivate();
            }
            return toolErrors2;
        }

        private Vector3 m_connectionOffset => (Vector3)typeof(HarborAI).GetField("m_connectionOffset",
                BindingFlags.Public | BindingFlags.Instance)
            .GetValue(this);
    }
}