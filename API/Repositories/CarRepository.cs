using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using HTSA.Models;
using System;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public class CarRepository : ICarRepository
    {
        ILogger _logger;
        ApplContext _context;
        ITokenInfoRepository _tokenInfoRepository;

        public CarRepository(ApplContext context, ILogger<CarRepository> logger, ITokenInfoRepository tokenInfoRepository)
        {
            _logger = logger;
            _context = context;
            _tokenInfoRepository = tokenInfoRepository;
        }

        public UpdateResponse AddCar(string baseURL, string token, string churchID, string personID, string makeID, string styleID, string colorID, string modelName, string plateNo, string stateLicensed, string countryLicensed)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();
            var lstCars = new List<Car>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "AddCar", "");
                updateResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    int paramCnt = 0;

                    sqlQuery = @"exec CAR.[SP_Cars_Add]";


                    if (personID != null && personID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PersonID = '" + personID + "'";
                        paramCnt++;
                    }

                    if (makeID != null && makeID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @MakeID = '" + makeID + "'";
                        paramCnt++;
                    }

                    if (styleID != null && styleID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @StyleID = '" + styleID + "'";
                        paramCnt++;
                    }

                    if (colorID != null && colorID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @colorID = '" + colorID + "'";
                        paramCnt++;
                    }

                    if (modelName != null && modelName.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @modelName = '" + modelName + "'";
                        paramCnt++;
                    }

                    if (plateNo != null && plateNo.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @plateNo = '" + plateNo + "'";
                        paramCnt++;
                    }

                    if (stateLicensed != null && stateLicensed.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @stateLicensed = '" + stateLicensed + "'";
                        paramCnt++;
                    }

                    if (countryLicensed != null && countryLicensed.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @countryLicensed = '" + countryLicensed + "'";
                        paramCnt++;
                    }


                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    rdr.DbDataReader.Dispose();

                    updateResp.success = true;
                }
            }
            catch (Exception ex)
            {
                updateResp.success = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='CarRepository.AddCar'");
            }
            return updateResp;
        }

        public UpdateResponse UpdateCar(string baseURL, string token, string carID, string churchID, string personID, string makeID, string styleID, string colorID, string modelName, string plateNo, string stateLicensed, string countryLicensed)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();
            var lstCars = new List<Car>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "UpdateCar", "");
                updateResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    int paramCnt = 0;

                    sqlQuery = @"exec CAR.[SP_Cars_Update]";

                    if (carID != null && carID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @CarID = '" + carID + "'";
                        paramCnt++;
                    }

                    if (personID != null && personID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PersonID = '" + personID + "'";
                        paramCnt++;
                    }

                    if (makeID != null && makeID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @MakeID = '" + makeID + "'";
                        paramCnt++;
                    }

                    if (styleID != null && styleID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @StyleID = '" + styleID + "'";
                        paramCnt++;
                    }

                    if (colorID != null && colorID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @colorID = '" + colorID + "'";
                        paramCnt++;
                    }

                    if (modelName != null && modelName.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @modelName = '" + modelName + "'";
                        paramCnt++;
                    }

                    if (plateNo != null && plateNo.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @plateNo = '" + plateNo + "'";
                        paramCnt++;
                    }

                    if (stateLicensed != null && stateLicensed.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @stateLicensed = '" + stateLicensed + "'";
                        paramCnt++;
                    }

                    if (countryLicensed != null && countryLicensed.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @countryLicensed = '" + countryLicensed + "'";
                        paramCnt++;
                    }


                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    rdr.DbDataReader.Dispose();

                    updateResp.success = true;
                }
            }
            catch (Exception ex)
            {
                updateResp.success = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='CarRepository.UpdateCar'");
            }
            
            return updateResp;
        }


        public UpdateResponse AddDriver(string baseURL, string token, string carID, string personID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();
            var lstCars = new List<Car>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "AddDriver", "");
                updateResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    int paramCnt = 0;

                    sqlQuery = @"exec CAR.[SP_Car_AddDriver]";

                    if (carID != null && carID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @CarID = '" + carID + "'";
                        paramCnt++;
                    }

                    if (personID != null && personID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PersonID = '" + personID + "'";
                        paramCnt++;
                    }

                    

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    rdr.DbDataReader.Dispose();

                    updateResp.success = true;
                }
            }
            catch (Exception ex)
            {
                updateResp.success = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='CarRepository.AddDriver'");
            }

            return updateResp;
        }

        public UpdateResponse RemoveDriver(string baseURL, string token, string carID, string personID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();
            var lstCars = new List<Car>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "RemoveDriver", "");
                updateResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    int paramCnt = 0;

                    sqlQuery = @"exec CAR.[SP_Car_RemoveDriver]";

                    if (carID != null && carID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @CarID = '" + carID + "'";
                        paramCnt++;
                    }

                    if (personID != null && personID.Trim() != "")
                    {
                        if (paramCnt > 0)
                            sqlQuery += ", ";
                        sqlQuery += " @PersonID = '" + personID + "'";
                        paramCnt++;
                    }



                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    rdr.DbDataReader.Dispose();

                    updateResp.success = true;
                }
            }
            catch (Exception ex)
            {
                updateResp.success = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='CarRepository.RemoveDriver'");
            }

            return updateResp;
        }

        public CarsResponse GetCars(string baseURL, string token, string churchID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            CarsResponse carsResp = new CarsResponse();
            var lstCars = new List<Car>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "GetCars", "");
                carsResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec CAR.[SP_Cars_Get]";
                    int paramCnt = 0;

                    if (churchID != null && churchID.Trim() != "")
                    {
                        sqlQuery += " @ChurchID = '" + churchID + "'";
                        paramCnt++;
                    }

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    while (rdr.DbDataReader.Read())
                    {
                        Car car = new Car();

                        car.CarID = System.Convert.ToInt32(rdr.DbDataReader["CarID"]);
                        car.PersonID = rdr.DbDataReader["PersonID"].ToString();
                        car.PersonName = rdr.DbDataReader["PersonName"].ToString();
                        car.MakeName  = rdr.DbDataReader["MakeName"].ToString();
                        car.ColorName  = rdr.DbDataReader["ColorName"].ToString();
                        car.StyleName  = rdr.DbDataReader["StyleName"].ToString();
                        car.ModelName  = rdr.DbDataReader["ModelName"].ToString();
                        car.ColorID  = rdr.DbDataReader["ColorID"].ToString();
                        car.StyleID  = rdr.DbDataReader["StyleID"].ToString();
                        car.MakeID = rdr.DbDataReader["MakeID"].ToString();
                        car.PlateNo  = rdr.DbDataReader["PlateNo"].ToString();
                        car.StateLicensed  = rdr.DbDataReader["StateLicensed"].ToString();
                        car.ChurchID  = rdr.DbDataReader["ChurchID"].ToString();
                        car.StickerNum = rdr.DbDataReader["StickerNum"].ToString();

                        lstCars.Add(car);
                    }
                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='CarRepository.GetCars'");
            }
            carsResp.CarsList = lstCars;
            carsResp.CarsListCount = lstCars.Count;
            return carsResp;
        }

        public UpdateResponse CarsDeActivate(string baseURL, string token, string carID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();
            var lstCars = new List<Car>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "CarsDeActivate", "");
                updateResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec CAR.[SP_Cars_DeActivate]";
                    int paramCnt = 0;

                    if (carID != null && carID.Trim() != "")
                    {
                        sqlQuery += " @carID = '" + carID + "'";
                        paramCnt++;
                    }

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);
                    rdr.DbDataReader.Dispose();

                    updateResp.success = true; ;
                }
            }
            catch (Exception ex)
            {
                updateResp.success = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='CarRepository.CarsDeActivate'");
            }
            return updateResp;
        }

        public UpdateResponse CarsActivate(string baseURL, string token, string carID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            UpdateResponse updateResp = new UpdateResponse();
            var lstCars = new List<Car>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "CarsActivate", "");
                updateResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec CAR.[SP_Cars_Activate]";
                    int paramCnt = 0;

                    if (carID != null && carID.Trim() != "")
                    {
                        sqlQuery += " @carID = '" + carID + "'";
                        paramCnt++;
                    }

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);
                    rdr.DbDataReader.Dispose();
                    updateResp.success = true;
                }
            }
            catch (Exception ex)
            {
                updateResp.success = false;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='CarRepository.CarsActivate'");
            }
            
            return updateResp;
        }


        public CarLookupsResponse GetLookups(string baseURL, string token, string churchID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            CarLookupsResponse carLookupsResp = new CarLookupsResponse();
            var lstCarColor = new List<CarColor>();
            var lstCarMake = new List<CarMake>();
            var lstCarStyle = new List<CarStyle>();
            var lstPeople = new List<Person>();

            try
            {
                AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "GetLookups", "");
                carLookupsResp.authResponse = auth;

                if (auth.success)
                {
                    string sqlQuery;

                    sqlQuery = @"exec CAR.[SP_Cars_GetLookups]";
                    int paramCnt = 0;

                    if (churchID != null && churchID.Trim() != "")
                    {
                        sqlQuery += " @ChurchID = '" + churchID + "'";
                        paramCnt++;
                    }

                    var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                    while (rdr.DbDataReader.Read())
                    {
                        CarColor carColor = new CarColor();

                        carColor.ColorID = System.Convert.ToInt32(rdr.DbDataReader["ColorID"]);
                        carColor.ColorName = rdr.DbDataReader["ColorName"].ToString();
                        lstCarColor.Add(carColor);
                    }
                    rdr.DbDataReader.NextResult();

                    while (rdr.DbDataReader.Read())
                    {
                        CarMake carMake = new CarMake();

                        carMake.MakeID = System.Convert.ToInt32(rdr.DbDataReader["MakeID"]);
                        carMake.MakeName = rdr.DbDataReader["MakeName"].ToString();
                        lstCarMake.Add(carMake);
                    }

                    rdr.DbDataReader.NextResult();

                    while (rdr.DbDataReader.Read())
                    {
                        CarStyle carStyle = new CarStyle();

                        carStyle.StyleID = System.Convert.ToInt32(rdr.DbDataReader["StyleID"]);
                        carStyle.StyleName = rdr.DbDataReader["StyleName"].ToString();
                        lstCarStyle.Add(carStyle);
                    }

                    rdr.DbDataReader.NextResult();

                    while (rdr.DbDataReader.Read())
                    {
                        Person p = new Person();

                        p.PersonID = rdr.DbDataReader["PersonID"].ToString();
                        p.PersonName = rdr.DbDataReader["PersonName"].ToString();
                        lstPeople.Add(p);
                    }

                    rdr.DbDataReader.Dispose();
                }
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='CarRepository.GetLookups'");
            }
            carLookupsResp.CarColorList = lstCarColor;
            carLookupsResp.CarColorListCount = lstCarColor.Count;

            carLookupsResp.CarMakeList = lstCarMake;
            carLookupsResp.CarMakeListCount = lstCarMake.Count;

            carLookupsResp.CarStyleList = lstCarStyle;
            carLookupsResp.CarStyleListCount = lstCarStyle.Count;

            carLookupsResp.PeopleList = lstPeople;
            carLookupsResp.PeopleListCount = lstPeople.Count;

            return carLookupsResp;
        }

    }
}
