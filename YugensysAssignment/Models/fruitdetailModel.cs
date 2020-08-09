using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YugensysAssignment.Models
{
    public class fruitdetailModel
    {
        public int Id { get; set; }

        public int fruitId { get; set; }

        public String sProperty { get; set; }

        public  DateTime dtInsert  { get; set; }

        public List<CustomList> customLists { get; set; }

        public List<SelectListItem> selectList { get; set; }

    }
}