using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using HTSA.Models;
using System;
using TokenAuth.Repositories;
using TokenAuth.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;
using HTSA.API.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public class ChurchRepository : IChurchRepository
    {
        ILogger _logger;
        ApplContext _context;
        IAuthRepository _authRepository;
        INotifRepository _notifRepository;
        IPushRepository _pushRepository;

        public ChurchRepository(ApplContext context, ILogger<ChurchRepository> logger, IAuthRepository authRepository, INotifRepository notifRepository, IPushRepository pushRepository)
        {
            _logger = logger;
            _context = context;
            _authRepository = authRepository;
            _notifRepository = notifRepository;
            _pushRepository = pushRepository;
        }

       
        public ChurchesResponse GetChurches(string baseURL, string token, string churchID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            ChurchesResponse churchesResp = new ChurchesResponse();
            var lstChurches = new List<Church>();

            try
            {
                string sqlQuery;

                sqlQuery = @"exec CHRCH.[SP_Churches_Get]";


                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    Church church = new Church();

                    church.ChurchID = rdr.DbDataReader["ChurchID"].ToString();
                    church.ChurchName = rdr.DbDataReader["ChurchName"].ToString();
                    church.CAddress = rdr.DbDataReader["CAddress"].ToString();
                    church.CountryID = rdr.DbDataReader["CountryID"].ToString();
                    church.CityID = rdr.DbDataReader["CityID"].ToString();
                    church.CityName = rdr.DbDataReader["CityName"].ToString();
                    church.ZipCode = rdr.DbDataReader["ZipCode"].ToString();
                    church.StateID = rdr.DbDataReader["StateID"].ToString();
                    church.LocLatitude = rdr.DbDataReader["LocLatitude"].ToString();
                    church.LocLongitude = rdr.DbDataReader["LocLongitude"].ToString();
                    church.ChurchInfoHtml = rdr.DbDataReader["ChurchInfoHtml"].ToString();

                    lstChurches.Add(church);
                }
                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='ChurchRepository.GetChurches'");
            }
            churchesResp.ChurchesList = lstChurches;
            churchesResp.ChurchesListCount = lstChurches.Count;
            return churchesResp;
        }

        public PersonChurchesResponse GetPersonChurches(string baseURL, string PersonID)

        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PersonChurchesResponse personChurchesResp = new PersonChurchesResponse();
            var lstPersonChurches = new List<PersonChurch>();

            try
            {
                    string sqlQuery;
		            int paramCnt = 0;

                    sqlQuery = @"exec CHRCH.SP_Person_Churches_Get";

		             if (PersonID != null && PersonID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PersonID = '" + PersonID + "'";
                        paramCnt++;
                    }
					
                     /*
					if (ChurchID != null && ChurchID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @ChurchID = '" + ChurchID + "'";
                        paramCnt++;
                    }
					
					if (StatusID != null && StatusID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @StatusID = '" + StatusID + "'";
                        paramCnt++;
                    }
                    */

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    while (rdr.DbDataReader.Read())
                    {
                        PersonChurch pc = new PersonChurch();

						//Read fields from DataReader
						//-----------------------------
                        pc.ChurchID = rdr.DbDataReader["ChurchID"].ToString();
						pc.PersonID = rdr.DbDataReader["PersonID"].ToString();
                        pc.PersonName = rdr.DbDataReader["PersonName"].ToString();
                        pc.JoinTime = rdr.DbDataReader["JoinTime"].ToString();
						pc.StatusID = rdr.DbDataReader["StatusID"].ToString();
						pc.LastModified = rdr.DbDataReader["LastModified"].ToString();
						pc.CreatedDT = rdr.DbDataReader["CreatedDT"].ToString();
                        pc.ChurchName = rdr.DbDataReader["ChurchName"].ToString();
                        pc.CAddress = rdr.DbDataReader["CAddress"].ToString();
                        pc.CountryID = rdr.DbDataReader["CountryID"].ToString();
                        pc.CityID = rdr.DbDataReader["CityID"].ToString();
                        pc.CityName = rdr.DbDataReader["CityName"].ToString();
                        pc.ZipCode = rdr.DbDataReader["ZipCode"].ToString();
                        pc.StateID = rdr.DbDataReader["StateID"].ToString();
                        pc.LocLatitude = rdr.DbDataReader["LocLatitude"].ToString();
                        pc.LocLongitude = rdr.DbDataReader["LocLongitude"].ToString();
                        pc.ChurchInfoHtml = rdr.DbDataReader["ChurchInfoHtml"].ToString();

                    lstPersonChurches.Add(pc);
                    }
                    rdr.DbDataReader.Dispose();
                
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='ChurchRepository.GetPersonChurches'");
            }
            personChurchesResp.PersonChurchesList = lstPersonChurches;
            // personChurchesResp.PersonChurchesListCount = lstPersonChurches.Count;
            return personChurchesResp;
        }

        public UpdateResponse AddPersonChurch(PersonChurch pc)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();
            RelationalDataReader rdr = null;

            try
            {

                if (pc == null)
                {
                    throw new ArgumentNullException(nameof(pc));
                }

                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec  CHRCH.SP_Person_Churches_Add";

                if (pc.PersonID != null && pc.PersonID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PersonID = '" + pc.PersonID + "'";
                    paramCnt++;
                }

                if (pc.ChurchID != null && pc.ChurchID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ChurchID = '" + pc.ChurchID + "'";
                    paramCnt++;
                }


                if (pc.StatusID != null && pc.StatusID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @StatusID = '" + pc.StatusID + "'";
                    paramCnt++;
                }

                rdr = _context.Database.ExecuteSqlQuery(sqlQuery);
                
                updateResp.success = true;

                rdr.DbDataReader.Dispose();

                this.NotifyAdmins(pc);

            }
            catch (Exception ex)
            {
                updateResp.success = false;
                updateResp.value = "You have already submitted another request!";
                // rdr.DbDataReader.Dispose();
                _logger.LogError(ex.Message);
            }

            return updateResp;
        }

        public UpdateResponse UpdatePersonChurch(PersonChurch pc)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();

            try
            {


                if (pc == null)
                {
                    throw new ArgumentNullException(nameof(pc));
                }

                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec  CHRCH.SP_Person_Churches_Update";

                if (pc.PersonID != null && pc.PersonID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PersonID = '" + pc.PersonID + "'";
                    paramCnt++;
                }

                if (pc.ChurchID != null && pc.ChurchID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ChurchID = '" + pc.ChurchID + "'";
                    paramCnt++;
                }


                if (pc.StatusID != null && pc.StatusID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @StatusID = '" + pc.StatusID + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                updateResp.success = true;
                
                rdr.DbDataReader.Dispose();

                this.SendApprovalNotifications(pc);

            }
            catch (Exception ex)
            {
                updateResp.success = false;
                updateResp.value = ex.Message;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PersonRepository.UpdatePersonChurch'");
            }

            return updateResp;
        }

        private void NotifyAdmins(PersonChurch pc)
        {
            PeopleResponse prAppAdmins = this.GetAppAdmins();
            PeopleResponse prChurchAdmins = this.GetChurchAdmins(pc.ChurchID);

            if (prAppAdmins != null && prAppAdmins.PeopleList != null)
            {
                foreach (PeopleListItem per in prAppAdmins.PeopleList)
                {
                    this.SendRequestNotifications(per, pc);
                }
            }

            if (prChurchAdmins != null && prChurchAdmins.PeopleList != null)
            {
                foreach (PeopleListItem per in prChurchAdmins.PeopleList)
                {
                    this.SendRequestNotifications(per, pc);
                }
            }

        }

        private void SendApprovalNotifications(PersonChurch pc)
        {
            // Send Email Notif to member
            // ---------------------------
            EmailModel emailObj = new EmailModel();
            emailObj.To = pc.Email;

            if (pc.StatusID == "ACC")
            {
                emailObj.Subject = "Church Membership Approved";
                emailObj.Body = pc.PersonName + ", Congratulations. Your Church Membership Request has been approved!";
            }
            else if (pc.StatusID == "REJ")
            {
                emailObj.Subject = "Church Membership Rejected";
                emailObj.Body = "Your Church Membership Request has been rejected!";
            }
            // _notifRepository.SendEmail(emailObj);

            // Send Push Notif to member
            //-----------------------------
            _pushRepository.SendTaggedPush("PersonId", pc.PersonID, emailObj.Subject, "", emailObj.Body);

        }

        private void SendRequestNotifications(PeopleListItem per, PersonChurch pc)
        {
            // Send Email Notif to member
            // ---------------------------
            EmailModel emailObj = new EmailModel();
            emailObj.To = per.Email;
            emailObj.From = pc.Email;

            emailObj.Subject = "Church Membership Request";
            emailObj.Body = pc.PersonName + " is requesting to join the church.";
            
            // _notifRepository.SendEmail(emailObj);

            // Send Push Notif to member
            //-----------------------------
            _pushRepository.SendTaggedPush("PersonId", per.PersonId, emailObj.Subject, "", emailObj.Body);

        }

        public async Task<PeopleResponse> GetMembers(string churchId, string statusId, string baseURL)
        {
            PeopleResponse pr = new PeopleResponse();

            try
            {

                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec CHRCH.SP_ChurchMembers_Get";

               
                if (churchId != null && churchId.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ChurchId = '" + churchId + "'";
                    paramCnt++;
                }

                if (statusId != null && statusId.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @StatusId = '" + statusId + "'";
                    paramCnt++;
                }


                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (rdr.DbDataReader.HasRows)
                {
                    while (rdr.DbDataReader.Read())
                    {
                        PeopleListItem newPeopleListItem = new PeopleListItem();

                        newPeopleListItem.PersonId = rdr.DbDataReader["PersonId"].ToString();
                        newPeopleListItem.FirstName = rdr.DbDataReader["FirstName"].ToString();
                        newPeopleListItem.MiddleName = rdr.DbDataReader["MiddleName"].ToString();
                        newPeopleListItem.LastName = rdr.DbDataReader["LastName"].ToString();
                        newPeopleListItem.DisplayName = newPeopleListItem.FirstName + " " + newPeopleListItem.LastName;
                        newPeopleListItem.HasPic = System.Convert.ToInt32(rdr.DbDataReader["HasPic"].ToString());
                        newPeopleListItem.PictureName = rdr.DbDataReader["PictureName"].ToString();

                        pr.PeopleList.Add(newPeopleListItem);
                    }
                }
                else
                {
                    throw new Exception("No records found in database!");
                }

                rdr.DbDataReader.Dispose();

                pr.IsSuccess = true;


            }
            catch (Exception ex)
            {
                pr.IsSuccess = false;
                pr.Message = ex.Message;
            }

            return pr;
        }

        public PeopleResponse GetChurchAdmins(string churchId)
        {
            PeopleResponse pr = new PeopleResponse();

            try
            {

                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec PRIV.SP_ChurchAdmins_Get";


                if (churchId != null && churchId.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ChurchId = '" + churchId + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (rdr.DbDataReader.HasRows)
                {
                    while (rdr.DbDataReader.Read())
                    {
                        PeopleListItem newPeopleListItem = new PeopleListItem();

                        newPeopleListItem.PersonId = rdr.DbDataReader["PersonId"].ToString();
                        newPeopleListItem.FirstName = rdr.DbDataReader["FirstName"].ToString();
                        newPeopleListItem.MiddleName = rdr.DbDataReader["MiddleName"].ToString();
                        newPeopleListItem.LastName = rdr.DbDataReader["LastName"].ToString();
                        newPeopleListItem.DisplayName = newPeopleListItem.FirstName + " " + newPeopleListItem.LastName;
                        newPeopleListItem.Email = rdr.DbDataReader["Email"].ToString();
                        newPeopleListItem.Phone = rdr.DbDataReader["CellPhone"].ToString();

                        pr.PeopleList.Add(newPeopleListItem);
                    }
                }
                else
                {
                    throw new Exception("No records found in database!");
                }

                rdr.DbDataReader.Dispose();

                pr.IsSuccess = true;


            }
            catch (Exception ex)
            {
                pr.IsSuccess = false;
                pr.Message = ex.Message;
            }

            return pr;
        }

        public PeopleResponse GetAppAdmins()
        {
            PeopleResponse pr = new PeopleResponse();

            try
            {

                string sqlQuery;

                sqlQuery = @"exec PRIV.SP_AppAdmins_Get";

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (rdr.DbDataReader.HasRows)
                {
                    while (rdr.DbDataReader.Read())
                    {
                        PeopleListItem newPeopleListItem = new PeopleListItem();

                        newPeopleListItem.PersonId = rdr.DbDataReader["PersonId"].ToString();
                        newPeopleListItem.FirstName = rdr.DbDataReader["FirstName"].ToString();
                        newPeopleListItem.MiddleName = rdr.DbDataReader["MiddleName"].ToString();
                        newPeopleListItem.LastName = rdr.DbDataReader["LastName"].ToString();
                        newPeopleListItem.DisplayName = newPeopleListItem.FirstName + " " + newPeopleListItem.LastName;
                        newPeopleListItem.Email = rdr.DbDataReader["Email"].ToString();
                        newPeopleListItem.Phone = rdr.DbDataReader["CellPhone"].ToString();

                        pr.PeopleList.Add(newPeopleListItem);
                    }
                }
                else
                {
                    throw new Exception("No records found in database!");
                }

                rdr.DbDataReader.Dispose();

                pr.IsSuccess = true;


            }
            catch (Exception ex)
            {
                pr.IsSuccess = false;
                pr.Message = ex.Message;
            }

            return pr;
        }


    }
}
