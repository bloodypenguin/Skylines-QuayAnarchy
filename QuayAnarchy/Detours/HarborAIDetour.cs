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
            Vector3 pos;
            Vector3 dir;
            if (this.m_info.m_placementMode == BuildingInfo.PlacementMode.Shoreline && BuildingTool.SnapToCanal(position, out pos, out dir, 40f, false))
            {
                angle = Mathf.Atan2(dir.x, -dir.z);
                Vector3 vector3 = pos + dir * (this.m_info.m_generatedInfo.m_max.z - 6f);
                position.x = vector3.x;
                position.z = vector3.z;
            }
            ToolBase.ToolErrors toolErrors = base.CheckBuildPosition(relocateID, ref position, ref angle, waterHeight, elevation, ref connectionSegment, out productionRate, out constructionCost);
            //begin mod
            //end mod
            Vector3 position1 = Building.CalculatePosition(position, angle, Util.GetPrivate<Vector3>(this, "m_connectionOffset"));
            Vector3 position2 = position1;
            uint laneID;
            byte offset;
            if (ShipDockAI.FindConnectionPath(ref position2, out laneID, out offset))
            {
                position1.y = position2.y;
                connectionSegment.a = position1;
                connectionSegment.b = position2;
                if (!Singleton<TerrainManager>.instance.HasWater(Segment2.XZ(connectionSegment), 50f, false))
                    toolErrors |= ToolBase.ToolErrors.CannotConnect;
            }
            else
                toolErrors |= ToolBase.ToolErrors.CannotConnect;
            GuideController guideController = Singleton<GuideManager>.instance.m_properties;
            if (guideController != null)
            {
                if ((toolErrors & ToolBase.ToolErrors.CannotConnect) != ToolBase.ToolErrors.None)
                    Singleton<BuildingManager>.instance.m_harborPlacement.Activate(guideController.m_harborPlacement);
                else
                    Singleton<BuildingManager>.instance.m_harborPlacement.Deactivate();
            }
            return toolErrors;
        }
    }
}