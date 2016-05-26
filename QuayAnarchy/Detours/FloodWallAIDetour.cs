using ColossalFramework;
using ColossalFramework.Math;
using QuayAnarchy.Redirection;
using UnityEngine;

namespace QuayAnarchy.Detours
{
    [TargetType(typeof(FloodWallAI))]
    public class FloodWallAIDetour : PlayerNetAI
    {
        [RedirectMethod]
        public override ToolBase.ToolErrors CheckBuildPosition(bool test, bool visualize, bool overlay, bool autofix, ref NetTool.ControlPoint startPoint, ref NetTool.ControlPoint middlePoint, ref NetTool.ControlPoint endPoint, out BuildingInfo ownerBuilding, out Vector3 ownerPosition, out Vector3 ownerDirection, out int productionRate)
        {
            ToolBase.ToolErrors toolErrors = base.CheckBuildPosition(test, visualize, overlay, autofix, ref startPoint, ref middlePoint, ref endPoint, out ownerBuilding, out ownerPosition, out ownerDirection, out productionRate);
            NetManager instance1 = Singleton<NetManager>.instance;
            TerrainManager instance2 = Singleton<TerrainManager>.instance;
            if ((int)startPoint.m_node != 0)
            {
                if (instance1.m_nodes.m_buffer[(int)startPoint.m_node].Info.m_class.m_level != this.m_info.m_class.m_level)
                    toolErrors |= ToolBase.ToolErrors.InvalidShape;
            }
            else if ((int)startPoint.m_segment != 0 && instance1.m_segments.m_buffer[(int)startPoint.m_segment].Info.m_class.m_level != this.m_info.m_class.m_level)
                toolErrors |= ToolBase.ToolErrors.InvalidShape;
            if ((int)endPoint.m_node != 0)
            {
                if (instance1.m_nodes.m_buffer[(int)endPoint.m_node].Info.m_class.m_level != this.m_info.m_class.m_level)
                    toolErrors |= ToolBase.ToolErrors.InvalidShape;
            }
            else if ((int)endPoint.m_segment != 0 && instance1.m_segments.m_buffer[(int)endPoint.m_segment].Info.m_class.m_level != this.m_info.m_class.m_level)
                toolErrors |= ToolBase.ToolErrors.InvalidShape;
            Vector3 middlePos1;
            Vector3 middlePos2;
            NetSegment.CalculateMiddlePoints(startPoint.m_position, middlePoint.m_direction, endPoint.m_position, -endPoint.m_direction, true, true, out middlePos1, out middlePos2);
            Bezier2 bezier2;
            bezier2.a = VectorUtils.XZ(startPoint.m_position);
            bezier2.b = VectorUtils.XZ(middlePos1);
            bezier2.c = VectorUtils.XZ(middlePos2);
            bezier2.d = VectorUtils.XZ(endPoint.m_position);
            int num = Mathf.CeilToInt(Vector2.Distance(bezier2.a, bezier2.d) * 0.005f) + 3;
            Segment2 segment;
            segment.a = bezier2.a;
            for (int index = 1; index <= num; ++index)
            {
                segment.b = bezier2.Position((float)index / (float)num);
                //begin mod
                //end mod
                segment.a = segment.b;
            }
            return toolErrors;
        }

        [RedirectMethod]
        public override NetInfo GetInfo(float minElevation, float maxElevation, float length, bool incoming, bool outgoing, bool curved, bool enableDouble, ref ToolBase.ToolErrors errors)
        {
            //begin mod
            //end mod
            return this.m_info;
        }
    }
}