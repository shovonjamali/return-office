using System.Collections.Generic;

namespace WFO.RTO_CLV.RERWeb.Entities
{
    public class PermissionMatrix
    {
        public string State { get; set; }
        public List<PermissionUnit> PermissionUnitList { get; set; }
    }
}