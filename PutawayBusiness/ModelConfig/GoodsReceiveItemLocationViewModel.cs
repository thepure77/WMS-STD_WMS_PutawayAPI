using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRBusiness.GoodsReceive
{
    
    public partial class GoodsReceiveItemLocationViewModel
    {
        [Key]
        public Guid goodsReceiveItemLocation_Index { get; set; }

        public Guid goodsReceive_Index { get; set; }

        public Guid goodsReceiveItem_Index { get; set; }

        public Guid? tagItem_Index { get; set; }

        public Guid? tag_Index { get; set; }

        [StringLength(50)]
        public string tag_No { get; set; }

        public Guid? product_Index { get; set; }

        [Required]
        [StringLength(50)]
        public string product_Id { get; set; }

        [Required]
        [StringLength(200)]
        public string product_Name { get; set; }

        [StringLength(200)]
        public string product_SecondName { get; set; }

        [StringLength(200)]
        public string product_ThirdName { get; set; }

        [StringLength(50)]
        public string product_Lot { get; set; }

        public Guid? itemStatus_Index { get; set; }

        [Required]
        [StringLength(50)]
        public string itemStatus_Id { get; set; }

        [Required]
        [StringLength(200)]
        public string itemStatus_Name { get; set; }

        public Guid? productConversion_Index { get; set; }

        [Required]
        [StringLength(50)]
        public string productConversion_Id { get; set; }

        [Required]
        [StringLength(200)]
        public string productConversion_Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? mfg_Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? exp_Date { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? unitWeight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? weight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? unitWidth { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? unitLength { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? unitHeight { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? unitVolume { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? volume { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? unitPrice { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? price { get; set; }

        public Guid? owner_Index { get; set; }

        [Required]
        [StringLength(50)]
        public string owner_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string owner_Name { get; set; }

        public Guid? location_Index { get; set; }

        [StringLength(50)]
        public string location_Id { get; set; }

        [StringLength(200)]
        public string location_Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? qty { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ratio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal totalQty { get; set; }

        [StringLength(200)]
        public string udf_1 { get; set; }

        [StringLength(200)]
        public string udf_2 { get; set; }

        [StringLength(200)]
        public string udf_3 { get; set; }

        [StringLength(200)]
        public string udf_4 { get; set; }

        [StringLength(200)]
        public string udf_5 { get; set; }

        [StringLength(200)]
        public string create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? create_Date { get; set; }

        [StringLength(200)]
        public string update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? update_Date { get; set; }

        [StringLength(200)]
        public string cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? cancel_Date { get; set; }

        public int? putaway_Status { get; set; }

        [StringLength(200)]
        public string putaway_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? putaway_Date { get; set; }

        public Guid? suggest_Location_Index { get; set; }

    }
}
