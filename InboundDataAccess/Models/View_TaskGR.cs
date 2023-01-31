using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GRDataAccess.Models
{
  

    public partial class View_TaskGR
    {
        [Key]
        public Guid? TaskGR_Index { get; set; }
        public string TaskGR_No { get; set; }
        public string Ref_Document_No { get; set; }

        public Guid? Ref_Document_Index { get; set; }
        public string UserAssign { get; set; }
        public string Create_By { get; set; }
        public DateTime? Create_Date { get; set; }
        public int? Document_Status { get; set; }
        public int? IsScanDockToStaging { get; set; }
        //public string Tag_No { get; set; }
        public string Tag_No { get; set; }




    }
}
