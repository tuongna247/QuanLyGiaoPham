using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTLVN.QLTLH.Models
{
    public class GetPermissionList_Result
    {
        public static GetPermissionList_Result CreateGetPermissionList_Result(global::System.String username, global::System.Int32 id)
        {
            GetPermissionList_Result getPermissionList_Result = new GetPermissionList_Result();
            getPermissionList_Result.username = username;
            getPermissionList_Result.id = id;
            return getPermissionList_Result;
        }
        public string Name { get; set; }
        public string username { get; set; }
        public int id { get; set; }
        public int RoleMappingId { get; set; }
        public int UserId { get; set; }
        public int RoleMapping { get; set; }
    }
}