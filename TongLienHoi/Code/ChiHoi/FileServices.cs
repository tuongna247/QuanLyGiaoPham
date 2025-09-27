using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTLVN.QLTLH.Code.ChiHoi
{
    public class FileServices
    {
    //    #region Properties

    //private TriumphWebsite.TransferFile _transferFile = new TriumphWebsite.TransferFile();

    //#endregion

    //public FileService () {
    //}

    //[WebMethod(Description = "Web service provides method,return the array of byte")]
    //public byte[] DownloadFile(string fileName, string posCode, int type, bool isDaily)
    //{
    //    string downloadPath = string.Empty;
    //    try
    //    {
    //        if (isDaily)
    //        {
    //            downloadPath = Path.Combine(Server.MapPath("."), TriumphPOS.Data.FileDataManager._POS_DOWNLOAD_PATH, posCode);
    //        }
    //        else
    //        {
    //            downloadPath = Path.Combine(Server.MapPath("."), TriumphPOS.Data.FileDataManager._POS_GENERAL_DOWNLOAD_PATH);
    //        }

    //        if (!Directory.Exists(downloadPath))
    //        {
    //            Directory.CreateDirectory(downloadPath);
    //        }

    //        if (type == FileDataManager._POS_TYPE_CONSUMERS)
    //        {
    //            SynchronizeData.ExportConsumers(Path.Combine(downloadPath, TriumphPOS.Data.FileDataManager._POS_CONSUMERS));
    //        }
    //        //else if (type == FileDataManager._POS_TYPE_INTERNALTRANSFERS)
    //        //{
    //        //    SynchronizeData.ExportPOSInternalTransfer(Path.Combine(downloadPath, TriumphPOS.Data.FileDataManager._POS_INTERNALTRANSFERS), posCode);
    //        //}
    //        else if (type == FileDataManager._POS_TYPE_PROMOTIONS)
    //        {
    //            SynchronizeData.ExportPOSPromotionItems(Path.Combine(downloadPath, TriumphPOS.Data.FileDataManager._POS_PROMOTIONS));
    //        }
    //    }
    //    catch { }

    //    return _transferFile.ReadBinaryFile(downloadPath + "\\", fileName);
    //}


    //[WebMethod(Description = "Web service provides method，if upload file successfully.")]
    //public bool UploadFile(byte[] fs, string fileName, string posCode, int type)
    //{
    //    bool bReturn = false;
    //    try
    //    {
    //        string uploadPath = Path.Combine(Server.MapPath("."), TriumphPOS.Data.FileDataManager._POS_UPLOAD_PATH);
    //        if (!Directory.Exists(uploadPath))
    //        {
    //            Directory.CreateDirectory(uploadPath);
    //        }
    //        if (!Directory.Exists(Path.Combine(uploadPath, posCode)))
    //        {
    //            Directory.CreateDirectory(Path.Combine(uploadPath, posCode));
    //        }

    //        if (type == FileDataManager._POS_TYPE_MONTH_END)
    //        {
    //            string sMonthEndDir = DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
    //            if (!Directory.Exists(Path.Combine(uploadPath, posCode, sMonthEndDir)))
    //            {
    //                Directory.CreateDirectory(Path.Combine(uploadPath, posCode, sMonthEndDir));
    //            }

    //            bReturn = _transferFile.WriteBinarFile(fs, uploadPath + "\\" + posCode + "\\" + sMonthEndDir + "\\", fileName);
    //        }
    //        else if (_transferFile.WriteBinarFile(fs, uploadPath + "\\" + posCode + "\\", fileName))
    //        {
    //            if (type == FileDataManager._POS_TYPE_NEW_CONSUMERS)
    //            {
    //                bReturn = SynchronizeData.ImportNewConsumers(uploadPath, posCode);
    //            }
    //            else if (type == FileDataManager._POS_TYPE_BILLS)
    //            {
    //                bReturn = SynchronizeData.ImportBills(uploadPath, posCode);
    //            }
    //            else if (type == FileDataManager._POS_TYPE_INTERNALTRANSFERS)
    //            {
    //                bReturn = SynchronizeData.ImportInternalTransfers(uploadPath, posCode);
    //            } 
    //        }
    //    }
    //    catch { }

    //    return bReturn;
    //}
    }
}