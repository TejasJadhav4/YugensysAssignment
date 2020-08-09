using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using YugensysAssignment.Models;

namespace YugensysAssignment.Interface
{
    public interface IHomeService
    {
        void SaveDetail(fruitdetailModel model);
        List<SelectListItem> GetSelectListItems(String searchText);
    }
}
