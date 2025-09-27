using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTLVN.QLTLH.Code.ChiHoi
{
    public class FileDataManager
    {
        // File directory
        public const string _POS_UPLOAD_PATH = "xml\\pos\\in";
        public const string _POS_DOWNLOAD_PATH = "xml\\pos\\out";
        public const string _POS_GENERAL_DOWNLOAD_PATH = "xml\\pos\\out\\general";
        public const string _POS_TEMP_PATH = "xml\\pos\\temp";

        // File Name
        public const string _POS_USER = "POSUsers.xml";
        public const string _POS_OUTLETS = "POSOutlets.xml";
        public const string _POS_ITEMS = "Items.xml";
        public const string _POS_CONSUMERS = "POSConsumers.xml";
        public const string _POS_NEW_CONSUMERS = "POSNewConsumers.xml";
        public const string _POS_PROMOTION_ITEMS = "POSPromotionItems.xml";
        public const string _POS_BILLS = "POSBills.xml";
        public const string _POS_BILL_ITEMS = "POSBillItems.xml";
        public const string _POS_INTERNALTRANSFERS = "POSInternalTransfers.xml";
        public const string _POS_ITITEM = "POSITItem.xml";
        public const string _POS_OUTLET_ITEMS_IN_STORE = "POSItems.xml";
        public const string _POS_PROMOTIONS = "POSPromotions.xml";
        public const string _POS_STYLES = "Styles.xml";
        public const string _POS_WHITEM = "WarehouseItems.xml";

        // File Type
        public const int _POS_TYPE_NONE = 0;
        public const int _POS_TYPE_NEW_CONSUMERS = 1;
        public const int _POS_TYPE_BILLS = 2;
        public const int _POS_TYPE_BILL_ITEMS = 3;
        public const int _POS_TYPE_INTERNALTRANSFERS = 4;
        public const int _POS_TYPE_ITITEM = 5;
        public const int _POS_TYPE_MONTH_END = 6;
        public const int _POS_TYPE_CONSUMERS = 7;
        public const int _POS_TYPE_PROMOTIONS = 8;
    }
}