using PlanGIBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace putawayBusiness.ViewModels
{

    public class View_TaskGRViewModel : Pagination
    {
        public Guid? taskGR_Index { get; set; }
        public string taskGR_No { get; set; }
        public string ref_Document_No { get; set; }

        public Guid? ref_Document_Index { get; set; }
        public string userAssign { get; set; }
        public string create_By { get; set; }
        public string create_Date { get; set; }
        public string key { get; set; }
        public int? update { get; set; }

    }



    public class actionResultViewModel
    {
        public IList<View_TaskGRViewModel> itemsTaskPutaway { get; set; }
        public Pagination pagination { get; set; }
        public bool msg { get; set; }

    }

}
