using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PII.Base.LogHelper;
using PII.Base.BO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Configuration;

namespace PII.Base.Bll
{
    public class Users
    {        
        SqlDatabase db = new SqlDatabase(ConfigurationManager.ConnectionStrings["PII_DEV_DB"].ConnectionString);

        public DataSet ValidateLoginUser(string userName, byte[] password, int dealerId,
            int Userid, int RoleId, out string errorMessage)
        {
            errorMessage = string.Empty;
            DataSet userPermissions = new DataSet();
            string spName = string.Empty;          
            spName = "Core.ValidateUser";           
            DbCommand command = db.GetStoredProcCommand(spName);          

            try
            {
                db.AddInParameter(command, "@userName", SqlDbType.VarChar, userName);
                //db.AddInParameter(command, "@passwordHash", SqlDbType.VarChar, Security.EncDec.Encrypt(password));
                db.AddInParameter(command, "@passwordHash", SqlDbType.VarBinary, password);
                db.AddInParameter(command, "@dealerId", SqlDbType.Int, dealerId);
                db.AddInParameter(command, "@UserId", SqlDbType.Int, Userid);
                db.AddInParameter(command, "@RoleId", SqlDbType.Int, RoleId); 
                db.AddOutParameter(command, "@errorMsg", SqlDbType.VarChar, 250);

                userPermissions = db.ExecuteDataSet(command);
                object errMsg = db.GetParameterValue(command, "@errorMsg");

                if (errMsg != null && errMsg.ToString() != "")
                {
                    errorMessage = errMsg.ToString();
                }
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return userPermissions;
        }
       
        //Added by venu 
        public int InsertLoginDetails(int userId, string UserName, int dealerId, string AccessedFrom)
        {
            int result = 0;
            if (userId != 0 && userId != null)            {
                DbCommand command = db.GetStoredProcCommand("Core.InsertLoginDetails");
                try
                {
                    db.AddInParameter(command, "@UserId", SqlDbType.Int, userId);
                    db.AddInParameter(command, "@UserName", SqlDbType.VarChar, UserName);
                    db.AddInParameter(command, "@DealerId", SqlDbType.Int, dealerId);
                    db.AddInParameter(command, "@AccessedFrom", SqlDbType.VarChar, AccessedFrom); 
                    result = Convert.ToInt32(db.ExecuteScalar(command));
                }
                catch (Exception ex)
                {
                    new Log().LogError(ex.Message, ex.StackTrace);
                }
            }
            return result;
        }

       
     
        public int CreateUser(UserInfo user, out int userId, int CreatedBy, bool update = false, string checkedModules = "")
        {

            int result = 0;
            userId = 0;
            //int status = 1;
            DbCommand command;
            if (update == true)
            {
                //status = 0;
                command = db.GetStoredProcCommand("Core.UpdateUser");
                db.AddInParameter(command, "@userId", SqlDbType.VarChar, user.userid);
                db.AddInParameter(command, "@createdBy", SqlDbType.VarChar, CreatedBy);
                db.AddInParameter(command, "@checkedModules", SqlDbType.VarChar, checkedModules);

            }
            else
            {
                //status = 1;
                command = db.GetStoredProcCommand("Core.CreateUser");
                db.AddOutParameter(command, "@userId", SqlDbType.Int, userId);
                db.AddInParameter(command, "@createdBy", SqlDbType.VarChar, CreatedBy);
                db.AddInParameter(command, "@checkedModules", SqlDbType.VarChar, checkedModules);
            }
            try
            {
                db.AddInParameter(command, "@UserName", SqlDbType.VarChar, user.Username);
                db.AddInParameter(command, "@Password", SqlDbType.VarChar, user.Password);
                db.AddInParameter(command, "@FirstName", SqlDbType.VarChar, user.FirstName);
                db.AddInParameter(command, "@LastName", SqlDbType.VarChar, user.LastName);

                db.AddInParameter(command, "@PhoneNumber", SqlDbType.VarChar, user.PhoneNumber);
                db.AddInParameter(command, "@Email", SqlDbType.VarChar, user.Email);
                db.AddInParameter(command, "@Address", SqlDbType.VarChar, user.Address);
                db.AddInParameter(command, "@City", SqlDbType.VarChar, user.City);

                db.AddInParameter(command, "@StateCode", SqlDbType.VarChar, user.StateCode);
                db.AddInParameter(command, "@Zip", SqlDbType.VarChar, user.Zip);
                db.AddInParameter(command, "@IsActive", SqlDbType.VarChar, user.IsActive);
                //db.AddInParameter(command, "@IsActive", SqlDbType.VarChar, status);


                db.AddInParameter(command, "@passwordHash", SqlDbType.VarChar, Security.EncDec.Encrypt(user.Password));
                db.AddInParameter(command, "@RoleId", SqlDbType.Int, user.Roleid);
                db.AddInParameter(command, "@dealerId", SqlDbType.Int, user.DealershipId);
                db.AddInParameter(command, "@CompanyId", SqlDbType.Int, user.CompanyId);
                db.AddInParameter(command, "@GroupId", SqlDbType.Int, user.GroupId);

                result = db.ExecuteNonQuery(command);
                if (update != true)
                {
                    userId = Convert.ToInt32(db.GetParameterValue(command, "@userId"));
                }
                else
                    userId = user.userid;

            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return result;
        }
        public int CreateCompany(CompanyInfo company, bool update = false)
        {
            int result = 0;
            DbCommand command;
            if (update == false)
            {
                command = db.GetStoredProcCommand("Core.CreateCompany");
            }
            else
            {
                command = db.GetStoredProcCommand("Core.UpdateCompany");
                db.AddInParameter(command, "@companyId", SqlDbType.Int, company.CompanyId);
            }
            try
            {
                db.AddInParameter(command, "@CompanyName", SqlDbType.VarChar, company.CompanyName);
                db.AddInParameter(command, "@LegalName", SqlDbType.VarChar, company.LegalName);
                db.AddInParameter(command, "@Address1", SqlDbType.VarChar, company.Address1);
                db.AddInParameter(command, "@Address2", SqlDbType.VarChar, company.Address2);

                db.AddInParameter(command, "@StateID", SqlDbType.Int, company.StateId);
                db.AddInParameter(command, "@City", SqlDbType.VarChar, company.City);
                db.AddInParameter(command, "@zip", SqlDbType.VarChar, company.ZipCode);


                result = db.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return result;
        }
        
        public int CheckGroupAvailability(string groupName)
        {
            DbCommand getUserPermissions = db.GetStoredProcCommand("Core.checkGroupAvailability");
            int exists = 0;
            try
            {
                db.AddInParameter(getUserPermissions, "@GroupName", SqlDbType.VarChar, groupName);
                exists = Convert.ToInt32(db.ExecuteScalar(getUserPermissions));
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return exists;
        }

        public int CheckCompanyAvailability(string companyName)
        {
            DbCommand getUserPermissions = db.GetStoredProcCommand("Core.checkCompanyAvailability");
            int exists = 0;
            try
            {
                db.AddInParameter(getUserPermissions, "@CompanyName", SqlDbType.VarChar, companyName);
                exists = Convert.ToInt32(db.ExecuteScalar(getUserPermissions));
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return exists;
        }

        public int CheckDealershipAvailability(string dealershipName)
        {
            DbCommand getUserPermissions = db.GetStoredProcCommand("checkDlrshipAvailability");
            int exists = 0;
            try
            {
                db.AddInParameter(getUserPermissions, "@dlrshipName", SqlDbType.VarChar, dealershipName);
                exists = Convert.ToInt32(db.ExecuteScalar(getUserPermissions));
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return exists;
        }
        public int checkusernameAvailability(string username)
        {
            DbCommand getUserPermissions = db.GetStoredProcCommand("Core.CheckusernameAvailability");
            int exists = 0;
            try
            {
                db.AddInParameter(getUserPermissions, "@username", SqlDbType.VarChar, username);
                exists = Convert.ToInt32(db.ExecuteScalar(getUserPermissions));
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return exists;
        }

        public int CreateGroup(GroupInfo group)
        {
            int result = 0;
            DbCommand command = db.GetStoredProcCommand("Core.CreateGroup");
            try
            {
                db.AddInParameter(command, "@GroupName", SqlDbType.VarChar, group.GroupName);
                db.AddInParameter(command, "@GroupDescription", SqlDbType.VarChar, group.GroupDescription);
                db.AddInParameter(command, "@ParentID", SqlDbType.Int, group.ParentID);
                db.AddInParameter(command, "@LevelID", SqlDbType.Int, group.LevelID);

                db.AddInParameter(command, "@CompanyId", SqlDbType.Int, group.CompanyId);
                db.AddInParameter(command, "@IsActive", SqlDbType.Bit, group.IsActive);



                result = db.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return result;
        }
        public int UpdateGroup(GroupInfo group)
        {
            int result = 0;
            DbCommand command = db.GetStoredProcCommand("Core.UpdateGroup");
            try
            {
                db.AddInParameter(command, "@GroupName", SqlDbType.VarChar, group.GroupName);
                db.AddInParameter(command, "@GroupDescription", SqlDbType.VarChar, group.GroupDescription);
                db.AddInParameter(command, "@groupId", SqlDbType.Int, group.groupId);

                result = db.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return result;
        }
        public int CreateDealership(DealerInfo dealership)
        {
            int result = 0;
            DbCommand command = db.GetStoredProcCommand("Core.CreateDealership");
            try
            {
                db.AddInParameter(command, "@DealerShipName", SqlDbType.VarChar, dealership.DealerShipName);
                db.AddInParameter(command, "@LegalName", SqlDbType.VarChar, dealership.LegalName);
                db.AddInParameter(command, "@Address", SqlDbType.VarChar, dealership.Address);
                db.AddInParameter(command, "@Zip", SqlDbType.VarChar, dealership.Zip);
                db.AddInParameter(command, "@StateId", SqlDbType.VarChar, dealership.StateID);
                db.AddInParameter(command, "@City", SqlDbType.VarChar, dealership.City);

                db.AddInParameter(command, "@Phone", SqlDbType.VarChar, dealership.Phone);
                db.AddInParameter(command, "@Email", SqlDbType.VarChar, dealership.Email);
                db.AddInParameter(command, "@Fax", SqlDbType.VarChar, dealership.Fax);
                db.AddInParameter(command, "@CompanyId", SqlDbType.Int, dealership.CompanyId);
                db.AddInParameter(command, "@GroupId", SqlDbType.Int, dealership.GroupID);
                db.AddInParameter(command, "@IsActive", SqlDbType.Bit, dealership.IsActive);

                db.AddInParameter(command, "@Website", SqlDbType.VarChar, dealership.Website);
                db.AddInParameter(command, "@DealerFinanceLicense", SqlDbType.VarChar, dealership.DealerFinanceLicense);
                db.AddInParameter(command, "@ServiceScheduleUrl", SqlDbType.VarChar, dealership.ServiceSheduleUrl);
                db.AddInParameter(command, "@DefaultUsers", SqlDbType.Int, dealership.DefaultUsers);
                db.AddInParameter(command, "@TimeZone", SqlDbType.VarChar, dealership.TimeZone);
                db.AddInParameter(command, "@DealerInfo", SqlDbType.VarChar, dealership.DealerOtherInfo);

                result = db.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return result;
        }

       
        public DataSet GetUsers(out int result, int StartIndex = 0, int pagesize = 10, int id = 0, DateTime? fromdate = null, DateTime? todate = null,
            int isActive = 1, int dealerid = 1, int UserId = 0, string Search = "", string sortKey = "UserName",
            string sortDir = "ASC", int edituserroleId = 1, int reqRoleId = 0)
        {

            result = 0;
            int currentrow = ((StartIndex - 1) * pagesize) + 1;
            DataSet ds = new DataSet();
            DbCommand command;
            try
            {
                if (id == 0)
                {
                    //command = db.GetStoredProcCommand("GetUsers_v1");
                    command = db.GetStoredProcCommand("Core.GetUsersList");
                    //db.AddInParameter(command, "@StartIndex", SqlDbType.Int, currentrow);
                    db.AddInParameter(command, "@StartIndex", SqlDbType.Int, StartIndex);
                    db.AddInParameter(command, "@PageSize", SqlDbType.Int, pagesize);
                    db.AddInParameter(command, "@fromdate", SqlDbType.Date, fromdate);
                    db.AddInParameter(command, "@todate", SqlDbType.Date, todate);
                    db.AddInParameter(command, "@isActive", SqlDbType.Int, isActive);
                    db.AddInParameter(command, "@DealerId", SqlDbType.Int, dealerid);
                    db.AddInParameter(command, "@UserId", SqlDbType.Int, UserId);
                    db.AddOutParameter(command, "@TotalCount", SqlDbType.Int, result);
                    db.AddInParameter(command, "@SearchVal", SqlDbType.VarChar, Search);
                    db.AddInParameter(command, "@sortKey", SqlDbType.VarChar, sortKey);
                    db.AddInParameter(command, "@sortDir", SqlDbType.VarChar, sortDir);
                    db.AddInParameter(command, "@edituserroleId", SqlDbType.Int, edituserroleId);
                    ds = db.ExecuteDataSet(command);
                    result = Convert.ToInt32(db.GetParameterValue(command, "@TotalCount"));

                }
                else
                {
                    command = db.GetStoredProcCommand("Core.GetUserdetails");
                    db.AddInParameter(command, "@Userid", SqlDbType.VarChar, id);
                    db.AddInParameter(command, "@DealerId", SqlDbType.VarChar, dealerid);
                    db.AddInParameter(command, "@edituserroleId", SqlDbType.Int, edituserroleId);
                    db.AddInParameter(command, "@reqRoleId", SqlDbType.Int, reqRoleId);
                    ds = db.ExecuteDataSet(command);
                }
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }
              
       
        public int DeactivateDlrship(int id = 0)
        {
            int result = 0;
            DbCommand command;
            try
            {
                command = db.GetStoredProcCommand("Core.ActDeactDealership");
                db.AddInParameter(command, "@DealershipId", SqlDbType.VarChar, id);
                result = db.ExecuteNonQuery(command);

            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return result;
        }
       
        
        public DataSet GetCompanies(out int result, int startpage = 1, int pagesize = 10, int id = 0)
        {
            result = 0;
            //result2 = 0;
            int currentrow = ((startpage - 1) * pagesize) + 1;
            DataSet ds = new DataSet();
            DbCommand command;
            try
            {
                if (id == 0)
                {
                    command = db.GetStoredProcCommand("Core.GetCompanies");
                    db.AddInParameter(command, "@StartIndex", SqlDbType.Int, currentrow);
                    db.AddInParameter(command, "@PageSize", SqlDbType.Int, pagesize);
                    db.AddOutParameter(command, "@TotalCount", SqlDbType.Int, result);
                    ds = db.ExecuteDataSet(command);
                    result = Convert.ToInt32(db.GetParameterValue(command, "@TotalCount"));
                }
                else
                {
                    command = db.GetStoredProcCommand("Core.GetCompanyDetails");
                    db.AddInParameter(command, "@companyid", SqlDbType.Int, id);
                    ds = db.ExecuteDataSet(command);

                }
                //ds = db.ExecuteDataSet(command);
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetCompaniesByName(out int result, int startpage = 1, int pagesize = 10, int id = 0, string Search = "")
        {
            result = 0;
            //result2 = 0;
            int currentrow = ((startpage - 1) * pagesize) + 1;
            DataSet ds = new DataSet();
            DbCommand command;
            try
            {
                if (id == 0)
                {
                    command = db.GetStoredProcCommand("Core.GetCompaniesByName");
                    db.AddInParameter(command, "@StartIndex", SqlDbType.Int, currentrow);
                    db.AddInParameter(command, "@PageSize", SqlDbType.Int, pagesize);
                    db.AddOutParameter(command, "@TotalCount", SqlDbType.Int, result);
                    db.AddInParameter(command, "@Search", SqlDbType.VarChar, Search);
                    ds = db.ExecuteDataSet(command);
                    result = Convert.ToInt32(db.GetParameterValue(command, "@TotalCount"));
                }
                else
                {
                    command = db.GetStoredProcCommand("Core.GetCompanyDetails");
                    db.AddInParameter(command, "@companyid", SqlDbType.Int, id);
                    ds = db.ExecuteDataSet(command);

                }
                //ds = db.ExecuteDataSet(command);
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }




        public DataSet GetGroups(out int result, out int result2, int startpage = 1, int pagesize = 10, int companyId = 0, int alldetils = 0)
        {
            result = 0;
            result2 = 0;
            int currentrow = ((startpage - 1) * pagesize) + 1;
            DataSet ds = new DataSet();
            DbCommand command = db.GetStoredProcCommand("Core.GetGroups");
            if (alldetils == 1)
            {
                db.AddInParameter(command, "@alldetails", SqlDbType.Int, alldetils);
            }

            try
            {
                db.AddInParameter(command, "@StartIndex", SqlDbType.Int, currentrow);
                db.AddInParameter(command, "@PageSize", SqlDbType.Int, pagesize);
                db.AddInParameter(command, "@companyId", SqlDbType.Int, companyId);
                db.AddOutParameter(command, "@TotalCount1", SqlDbType.Int, result);
                db.AddOutParameter(command, "@TotalCount2", SqlDbType.Int, result2);
                ds = db.ExecuteDataSet(command);
                //result = Convert.ToInt32(db.GetParameterValue(command, "@TotalCount1"));
                 result = String.IsNullOrEmpty(db.GetParameterValue(command, "@TotalCount1").ToString()) ? 0 : Convert.ToInt32(db.GetParameterValue(command, "@TotalCount1"));
                result2 = String.IsNullOrEmpty(db.GetParameterValue(command, "@TotalCount2").ToString()) ? 0: Convert.ToInt32(db.GetParameterValue(command, "@TotalCount2"));

            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }


        public DataSet GetChildGroups(out int result, int groupId, int level = 1, int pagesize = 10, int startpage = 1)
        {
            result = 0;
            int currentrow = ((startpage - 1) * pagesize) + 1;
            DataSet ds = new DataSet();
            DbCommand command = db.GetStoredProcCommand("Core.getChildGroups");

            try
            {
                db.AddInParameter(command, "@StartIndex", SqlDbType.Int, currentrow);
                db.AddInParameter(command, "@PageSize", SqlDbType.Int, pagesize);
                db.AddInParameter(command, "@GroupId", SqlDbType.Int, groupId);
                db.AddInParameter(command, "@Levelid", SqlDbType.Int, level);
                db.AddOutParameter(command, "@TotalCount", SqlDbType.Int, result);
                ds = db.ExecuteDataSet(command);
                result = Convert.ToInt32(db.GetParameterValue(command, "@TotalCount"));


            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }


        public DataSet GetDealerships(out int result, out int result2, int startpage = 1, int pagesize = 10, int groupid = 0, int companyId = 0)
        {
            result = 0;
            result2 = 0;
            int currentrow = ((startpage - 1) * pagesize) + 1;
            DataSet ds = new DataSet();
            DbCommand command = db.GetStoredProcCommand("Core.GetDealerships");
            if (companyId != 0)
            {
                db.AddInParameter(command, "@companyId", SqlDbType.Int, companyId);
            }
            try
            {
                db.AddInParameter(command, "@StartIndex", SqlDbType.Int, currentrow);
                db.AddInParameter(command, "@PageSize", SqlDbType.Int, pagesize);
                db.AddInParameter(command, "@groupId", SqlDbType.Int, groupid);
                db.AddOutParameter(command, "@TotalCount1", SqlDbType.Int, result);
                db.AddOutParameter(command, "@TotalCount2", SqlDbType.Int, result2);
                ds = db.ExecuteDataSet(command);
                result = Convert.ToInt32(db.GetParameterValue(command, "@TotalCount1"));
                //result2 = Convert.ToInt32(db.GetParameterValue(command, "@TotalCount2"));
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }


        public DataSet GetDealersByName(out int result, out int result2, int startpage = 1, int pagesize = 10, int groupid = 0, int companyId = 0, string Search = "")
        {
            result = 0;
            result2 = 0;
            int currentrow = ((startpage - 1) * pagesize) + 1;
            DataSet ds = new DataSet();
            DbCommand command = db.GetStoredProcCommand("Core.GetdealersByName");
            if (companyId != 0)
            {
                db.AddInParameter(command, "@companyId", SqlDbType.Int, companyId);
            }
            try
            {
                db.AddInParameter(command, "@StartIndex", SqlDbType.Int, currentrow);
                db.AddInParameter(command, "@PageSize", SqlDbType.Int, pagesize);
                db.AddInParameter(command, "@groupId", SqlDbType.Int, groupid);
                db.AddOutParameter(command, "@TotalCount1", SqlDbType.Int, result);
                db.AddOutParameter(command, "@TotalCount2", SqlDbType.Int, result2);
                db.AddInParameter(command, "@Search", SqlDbType.VarChar, Search);
                ds = db.ExecuteDataSet(command);
                result = Convert.ToInt32(db.GetParameterValue(command, "@TotalCount1"));
                //result2 = Convert.ToInt32(db.GetParameterValue(command, "@TotalCount2"));
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }


        public DataSet GetCompanyNgroupIds(int dealershipId = 0)
        {
            DataSet ds = new DataSet();
            DbCommand command = db.GetStoredProcCommand("Core.GetCompanyNgroupId");

            try
            {
                db.AddInParameter(command, "@dealershipId", SqlDbType.Int, dealershipId);
                ds = db.ExecuteDataSet(command);

            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }
        public DataSet ChangeDealershipsStatus(int groupid, int companyId = 0)
        {
            DataSet ds = new DataSet();
            DbCommand command = db.GetStoredProcCommand("Core.GetDealerships");
            if (companyId != 0)
            {
                db.AddInParameter(command, "@companyId", SqlDbType.Int, companyId);
            }
            try
            {
                db.AddInParameter(command, "@groupId", SqlDbType.Int, groupid);
                ds = db.ExecuteDataSet(command);

            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }
        public DataSet GetStates()
        {
            DataSet ds = new DataSet();
            DbCommand command = db.GetStoredProcCommand("Core.GetStates");
            try
            {

                ds = db.ExecuteDataSet(command);

            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }

        public DataSet GetGroupsByCmpny(int companyId)
        {
            DataSet ds = new DataSet();
            DbCommand command = db.GetStoredProcCommand("Core.GetGroupsByCompany");
            try
            {
                db.AddInParameter(command, "@CompanyId", SqlDbType.VarChar, companyId);
                ds = db.ExecuteDataSet(command);

            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }
        public DataSet GetDealersByGroup(int GroupId)
        {
            DataSet ds = new DataSet();
            DbCommand command = db.GetStoredProcCommand("Core.GetDealersByGroup");
            try
            {
                db.AddInParameter(command, "@GroupId", SqlDbType.VarChar, GroupId);
                ds = db.ExecuteDataSet(command);

            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }
       

        //sathyasai added this code to maintain logout status
        public int DoLogout(string username)
        {
            DbCommand command = db.GetStoredProcCommand("Core.DoLogout");
            int result = 0;
            try
            {
                db.AddInParameter(command, "@username", SqlDbType.VarChar, username.Trim());

                result = Convert.ToInt32(db.ExecuteScalar(command));
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return result;
        }


        //sathyasai wrote this code regarding update password
        public int upadtenewpassword_edit(string UsrId, byte[] NewPassword)
        {
            int result = 0;
            DbCommand command = db.GetStoredProcCommand("Core.UpdateNewPassword");
            try
            {
                db.AddInParameter(command, "@userId", SqlDbType.Int, UsrId);
                db.AddInParameter(command, "@passwordHash", SqlDbType.VarBinary, NewPassword);
                result = db.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return result;

        }
               

        public DataSet GetLoginUserDetails(string userName)
        {
            DataSet userDetails = new DataSet();
            string spName = string.Empty;
            spName = "Core.GetLoginUserDetails";

            DbCommand command = db.GetStoredProcCommand(spName);

            try
            {
                db.AddInParameter(command, "@userName", SqlDbType.VarChar, userName);

                userDetails = db.ExecuteDataSet(command);
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }

            return userDetails;
        }

        public DataSet GetCategories(int dealerId)
        {
            DataSet ds = new DataSet();
            DbCommand command = db.GetStoredProcCommand("Core.GetCategories");
            try
            {
                db.AddInParameter(command, "@dealerId", SqlDbType.Int, dealerId);
                ds = db.ExecuteDataSet(command);
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return ds;
        }

        /// <summary>
        /// Reset the password hash based on the value in password column in userdetails table
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DataSet GetUserList(int UserId) //**UserId = 0 [For All Users]
        {
            string spName = string.Empty;
            spName = "Core.GetUsersToResetHashes";
            DataSet userList = new DataSet();
            DbCommand command = db.GetStoredProcCommand(spName);
            try
            {
                db.AddInParameter(command, "@UserId", SqlDbType.VarChar, UserId);
                userList = db.ExecuteDataSet(command);
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }

            return userList;
        }

        public int UpdatePasswordHashs(DataTable hashes)
        {
            DbCommand command = db.GetStoredProcCommand("Core.UpdatePasswordHashs");
            int result = 0;
            try
            {
                db.AddInParameter(command, "@hashes", SqlDbType.Structured, hashes);
                result = Convert.ToInt32(db.ExecuteScalar(command));
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return result;
        }

        public int ChangeLoginPassword_New(int userId, byte[] password, string newPassword)
        {
            DbCommand command = db.GetStoredProcCommand("Core.ChangeLoginPassword");
            int result = 0;
            try
            {
                db.AddInParameter(command, "@UserId", SqlDbType.Int, userId);
                db.AddInParameter(command, "@PasswordBytes", SqlDbType.VarBinary, password);
                db.AddInParameter(command, "@newPassword", SqlDbType.VarChar, newPassword);

                result = Convert.ToInt32(db.ExecuteScalar(command));
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return result;
        }
               public int ActivateDeactivateUser(int userid, int status)
        {
            int result = 0;

            DbCommand command;
            try
            {
                command = db.GetStoredProcCommand("Core.ActivateDeactivateUser");
                db.AddInParameter(command, "@userId", SqlDbType.Int, userid);
                db.AddInParameter(command, "@activeStatus", SqlDbType.Int, status);

                result = db.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                new Log().LogError(ex.Message, ex.StackTrace);
            }
            return result;
        }        
    }
}
