using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HTTLVN.QLTLH.Models;

namespace HTTLVN.QLTLH.Code
{
    public static class MimeAssistant
    {
        public static string GetMimeType(string extension)
        {
            string mime;
            return Mappings.TryGetValue(extension, out mime) ? mime : "application/octet-stream";
        }
        private static readonly Dictionary<string, string> Mappings =
            new Dictionary<string, string>
                {
                    {"doc", "application/msword"},
                    {"pdf", "application/pdf"},
                    {"docx", "application/msword"},
                    {"txt", "text/plain"},
                    {"xlsx", "application/vnd.ms-excel"},
                    {"xls", "application/vnd.ms-excel"},
                    {"xml", "application/xml"},
                    {"zip", "application/zip"},
                    {"ppt", "image/vnd.ms-powerpoint"},
                    {"jpeg", "image/jpeg"},
                    {"png", "image/png"},
                };
    }
    public static class Constant
    {
        public static string SessionLogin = "seesionLogin";
        public static string SessionLoginTinHuu = "seesionLoginTinHuu";
        public static string SessionLoginChurchId = "sessionLoginChurchId";
        public static string Document_import = ConfigurationManager.AppSettings["Document_import"];
        public static string Document_Avatar = ConfigurationManager.AppSettings["Document_Avatar"];
        public static string TinHuu_HinhDaiDien = ConfigurationManager.AppSettings["TinHuu_HinhDaiDien"];
        public static string TinHuu_HinhGiaDinh = ConfigurationManager.AppSettings["TinHuu_HinhGiaDinh"];
        public static string Document_Church = ConfigurationManager.AppSettings["Document_Church"];
        public static string Document_BangCap = ConfigurationManager.AppSettings["Document_BangCapNguoiPhoiNgau"];
        public static string Document_GiaoVuLenh = ConfigurationManager.AppSettings["Document_GiaoVuLenh"];
        public static string Document_VanThu = ConfigurationManager.AppSettings["Document_VanThu"];
        public static string adminRole = "superAdmin";
        public static string clientRole = "clientSales";
        public static string SITE_DOMAIN_NAME = "esmr.net";
        public static string mbs_filename = "MBS Daily Workbook.xls";
        public static string global_filepattern = "global.xls";
        public static string mbs_values_order = "'cof', 'settle_date', 'gf', 'sf_value', 'sf_excess', 'excess_limit', 'index_value'";

    }

    public class CommonFunction
    {
        public static readonly char[] _delimiterPost = { ',' };
        private static readonly char[] _delimiterslash = { '/' };
        private static DateTime MinValue = new DateTime(1900, 1, 1);

        public static string GetNewFileName(string path, string file)
        {
            var extension = Path.GetExtension(file);
            var fileName = Path.GetFileNameWithoutExtension(file);
            file = string.Format("{0}{1}", fileName, extension);
            var inc = 1;
            var systemLocation = Path.Combine(path, file);
            if (File.Exists(systemLocation))
            {
                do
                {
                    file = string.Format("{0}.{1}{2}", fileName, inc, extension);
                    systemLocation = Path.Combine(path, file);
                    inc++;
                } while (File.Exists(systemLocation));
            }
            return file.Replace("..", ".");
        }

        public static string[] GetValueBySplitString(string needsplit)
        {
            if (!string.IsNullOrWhiteSpace(needsplit))
                return needsplit.Split(_delimiterPost);
            return null;
        }

        public static DateTime FromExcelSerialDate(int SerialDate)
        {
            if (SerialDate > 59) SerialDate -= 1; //Excel/Lotus 2/29/1900 bug   
            return new DateTime(1899, 12, 31).AddDays(SerialDate);
        }

