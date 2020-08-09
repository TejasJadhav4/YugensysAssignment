using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YugensysAssignment.Interface;
using YugensysAssignment.Models;

namespace YugensysAssignment.Implementation
{
    public class HomeService : IHomeService
    {
        String cs = ConfigurationManager.ConnectionStrings["getcon"].ConnectionString;
        public void SaveDetail(fruitdetailModel model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "SaveDetail";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("fruitId", SqlDbType.Int);
                    cmd.Parameters["fruitId"].Value = model.fruitId;

                    cmd.Parameters.Add("sProperty", SqlDbType.NVarChar);
                    cmd.Parameters["sProperty"].Value = model.sProperty;

                    con.Open();
                    int insertedRows = cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                throw (ex);
            }
            
        }

        public List<CustomList> GetSuggestedDropdDownData(String searchText)
        {
            try
            {
                List<CustomList> lst = GetDropdDownData();
                if (!string.IsNullOrEmpty(searchText))
                {
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        SqlDataAdapter da = new SqlDataAdapter();
                        StringBuilder sbd = new StringBuilder();
                        sbd.Append(" SELECT DISTINCT TOP 2 f.Id Value");
                        sbd.Append(" 	,f.sName TEXT");
                        sbd.Append(" FROM fruitdetail fd");
                        sbd.Append(" INNER JOIN fruits f ON f.Id = fd.fruitId");
                        sbd.AppendFormat(" WHERE fd.sProperty LIKE '%{0}%'", searchText);
                        sbd.Append(" GROUP by f.Id , f.sName");
                        da.SelectCommand = new SqlCommand(sbd.ToString(), con);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds.Tables.Count > 0)
                        {
                            DataTable dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                List<CustomList> lstSuggested = new List<CustomList>();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    lstSuggested.Add(new CustomList { Value = Convert.ToInt32(dr["Value"]), Text = Convert.ToString(dr["Text"]), Type = "Suggested", index = 1 });
                                }
                                if (lstSuggested.Count > 0)
                                {
                                    foreach (CustomList item in lstSuggested)
                                    {
                                        lst.Remove(lst.Where(x => x.Value == item.Value).First());
                                    }
                                }
                                lst.AddRange(lstSuggested);
                                lst.OrderBy(x => x.index);
                            }
                        }
                    }
                }
                return lst;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<CustomList> GetDropdDownData()
        {
            try
            {
                List<CustomList> lst = new List<CustomList>();
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = new SqlCommand("Select Id Value, sName Text from fruits Order by sName", con);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                lst.Add(new CustomList { Value = Convert.ToInt32(dr["Value"]), Text = Convert.ToString(dr["Text"]), Type = "Other Options", index = 2 });
                            }
                        }
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public List<SelectListItem> GetSelectListItems(String searchText)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            List<String> lstGroup = new List<String>();
            lstGroup.Add("Suggested");
            lstGroup.Add("Other Options");
            List<CustomList> GetData = GetSuggestedDropdDownData(searchText);
            foreach (String item in lstGroup)
            {
                SelectListGroup group = new SelectListGroup() { Name = item };
                foreach (CustomList member in GetData.Where(v => v.Type == item))
                {
                    lst.Add(new SelectListItem
                    {
                        Value = member.Value.ToString(),
                        Text = member.Text,
                        Group = group
                    });
                }
            }
            return lst;
        }
    }
}