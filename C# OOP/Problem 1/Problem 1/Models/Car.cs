
using System;

namespace Problem_1.Models
{
    public class Car
    {
        public string Brand { get; set; }

        public int Year { get; set; }

        public int Mileage { get; set; }

        public decimal Value { get; set; }

        public virtual decimal Price(int Year)
        {
            if (Year <= 3)
            {
                return Value * 0.8m;
            }
            else if (Year > 3 && Year <= 6)
            {
                return Value * 0.6m;
            }
            else
            {
                return Value * 0.3m;
            }
        }

        public override string ToString()
        {
            return $"{Brand}: {Mileage} km, {Price(DateTime.Now.Year - Year):f2}";

        }

    }
}
