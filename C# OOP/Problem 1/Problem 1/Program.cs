using Problem_1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Problem_1
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<string> info = System.IO.File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\data.txt")).ToList();
            List<string> result = new List<string>();
            string[] carAddInfo = new string[2];
            string[] trackAddInfo = new string[3];

            for (int i = 0; i < info.Count; i++)
            {
                if (info[i] == "1")
                {
                    Car car = new Car();
                    car.Brand = info[i + 1];
                    carAddInfo = info[i + 2].Split();
                    car.Year = int.Parse(carAddInfo[0]);
                    car.Mileage = int.Parse(carAddInfo[1]);
                    car.Value = decimal.Parse(carAddInfo[2]);
                    result.Add(car.ToString());
                }
                else if (info[i] == "2")
                {
                    Track track = new Track();
                    track.Brand = info[i + 1];
                    trackAddInfo = info[i + 2].Split();
                    track.Year = int.Parse(trackAddInfo[0]);
                    track.Mileage = int.Parse(trackAddInfo[1]);
                    track.Value = decimal.Parse(trackAddInfo[2]);
                    track.Tonnage = int.Parse(trackAddInfo[3]);
                    result.Add(track.ToString());
                }
            }

            result.Reverse();

            using (StreamWriter writer = new StreamWriter(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\result.txt"), true))
            {
                foreach (var vehicle in result)
                {
                    writer.WriteLine(vehicle);
                }
                writer.Close();
            }
        }    
    }
}
