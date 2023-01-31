using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace putawayBusiness.ViewModels
{

    public class BinBalanceViewModel
    {
        [Key]
        public Guid binBalance_Index { get; set; }

        public Guid owner_Index { get; set; }

        public string owner_Id { get; set; }



        public string owner_Name { get; set; }

        public Guid location_Index { get; set; }


        public string location_Id { get; set; }


        public string location_Name { get; set; }

        public Guid goodsReceive_Index { get; set; }



        public string goodsReceive_No { get; set; }

        public DateTime goodsReceive_Date { get; set; }

        public Guid goodsReceiveItem_Index { get; set; }

        public Guid goodsReceiveItemLocation_Index { get; set; }

        public Guid tagItem_Index { get; set; }

        public Guid tag_Index { get; set; }


        public string tag_No { get; set; }

        public Guid product_Index { get; set; }



        public string product_Id { get; set; }



        public string product_Name { get; set; }


        public string product_SecondName { get; set; }


        public string product_ThirdName { get; set; }


        public string product_Lot { get; set; }

        public Guid itemStatus_Index { get; set; }



        public string itemStatus_Id { get; set; }



        public string itemStatus_Name { get; set; }


        public DateTime? goodsReceive_MFG_Date { get; set; }


        public DateTime? goodsReceive_EXP_Date { get; set; }

        public Guid goodsReceive_ProductConversion_Index { get; set; }



        public string goodsReceive_ProductConversion_Id { get; set; }



        public string goodsReceive_ProductConversion_Name { get; set; }


        public decimal? binBalance_Ratio { get; set; }


        public decimal? binBalance_QtyBegin { get; set; }


        public decimal? binBalance_WeightBegin { get; set; }


        public decimal? binBalance_VolumeBegin { get; set; }


        public decimal? binBalance_QtyBal { get; set; }


        public decimal? binBalance_WeightBal { get; set; }


        public decimal? binBalance_VolumeBal { get; set; }


        public decimal? binBalance_QtyReserve { get; set; }


        public decimal? binBalance_WeightReserve { get; set; }


        public decimal? binBalance_VolumeReserve { get; set; }

        public Guid? productConversion_Index { get; set; }



        public string productConversion_Id { get; set; }



        public string productConversion_Name { get; set; }


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


        public string isUse { get; set; }

        public int? binBalance_Status { get; set; }
    }
}
