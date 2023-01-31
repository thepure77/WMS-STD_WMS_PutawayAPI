using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PutawayDataAccess.Models
{

    public partial class wm_BinBalance
    {
        [Key]
        public Guid BinBalance_Index { get; set; }

        public Guid Owner_Index { get; set; }
                
        public string Owner_Id { get; set; }

        
        
        public string Owner_Name { get; set; }

        public Guid Location_Index { get; set; }

        
        public string Location_Id { get; set; }

        
        public string Location_Name { get; set; }

        public Guid GoodsReceive_Index { get; set; }

        
        
        public string GoodsReceive_No { get; set; }

        public DateTime GoodsReceive_Date { get; set; }

        public Guid GoodsReceiveItem_Index { get; set; }

        public Guid GoodsReceiveItemLocation_Index { get; set; }

        public Guid TagItem_Index { get; set; }

        public Guid Tag_Index { get; set; }

        
        public string Tag_No { get; set; }

        public Guid Product_Index { get; set; }

        
        
        public string Product_Id { get; set; }

        
        
        public string Product_Name { get; set; }

        
        public string Product_SecondName { get; set; }

        
        public string Product_ThirdName { get; set; }

        
        public string Product_Lot { get; set; }

        public Guid ItemStatus_Index { get; set; }

        
        
        public string ItemStatus_Id { get; set; }

        
        
        public string ItemStatus_Name { get; set; }

        
        public DateTime? GoodsReceive_MFG_Date { get; set; }

        
        public DateTime? GoodsReceive_EXP_Date { get; set; }

        public Guid GoodsReceive_ProductConversion_Index { get; set; }

        
        
        public string GoodsReceive_ProductConversion_Id { get; set; }

        
        
        public string GoodsReceive_ProductConversion_Name { get; set; }

        
        public decimal? BinBalance_Ratio { get; set; }

        
        public decimal? BinBalance_QtyBegin { get; set; }

        
        public decimal? BinBalance_WeightBegin { get; set; }

        
        public decimal? BinBalance_VolumeBegin { get; set; }

        
        public decimal? BinBalance_QtyBal { get; set; }

        
        public decimal? BinBalance_WeightBal { get; set; }

        
        public decimal? BinBalance_VolumeBal { get; set; }

        
        public decimal? BinBalance_QtyReserve { get; set; }

        
        public decimal? BinBalance_WeightReserve { get; set; }

        
        public decimal? BinBalance_VolumeReserve { get; set; }

        public Guid? ProductConversion_Index { get; set; }

        
        
        public string ProductConversion_Id { get; set; }

        
        
        public string ProductConversion_Name { get; set; }

        
        public string UDF_1 { get; set; }

        
        public string UDF_2 { get; set; }

        
        public string UDF_3 { get; set; }

        
        public string UDF_4 { get; set; }

        
        public string UDF_5 { get; set; }

        
        public string Create_By { get; set; }

        public DateTime? Create_Date { get; set; }

        
        public string Update_By { get; set; }

        public DateTime? Update_Date { get; set; }

        
        public string Cancel_By { get; set; }

        public DateTime? Cancel_Date { get; set; }

        
        public string IsUse { get; set; }

        public int? BinBalance_Status { get; set; }
    }
}
