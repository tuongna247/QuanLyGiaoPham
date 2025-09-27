using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CrystalDecisions.Shared;
using HTTLVN.QLTLH.Models.Services;

namespace HTTLVN.QLTLH.Code.ChiHoi
{
    public class TinHuuController
    {
        public static void ExportTinHuu()
        {
            try
            {
                var sqlService = new SqlService();
                DataSet ds = sqlService.ExecuteSPDataSet("ESP_GetTinHuu");
                ds.Namespace = POSTables._NAMESPACE;
                ds.DataSetName = POSTables._DB;
                ds.Tables[0].TableName = POSTables._TIN_HUU;
                ds.Tables[0].PrimaryKey = new DataColumn[] {ds.Tables[0].Columns[0]};
                ds.WriteXml("d:\\TinHuu.xml", XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {

            }
        }

        //public static bool ExportConsumers(string sFileName)
        //{
        //    try
        //    {
        //        DataSet dsConsumers = TriumphDataLayer.TriumphLibs.Consumers.GetAllPOSConsumers();
        //        if (dsConsumers != null && dsConsumers.Tables.Count > 0)
        //        {
        //            dsConsumers.Namespace = POSTables._POS_TRIUMPH_NAMESPACE;
        //            dsConsumers.DataSetName = POSTables._POS_TRIUMPH_DB;
        //            dsConsumers.Tables[0].TableName = POSTables._TIN_HUU;
        //            dsConsumers.Tables[0].PrimaryKey = new DataColumn[] { dsConsumers.Tables[0].Columns[0] };
        //            dsConsumers.WriteXml(sFileName, XmlWriteMode.WriteSchema);

        //            return true;
        //        }
        //        return true;
        //    }
        //    catch { }

        //    return false;
        //}
        //public static bool ExportPOSPromotionItems(string sFileName)
        //{
        //    try
        //    {
        //        DataTable dtPromotions = TriumphDataLayer.TriumphLibs.Promotions.LoadAllCurrentPromotion();

        //        if (dtPromotions != null && dtPromotions.Rows.Count > 0)
        //        {
        //            DataTable dt1 = dtPromotions.Copy();
        //            using (System.Data.DataSet ds = new System.Data.DataSet())
        //            {
        //                ds.Tables.Add(dt1);
        //                ds.Tables[0].TableName = POSTables._POS_PROMOTION;
        //                int num = 1;
        //                DataTable dtAdvancePromotionDiscount = TriumphDataLayer.TriumphLibs.Promotions.LoadAdvancePromotionDiscount(GeneralConstants._PROMOTION_TYPE_ADVANCE_DISCOUNT_PACK).ToDataTable();
        //                if (dtAdvancePromotionDiscount != null && dtAdvancePromotionDiscount.Rows.Count > 0)
        //                {
        //                    DataTable dt2 = dtAdvancePromotionDiscount.Copy();
        //                    ds.Tables.Add(dt2);
        //                    ds.Tables[num].TableName = POSTables._POS_PROMOTION_ADVANCEPROMOTION_DISCOUNT;
        //                    num = num + 1;
        //                }

        //                DataTable dtAdvancePromotionAddItem = TriumphDataLayer.TriumphLibs.Promotions.LoadAdvancePromotionAddItem().ToDataTable();
        //                if (dtAdvancePromotionAddItem != null && dtAdvancePromotionAddItem.Rows.Count > 0)
        //                {
        //                    DataTable dt2 = dtAdvancePromotionAddItem.Copy();
        //                    ds.Tables.Add(dt2);
        //                    ds.Tables[num].TableName = POSTables._POS_PROMOTION_ADVANCEPROMOTION_ADDITEM;
        //                    num = num + 1;
        //                }

        //                DataTable dtAdvancePromotionAddAccessory = TriumphDataLayer.TriumphLibs.Promotions.LoadAdvancePromotionAddAccessory().ToDataTable();
        //                if (dtAdvancePromotionAddAccessory != null && dtAdvancePromotionAddAccessory.Rows.Count > 0)
        //                {
        //                    DataTable dt2 = dtAdvancePromotionAddAccessory.Copy();
        //                    ds.Tables.Add(dt2);
        //                    ds.Tables[num].TableName = POSTables._POS_PROMOTION_ADVANCEPROMOTION_ADDACCESSORY;
        //                    num = num + 1;
        //                }

        //                DataTable dtPromotionItems = TriumphDataLayer.TriumphLibs.PromotionDetails.GetAllPOSPromotionItemsTable();
        //                if (dtPromotionItems != null && dtPromotionItems.Rows.Count > 0)
        //                {
        //                    DataTable dt2 = dtPromotionItems.Copy();
        //                    ds.Tables.Add(dt2);
        //                    ds.Tables[num].TableName = POSTables._POS_PROMOTION_ITEM;
        //                    num = num + 1;
        //                }
        //                ds.Namespace = POSTables._POS_TRIUMPH_NAMESPACE;
        //                ds.DataSetName = POSTables._POS_TRIUMPH_DB;
        //                ds.WriteXml(sFileName, XmlWriteMode.WriteSchema);

        //                return true;
        //            }
        //        }
        //        return true;
        //    }
        //    catch { }

        //    return false;
        //}

        //public static bool ImportNewConsumers(string sDirName, string sOutletCode)
        //{
        //    bool bReturn = true;
        //    try
        //    {
        //        string uploadPath = Path.Combine(sDirName, sOutletCode);
        //        if (!Directory.Exists(uploadPath))
        //        {
        //            Directory.CreateDirectory(uploadPath);
        //        }
        //        if (Directory.Exists(uploadPath))
        //        {
        //            //DataTable dtNewConsumers = new DataTable();
        //            DataSet ds = new DataSet();
        //            ds.ReadXml(Path.Combine(uploadPath, TriumphPOS.Data.FileDataManager._POS_NEW_CONSUMERS));
        //            if (ds == null || ds.Tables.Count <= 0) return false;

        //            DataTable dtNewConsumers = ds.Tables[0];
        //            //dtNewConsumers.ReadXml(Path.Combine(uploadPath, TriumphPOS.Data.FileDataManager._POS_NEW_CONSUMERS));
        //            if (dtNewConsumers != null && dtNewConsumers.Rows.Count > 0)
        //            {
        //                ConsumerCollection consumers = new ConsumerCollection();
        //                int iRowCount = dtNewConsumers.Rows.Count;
        //                for (int i = 0; i < iRowCount; i++)
        //                {
        //                    try
        //                    {
        //                        DataRow dr = dtNewConsumers.Rows[i];
        //                        string sCONSUMERCODE = dr["CONSUMERCODE"].ToString().Trim();
        //                        if (string.IsNullOrEmpty(sCONSUMERCODE)) continue;

        //                        // Get and Save a row of table CONSUMER
        //                        Triumph.SubSonicLibs.Consumer objConsumer = new Triumph.SubSonicLibs.Consumer(Triumph.SubSonicLibs.Consumer.ConsumercodeColumn.ToString(), sCONSUMERCODE);
        //                        if (objConsumer != null && objConsumer.Id > 0)
        //                        {
        //                            if (dr["REGISTERDATE"] != null && !string.IsNullOrEmpty(dr["REGISTERDATE"].ToString())) { objConsumer.Registerdate = DateTime.Parse(dr["REGISTERDATE"].ToString()); }
        //                            if (dr["FIRSTNAME"] != null && !string.IsNullOrEmpty(dr["FIRSTNAME"].ToString())) { objConsumer.Firstname = dr["FIRSTNAME"].ToString(); }
        //                            if (dr["LASTNAME"] != null && !string.IsNullOrEmpty(dr["LASTNAME"].ToString())) { objConsumer.Lastname = dr["LASTNAME"].ToString(); }
        //                            if (dr["GENDER"] != null && !string.IsNullOrEmpty(dr["GENDER"].ToString())) { objConsumer.Gender = int.Parse(dr["GENDER"].ToString()); }
        //                            if (dr["REGION"] != null && !string.IsNullOrEmpty(dr["REGION"].ToString())) { objConsumer.Region = int.Parse(dr["REGION"].ToString()); }
        //                            if (dr["IDCARDNO"] != null && !string.IsNullOrEmpty(dr["IDCARDNO"].ToString())) { objConsumer.Idcardno = dr["IDCARDNO"].ToString().Trim(); }
        //                            if (dr["IDCARDDATE"] != null && !string.IsNullOrEmpty(dr["IDCARDDATE"].ToString())) { objConsumer.Idcarddate = DateTime.Parse(dr["IDCARDDATE"].ToString()); }
        //                            if (dr["IDCARDPLACE"] != null && !string.IsNullOrEmpty(dr["IDCARDPLACE"].ToString())) { objConsumer.Idcardplace = dr["IDCARDPLACE"].ToString(); }
        //                            if (dr["STATUS"] != null && !string.IsNullOrEmpty(dr["STATUS"].ToString())) { objConsumer.Status = int.Parse(dr["STATUS"].ToString()); }
        //                            if (dr["STREETNO"] != null && !string.IsNullOrEmpty(dr["STREETNO"].ToString())) { objConsumer.Streetno = dr["STREETNO"].ToString(); }
        //                            if (dr["STREET"] != null && !string.IsNullOrEmpty(dr["STREET"].ToString())) { objConsumer.Street = dr["STREET"].ToString(); }
        //                            if (dr["WARD"] != null && !string.IsNullOrEmpty(dr["WARD"].ToString())) { objConsumer.Ward = dr["WARD"].ToString(); }
        //                            if (dr["DISTRICT"] != null && !string.IsNullOrEmpty(dr["DISTRICT"].ToString())) { objConsumer.District = dr["DISTRICT"].ToString(); }
        //                            if (dr["CITY"] != null && !string.IsNullOrEmpty(dr["CITY"].ToString())) { objConsumer.City = dr["CITY"].ToString(); }
        //                            if (dr["PROVINCE"] != null && !string.IsNullOrEmpty(dr["PROVINCE"].ToString())) { objConsumer.Province = dr["PROVINCE"].ToString(); }
        //                            if (dr["TELEPHONE"] != null && !string.IsNullOrEmpty(dr["TELEPHONE"].ToString())) { objConsumer.Telephone = dr["TELEPHONE"].ToString(); }
        //                            if (dr["EMAIL"] != null && !string.IsNullOrEmpty(dr["EMAIL"].ToString())) { objConsumer.Email = dr["EMAIL"].ToString(); }
        //                            if (dr["REMARK"] != null && !string.IsNullOrEmpty(dr["REMARK"].ToString())) { objConsumer.Remark = dr["REMARK"].ToString(); }
        //                            if (dr["DATECREATED"] != null && !string.IsNullOrEmpty(dr["DATECREATED"].ToString())) { objConsumer.Datecreated = DateTime.Parse(dr["DATECREATED"].ToString()); }
        //                            if (dr["UPDATEBY"] != null && !string.IsNullOrEmpty(dr["UPDATEBY"].ToString())) { objConsumer.Updateby = int.Parse(dr["UPDATEBY"].ToString()); }
        //                            //if (dr["ISNEW"] != null && !string.IsNullOrEmpty(dr["ISNEW"].ToString())) { objConsumer.IsNew = bool.Parse(dr["ISNEW"].ToString()); }
        //                            objConsumer.Isnewest = true;

        //                            objConsumer.Save();
        //                            consumers.Add(objConsumer);
        //                        }
        //                    }
        //                    catch
        //                    {
        //                        bReturn = false;
        //                    }
        //                }

        //                if (consumers != null && consumers.Count > 0)
        //                {
        //                    //consumers.SaveAll();
        //                    bReturn = true;
        //                }
        //            }
        //            else
        //            {
        //                bReturn = true;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        bReturn = false;
        //    }

        //    return bReturn;
        //}
    }
}