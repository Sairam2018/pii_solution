using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PII.Base.BO
{
    public class QRCodeB64imgUrls
    {
        public int invID;
        public string imgb64;
    }
    public class VehicleSearchBO
    {
        public int bofId { get; set; }
        public int QacId { get; set; }
        public int UsersId { get; set; }
        public int status { get; set; }
        public int inventoryId { get; set; }
        public string VIN { get; set; }
        public string year { get; set; }
        public string make { get; set; }
        public string family { get; set; }
        public string model { get; set; }
        public string price { get; set; }
        public string mileage { get; set; }
        public string Color { get; set; }
        public string StockType { get; set; }
        public string ImageUrl { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public int pageSelect { get; set; }
        public int isDelete { get; set; }
        public int isFavorite { get; set; }

        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string EMail { get; set; }
        public string PdfPath { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }

        public int CustomerSessionId { get; set; }
        public int StageFlag { get; set; }
        public int isInventory { get; set; }
        public int HDID { get; set; }
        public string Guid { get; set; }
    }

    public class PagingBO
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sortKey { get; set; }
        public string sortDir { get; set; }
        public string errMsg { get; set; }
        public int RecordCount { get; set; }
        public int RecFrom { get; set; }
        public int RecTo { get; set; }
        public bool ShowNext { get; set; }
        public bool ShowPrev { get; set; }
        public int NextPage { get; set; }
        public int PrevPage { get; set; }
       
    }
}
