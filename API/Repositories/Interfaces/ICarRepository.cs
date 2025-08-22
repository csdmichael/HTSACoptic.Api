using HTSA.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public interface ICarRepository
    {
        
        CarsResponse GetCars(string baseURL, string token, string churchID);
        UpdateResponse AddCar(string baseURL, string token, string churchID, string personID, string makeID, string styleID, string colorID, string modelName, string plateNo, string stateLicensed, string countryLicensed);
        CarLookupsResponse GetLookups(string baseURL, string token, string churchID);
        UpdateResponse CarsActivate(string baseURL, string token, string carID);
        UpdateResponse CarsDeActivate(string baseURL, string token, string carID);
        UpdateResponse UpdateCar(string baseURL, string token, string carID, string churchID, string personID, string makeID, string styleID, string colorID, string modelName, string plateNo, string stateLicensed, string countryLicensed);
        UpdateResponse AddDriver(string baseURL, string token, string carID, string personID);
        UpdateResponse RemoveDriver(string baseURL, string token, string carID, string personID);
    }
}