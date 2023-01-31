using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
    public partial class im_GoodsReceive
    {
        [Key]
        public Guid? GoodsReceive_Index { get; set; }

        public Guid Owner_Index { get; set; }

        [StringLength(50)]
        public string Owner_Id { get; set; }

        [StringLength(50)]
        public string Owner_Name { get; set; }

        public Guid DocumentType_Index { get; set; }

        [StringLength(50)]
        public string DocumentType_Id { get; set; }

        [StringLength(200)]
        public string DocumentType_Name { get; set; }

        [StringLength(50)]
        public string GoodsReceive_No { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? GoodsReceive_Date { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? GoodsReceive_Due_Date { get; set; }

        [StringLength(200)]
        public string DocumentRef_No1 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No2 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No3 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No4 { get; set; }

        [StringLength(200)]
        public string DocumentRef_No5 { get; set; }

        public int? Document_Status { get; set; }

        [StringLength(200)]
        public string Document_Remark { get; set; }

        [StringLength(200)]
        public string UDF_1 { get; set; }

        [StringLength(200)]
        public string UDF_2 { get; set; }

        [StringLength(200)]
        public string UDF_3 { get; set; }

        [StringLength(200)]
        public string UDF_4 { get; set; }

        [StringLength(200)]
        public string UDF_5 { get; set; }

        public int? DocumentPriority_Status { get; set; }

        [StringLength(200)]
        public string Create_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Create_Date { get; set; }

        [StringLength(200)]
        public string Update_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Update_Date { get; set; }

        [StringLength(200)]
        public string Cancel_By { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Cancel_Date { get; set; }

        public Guid? Warehouse_Index { get; set; }

        [StringLength(50)]
        public string Warehouse_Id { get; set; }

        [StringLength(200)]
        public string Warehouse_Name { get; set; }

        public Guid? Warehouse_Index_To { get; set; }

        [StringLength(50)]
        public string Warehouse_Id_To { get; set; }

        [StringLength(200)]
        public string Warehouse_Name_To { get; set; }

        public int? Putaway_Status { get; set; }

        public Guid? DockDoor_Index { get; set; }

        [StringLength(50)]
        public string DockDoor_Id { get; set; }

        [StringLength(200)]
        public string DockDoor_Name { get; set; }

        public Guid? VehicleType_Index { get; set; }

        [StringLength(50)]
        public string VehicleType_Id { get; set; }

        [StringLength(200)]
        public string VehicleType_Name { get; set; }

        public Guid? ContainerType_Index { get; set; }

        [StringLength(50)]
        public string ContainerType_Id { get; set; }

        [StringLength(200)]
        public string ContainerType_Name { get; set; }
        public string UserAssign { get; set; }

        public string Invoice_No { get; set; }

        public Guid? Vendor_Index { get; set; }

        [StringLength(50)]
        public string Vendor_Id { get; set; }

        [StringLength(200)]
        public string Vendor_Name { get; set; }

        public Guid? WHOwner_Index { get; set; }

        [StringLength(50)]
        public string WHOwner_Id { get; set; }

        [StringLength(200)]
        public string WHOwner_Name { get; set; }
    }
}