        public static string HumanReadFile(string fileName)
        {
            var file = fileName.Trim();
            if (!string.IsNullOrEmpty(file) && file.Length > 0 && file.IndexOf(@"\", StringComparison.Ordinal) > 0)
            {
                file = file.Substring(file.LastIndexOf(@"\") + 1, file.Length - (file.LastIndexOf(@"\") + 1));
            }
            if (!string.IsNullOrEmpty(file) && file.Length > 0 && file.IndexOf(@"/", StringComparison.Ordinal) > 0)
            {
                file = file.Substring(file.LastIndexOf(@"/") + 1, file.Length - (file.LastIndexOf(@"/") + 1));
            }
            return file;
        }



        public static double ConvertDouble(string str)
        {
            double res;
            double.TryParse(str, out res);
            return res;
        }

        /// <summary>
        /// On case user only input year, or year month or full year and month
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ConvertCustomDateTime(string str)
        {
            var res = DateTime.MinValue;
            try
            {
                if (!string.IsNullOrEmpty(str) && str.IndexOf("/", System.StringComparison.Ordinal) > -1)
                {
                    var split = str.Split(_delimiterslash);

                    switch (split.Count())
                    {

                        case 1:
                            {
                                res = new DateTime(ConvertInt(split[0]), 2, 1);
                                break;
                            }
                        case 2:
                            {
                                res = new DateTime(ConvertInt(split[1]), ConvertInt(split[0]), 1);
                            }
                            break;
                        case 3:
                            {
                                res = new DateTime(ConvertInt(split[2]), ConvertInt(split[1]), ConvertInt(split[0]));
                            }
                            break;
                        default:
                            DateTime.TryParse(str, new CultureInfo("vi-VN"), DateTimeStyles.None, out res);
                            break;
                    }
                }
                else
                {
                    res = new DateTime(ConvertInt(str), 1, 1);
                }
            }
            catch (Exception e)
            {
                res = DateTime.MinValue;
            }
            return res;

        }

        public static DateTime ConvertCustomDateTime(string str, bool isFrom)
        {
            var res = DateTime.MinValue;
            try
            {
                if (!string.IsNullOrEmpty(str) && str.IndexOf("/", System.StringComparison.Ordinal) > -1)
                {
                    var split = str.Split(_delimiterslash);

                    switch (split.Count())
                    {

                        case 1:
                            {
                                res = new DateTime(ConvertInt(split[0]), 2, 1);
                                break;
                            }
                        case 2:
                            {
                                if (isFrom)
                                {
                                    res = new DateTime(ConvertInt(split[1]), ConvertInt(split[0]), 1);
                                }
                                else
                                {
                                    var year = ConvertInt(split[1]);
                                    var month = ConvertInt(split[0]);
                                    var dayinMoth = DateTime.DaysInMonth(year, month);
                                    res = new DateTime(year, month, dayinMoth);
                                }
                            }
                            break;
                        case 3:
                            {
                                res = new DateTime(ConvertInt(split[2]), ConvertInt(split[1]), ConvertInt(split[0]));
                            }
                            break;
                        default:
                            DateTime.TryParse(str, new CultureInfo("vi-VN"), DateTimeStyles.None, out res);
                            break;
                    }
                }
                else
                {
                    res = new DateTime(ConvertInt(str), 1, 1);
                }
            }
            catch (Exception e)
            {
                res = DateTime.MinValue;
            }
            return res;

        }

        public static DateTime ConvertDateTime(string str)
        {
            DateTime res;
            DateTime.TryParse(str, new CultureInfo("vi-VN"), DateTimeStyles.None, out res);
            return res;
        }

        public static DateTime ConvertDateTimeYearOnly(string str)
        {
            var res = DateTime.MinValue;
            try
            {
                if (!string.IsNullOrEmpty(str) && str.IndexOf("/", System.StringComparison.Ordinal) > -1)
                {
                    var split = str.Split(_delimiterslash);
                    switch (split.Count())
                    {

                        case 1:
                            {
                                res = new DateTime(ConvertInt(split[0]), 1, 1);
                                break;
                            }
                        case 2:
                            {
                                res = new DateTime(ConvertInt(split[1]), ConvertInt(split[0]), 1);
                            }
                            break;
                        case 3:
                            {
                                res = new DateTime(ConvertInt(split[2]), ConvertInt(split[1]), 1);
                            }
                            break;
                        default:
                            DateTime.TryParse(str, new CultureInfo("vi-VN"), DateTimeStyles.None, out res);
                            break;
                    }
                }
                else
                {
                    res = new DateTime(ConvertInt(str), 1, 1);
                }
            }
            catch (Exception e)
            {
                res = DateTime.MinValue;
            }
            return res;
        }

        public static DateTime ConvertDateTime(string str, bool isVietNam)
        {
            DateTime res;
            DateTime.TryParse(str, new CultureInfo("vi-VN"), DateTimeStyles.None, out res);
            return res;
        }

        public static int ConvertInt(long str)
        {
            int res;
            int.TryParse(str.ToString(CultureInfo.InvariantCulture), out res);
            return res;
        }

        public static int ConvertInt(string str)
        {
            int res;
            int.TryParse(str, out res);
            return res;
        }

        public static string GetAddress(Address address)
        {
            return address.Street + " " + address.Ward + " " + address.District + " " + address.City + " " +
                   address.HomePhone + " " + address.Fax + " " + address.Email;
        }

        public static string ConvertToUnsign2(string str)
        {
            var strFormD = str.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (
                var t in
                    from t in strFormD
                    let uc = CharUnicodeInfo.GetUnicodeCategory(t)
                    where uc != UnicodeCategory.NonSpacingMark
                    select t)
            {
                sb.Append(t);
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }

        public static string TruncateAtWord(string value, int length)
        {
            if (value == null || value.Length < length ||
                value.IndexOf(" ", length, System.StringComparison.Ordinal) == -1)
                return value;

            return value.Substring(0, value.IndexOf(" ", length, System.StringComparison.Ordinal));
        }

        public static string ConvertToNamChucVu(TimeSpan? timeSpan)
        {
            int soNam = timeSpan.Value.Days / 360;
            int soThang = (timeSpan.Value.Days % 360) / 12;
            string result = "";
            if (soNam > 0)
            {
                result = soNam + " năm ";
            }
            if (soThang > 0)
            {
                result += soThang + " tháng";
            }
            return result;

        }

        public static string SetPassword(string password)
        {
            return Crypto.Hash(password);
        }

        public static bool CheckPassword(string password, string compare)
        {
            return string.Equals(compare, Crypto.Hash(password));
        }

        public static bool CheckValidationInput(Dictionary<string, string> erros, string compare)
        {
            return erros != null && erros.Any(error => error.Key.IndexOf(compare) != -1);
        }

        public static List<Clergy_AssignmentHistoryComparable> SortAssignmentHistories(
            List<Clergy_AssignmentHistory> sortlist)
        {
            var list =
                sortlist.Select(assignmentHistory => new Clergy_AssignmentHistoryComparable(assignmentHistory)).ToList();
            list = list.OrderByDescending(a => a.DateStartDate).ToList();
            return list;
        }

        public static List<VAssignComparable> SortViewAssignmentHistories(List<v_assign> sortlist)
        {
            var list = sortlist.Select(assignmentHistory => new VAssignComparable(assignmentHistory)).ToList();
            list = list.OrderByDescending(a => a.DateStartDate).ToList();
            return list;
        }


        public static List<Clergy_AssignmentHistory> GetIsCurrentList(List<Clergy_AssignmentHistory> list)
        {
            var result = new List<Clergy_AssignmentHistory>();
            foreach (var clergyAssignmentHistory in list)
            {
                if ((string.IsNullOrEmpty(clergyAssignmentHistory.EndDate)  && clergyAssignmentHistory.ChucVu !=null && (clergyAssignmentHistory.ChucVu.Name.Equals("tạm lo",StringComparison.CurrentCultureIgnoreCase)|| clergyAssignmentHistory.ChucVu.Name.Equals("kiêm nhiệm", StringComparison.CurrentCultureIgnoreCase))) ||  ConvertCustomDateTime(clergyAssignmentHistory.EndDate).Date >= DateTime.Now.Date )
                {
                    result.Add(clergyAssignmentHistory);
                }
            }
            return result;
        }

        public static List<Clergy_AssignmentHistory> GetIsNotCurrentList(List<Clergy_AssignmentHistory> list)
        {
            var result = new List<Clergy_AssignmentHistory>();
            foreach (var clergyAssignmentHistory in list)
            {
                if (ConvertCustomDateTime(clergyAssignmentHistory.EndDate).Date < DateTime.Now.Date && !string.IsNullOrEmpty(clergyAssignmentHistory.EndDate))
                {
                    result.Add(clergyAssignmentHistory);
                }
            }
            return result;
        }

        public static bool IsValidEmail(string inputEmail)
        {
            const string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            var re = new Regex(strRegex);
            return re.IsMatch(inputEmail);
        }

        public static bool IsDublicateID(string identityNumber, out string tengiaopham)
        {
            tengiaopham = string.Empty;
            var db = new CodeFirstTongLienHoiEntities();
            var giaopham = db.Clergies.FirstOrDefault(a => a.IdentityNumber == identityNumber);
            if (giaopham != null)
                tengiaopham = giaopham.FirstName + " " + giaopham.MiddleName + " " + giaopham.LastName;
            return giaopham != null;
        }

        public static bool IsContainTwoID(string identityNumber, int id, out string tengiaopham)
        {
            tengiaopham = string.Empty;
            var db = new CodeFirstTongLienHoiEntities();
            var giaopham = db.Clergies.FirstOrDefault(a => a.IdentityNumber == identityNumber);
            return giaopham != null && giaopham.Id != id;
        }

        public static bool IsValidateDateString(string date)
        {
            if (!string.IsNullOrEmpty(date) && (
                ConvertCustomDateTime(date) > DateTime.MaxValue ||
                ConvertCustomDateTime(date) < MinValue))
                return true;
            return !string.IsNullOrEmpty(date) &&
                   ConvertCustomDateTime(date) == DateTime.MinValue;
        }

        public static bool IsValidateFullDateString(string date)
        {
            var result = false;
            try
            {
                if (!string.IsNullOrEmpty(date) && (
                    ConvertDateTime(date) > DateTime.MaxValue ||
                    ConvertDateTime(date) < MinValue))
                    result = true;
                if (!string.IsNullOrEmpty(date) &&
                    ConvertDateTime(date) == DateTime.MinValue)
                    result = true;
            }
            catch
            {
                result = true;
            }
            return result;
        }

        public static bool CompareDateTime(string date, DateTime? needCompare)
        {
            return !string.IsNullOrEmpty(date) &&
                   ConvertCustomDateTime(date) > DateTime.MinValue &&
                   (needCompare) > DateTime.MinValue &&
                   (needCompare) <=
                   ConvertCustomDateTime(date);
        }

        public static bool CompareDateTime(string date, string needCompare)
        {
            if (!IsValidateDateString(needCompare))
            {
                return !string.IsNullOrEmpty(date) &&
                       ConvertCustomDateTime(date) > DateTime.MinValue &&
                       ConvertCustomDateTime(needCompare) > DateTime.MinValue &&
                       ConvertCustomDateTime(needCompare) > MinValue &&
                       !string.IsNullOrEmpty(needCompare) &&
                       ConvertCustomDateTime(needCompare) <=
                       ConvertCustomDateTime(date);
            }
            return false;

        }

        public static DateType GetDateType(string date)
        {
            if (IsValidateDateString(date)) return DateType.Year;
            if (date.Length == 4)
                return DateType.Year;
            if (date.Length > 4 && date.Length <= 7)
            {
                return DateType.MonthYear;
            }
            return date.Length > 7 ? DateType.FulDate : DateType.Year;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }


        internal static object CheckPassword(string password, string p, string church_id)
        {
            throw new NotImplementedException();
        }
    }

    public enum DateType { Year, MonthYear, FulDate }

    public static class Crypto
    {
        public static string Hash(string value)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(value)));
        }
    }

    public class TestDb
    {
        private CodeFirstTongLienHoiEntities db = new CodeFirstTongLienHoiEntities();
        public void LoadGiaoPham(int id)
        {
            var giaoPham = db.Clergies.FirstOrDefault(a => a.Id == id);
        }
    }
}