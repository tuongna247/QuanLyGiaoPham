using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using HTTLVN.QLTLH.Models.DBO;
using HTTLVN.QLTLH.Models.Services;

namespace HTTLVN.QLTLH.Models.Controllers
{
    public class AutoCodeController
    {
        /// <summary>
        /// get string config by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetConfigString(int type)
        {
            var result = "";
            var configs = GetAutoCode_SubsByType(type);
            var datetime = DateTime.Now.ToString("yyyyMMdd");
            foreach (var autoCodeSubDbo in configs)
            {
                if (autoCodeSubDbo.Type == null) continue;
                switch (autoCodeSubDbo.Type.ToUpper().Trim())
                {
                    case "Y":
                        {
                            if (autoCodeSubDbo.Length == 2)
                                result += datetime.Substring(2, 2);
                            if (autoCodeSubDbo.Length == 4)
                                result += datetime.Substring(0, 4);
                            break;
                        }
                    case "M":
                        {
                            if (autoCodeSubDbo.Length == 1)
                                result += datetime.Substring(5, 1);
                            if (autoCodeSubDbo.Length == 2)
                                result += datetime.Substring(4, 2);
                            break;
                        }
                    case "D":
                        {
                            if (autoCodeSubDbo.Length == 1)
                                result += datetime.Substring(7, 1);
                            if (autoCodeSubDbo.Length == 2)
                                result += datetime.Substring(6, 2);
                            break;
                        }
                    case "C1":
                    case "C2":
                    case "C3":
                    case "T":
                        {
                            result += autoCodeSubDbo.FixChar.Trim();
                            break;
                        }
                    case "AT":
                        {
                            result += "*".PadRight(autoCodeSubDbo.Length, '*');
                            break;
                        }
                }
            }
            return result.ToString();
        }

        public string GetAutoValueFromConfig(int type)
        {
            var sqlService = new SqlService();
            var codeWillbeUse = GetConfigString(type);
            int length = codeWillbeUse.Length - codeWillbeUse.IndexOf("*", StringComparison.Ordinal);
            var codeDigit = codeWillbeUse.Substring(0, codeWillbeUse.Length - length);
            sqlService.AddParameter("@CodeType", SqlDbType.Int, type);
            sqlService.AddParameter("@CodeDigit", SqlDbType.Char, codeDigit);
            sqlService.AddParameter("@digitLen", SqlDbType.Int, length);
            SqlDataReader reader = sqlService.ExecuteSPReader("USP_Autocode_Max_Select");
            string getNextCode = string.Empty;
            while (reader.Read())
            {
                getNextCode = reader[0].ToString();
            }
            return codeDigit + getNextCode.PadLeft(length, '0');

        }
        public AutoCodeDBO GetAutoCodeByType(int type)
        {
            AutoCodeDBO result = null;
            var sqlService = new SqlService();
            sqlService.AddParameter("@Id", SqlDbType.Int, type);
            var reader = sqlService.ExecuteSqlReader("select * from AutoCode where Id = @Id");
            var properties = typeof(AutoCodeDBO).GetProperties();
            while (reader.Read())
            {
                var element = new AutoCodeDBO();
                foreach (var f in properties)
                {
                    if (f.Name == "MAXCNT" || f.Name == "ROWNUMBER") continue;
                    var o = reader[f.Name];
                    if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
                }
                result = element;
                break;
            }
            return result;
        }

        public List<AutoCode_SubDBO> GetAutoCode_SubsByType(int type)
        {
            var danhmucs = new List<AutoCode_SubDBO>();
            var sqlService = new SqlService();
            sqlService.AddParameter("@CodeType", SqlDbType.Int, type);
            var reader = sqlService.ExecuteSqlReader("select * from AutoCode_Sub where CodeType = @CodeType order by position ");
            var properties = typeof(AutoCode_SubDBO).GetProperties();
            while (reader.Read())
            {
                var element = new AutoCode_SubDBO();
                foreach (var f in properties)
                {
                    if (f.Name == "MAXCNT" || f.Name == "ROWNUMBER") continue;
                    var o = reader[f.Name];
                    if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
                }
                danhmucs.Add(element);
            }
            return danhmucs;
        }

    }
}
