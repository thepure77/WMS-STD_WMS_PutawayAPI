using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PutawayDataAccess.Models
{
    public partial class View_InsertPutaway
    {
        [Key]
        public long RowIndex { get; set; }

        [StringLength(50)]
        public string isSku { get; set; }

        [StringLength(50)]
        public string Tag_No { get; set; }
        [StringLength(50)]
        public string TaskGR_No { get; set; }
        
        [StringLength(200)]
        public string Suggest_Location_Name { get; set; }

        public Guid? GoodsReceive_Index { get; set; }
        public Guid? Tag_Index { get; set; }


        
        [StringLength(200)]
        public string confirm_Location_Name { get; set; }

        [StringLength(200)]
        public string location_Name { get; set; }


        [StringLength(3)]
        public string operations { get; set; }

        public Guid? confirm_Location_Index { get; set; }

        [StringLength(50)]
        public string confirm_Location_Id { get; set; }


        [StringLength(4)]
        public string create_By { get; set; }
    }
}
