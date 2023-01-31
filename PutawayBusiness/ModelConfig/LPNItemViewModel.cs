using System;
using System.Collections.Generic;

namespace GRBusiness.LPNItem
{
    public partial class LPNItemViewModel
    {
        public Guid tagItem_Index { get; set; }

        public Guid? tag_Index { get; set; }

        public string tag_No { get; set; }

        public Guid goodsReceive_Index { get; set; }

        public Guid goodsReceiveItem_Index { get; set; }

        public Guid product_Index { get; set; }

        public string product_Id { get; set; }

        public string product_Name { get; set; }

        public string product_SecondName { get; set; }

        public string product_ThirdName { get; set; }

        public string product_Lot { get; set; }

        public Guid itemStatus_Index { get; set; }

        public string itemStatus_Id { get; set; }

        public string itemStatus_Name { get; set; }

        public Guid? suggest_Location_Index { get; set; }

        public string suggest_Location_Id { get; set; }

        public string suggest_Location_Name { get; set; }

        public decimal? qty { get; set; }

        public decimal ratio { get; set; }

        public decimal totalQty { get; set; }

        public Guid productConversion_Index { get; set; }

        public string productConversion_Id { get; set; }

        public string productConversion_Name { get; set; }

        public decimal? weight { get; set; }

        public decimal? volume { get; set; }

        public DateTime? mFG_Date { get; set; }

        public DateTime? eXP_Date { get; set; }

        public string tagRef_No1 { get; set; }

        public string tagRef_No2 { get; set; }

        public string tagRef_No3 { get; set; }

        public string tagRef_No4 { get; set; }

        public string tagRef_No5 { get; set; }

        public int? tag_Status { get; set; }

        public string uDF_1 { get; set; }

        public string uDF_2 { get; set; }

        public string uDF_3 { get; set; }

        public string uDF_4 { get; set; }

        public string uDF_5 { get; set; }

        public string create_By { get; set; }

        public DateTime? create_Date { get; set; }

        public string update_By { get; set; }

        public DateTime? update_Date { get; set; }

        public string cancel_By { get; set; }

        public DateTime? cancel_Date { get; set; }
        public Guid confirm_Location_Index { get; set; }
        public string confirm_Location_Id { get; set; }

        public string confirm_Location_Name { get; set; }
        public bool isSku { get; set; }
        public string taskGR_No { get; set; }
        public string erp_Location { get; set; }


        public List<LPNItemViewModel> listLPNItemViewModel { get; set; }
    }
}
