using System;
using System.Collections.Generic;
using System.Text;

namespace Problem_1.Models
{
    public class Track: Car
    {
        public Track()
            : base()
        {

        }

        public int Tonnage { get; set; }

        // if-else
        public override decimal Price(int Year)
        {
            if (Year <= 5)
            {
                return Value;
            }
            else if (Year > 5 && Tonnage <= 5)
            {
                return Value * 0.3m;
            }
            else if (Year > 5 && Tonnage > 5 && Tonnage <= 10)
            {
                return Value * 0.6m;
            }
            else
            {
                return Value * 0.8m;
            }
        }

        //ternary oparator
        //public override decimal Price(int Year)  
        //    => Year <= 5 ? Value : 
        //    (Year > 5 && Tonnage <= 5) ? Value * 0.3m : 
        //    (Year > 5 && Tonnage > 5 && Tonnage <= 10) ? Value* 0.6m : 
        //    Value* 0.8m;

        public override string ToString()
        {
            return $"{Brand}: {Mileage} km, {Price(DateTime.Now.Year - Year):f2}";
        }

    }
}
