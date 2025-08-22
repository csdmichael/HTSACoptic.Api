using Microsoft.AspNetCore.Mvc;
using HTSA.Repositories;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using HTSA.Models;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Controllers
{
    //[Authorize]
    //[Route("api/[controller]")]
    public class CarController : Controller
    {
        ICarRepository _CarRepository;
        readonly ILogger<CarController> _logger;

        public CarController(ILogger<CarController> logger, ICarRepository CarRepository)
        {
            _CarRepository = CarRepository;
            _logger = logger;
        }
        
        //----------------------------------------------------------
        //-- Michael Yaacoub - 10/7/2017
        //-- csdmichael@gmail.com
        //----------------------------------------------------------

        [HttpGet]
        public string GetCars(string token, string churchID)
        {
           
            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _CarRepository.GetCars(baseURL, token, churchID);
            return JsonConvert.SerializeObject(result);
            
        }

        [HttpGet]
        public string GetLookups(string token, string churchID)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _CarRepository.GetLookups(baseURL, token, churchID);
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public string ActivateCar([FromBody]ActivateCarResponse acr)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _CarRepository.CarsActivate(baseURL, acr.token, acr.carID);
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public string DeActivateCar([FromBody]ActivateCarResponse acr)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _CarRepository.CarsDeActivate(baseURL, acr.token, acr.carID);
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public string AddCar([FromBody] Car car)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _CarRepository.AddCar(baseURL, car.Token, car.ChurchID, car.PersonID, car.MakeID, car.StyleID, car.ColorID, car.ModelName, car.PlateNo, car.StateLicensed, car.CountryLicensed);
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public string UpdateCar([FromBody] Car car)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _CarRepository.UpdateCar(baseURL, car.Token, car.CarID.ToString(), car.ChurchID, car.PersonID, car.MakeID, car.StyleID, car.ColorID, car.ModelName, car.PlateNo, car.StateLicensed, car.CountryLicensed);
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public string AddDriver([FromBody] Car car)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _CarRepository.AddDriver(baseURL, car.Token, car.CarID.ToString(), car.PersonID);
            return JsonConvert.SerializeObject(result);

        }

        [HttpPost]
        public string RemoveDriver([FromBody] Car car)
        {

            string baseURL = Request.Scheme + "://" + Request.Host.Value;

            var result = _CarRepository.RemoveDriver(baseURL, car.Token, car.CarID.ToString(), car.PersonID);
            return JsonConvert.SerializeObject(result);

        }
    }
}
