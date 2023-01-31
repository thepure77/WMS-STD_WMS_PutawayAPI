using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace putawayBusiness.ViewModels
{

    public class ReportPutawayGRViewModel
    {

        public Guid? goodsReceive_Index { get; set; }

        public string goodsReceive_No { get; set; }

        public string goodsReceive_Date { get; set; }

        public string owner_Id { get; set; }

        public string owner_Name { get; set; }
        
        public string product_Id { get; set; }

        public string product_Name { get; set; }

        public string productConversion_Id { get; set; }

        public string productConversion_Name { get; set; }

        public decimal? qty { get; set; }

        public string tag_No { get; set; }

        public string location_Id { get; set; }

        public string location_Name { get; set; }

        public string itemStatus_Id { get; set; }

        public string itemStatus_Name { get; set; }

        public int? RowCount { get; set; }
    }
}
