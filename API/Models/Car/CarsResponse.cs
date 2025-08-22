using System.Collections.Generic;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------
namespace HTSA.Models
{
    public class CarsResponse
    {
        public string parentURL { get; set; }
        public int CarsListCount { get; set; }
        public List<Car> CarsList { get; set; }

        

    }


}
