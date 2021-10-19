using NUnit.Framework;
using System;
using food_trucks.Services.Util;
namespace test_food_trucks
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test1()
        {
            var records = GetInitalFoodTrucks.ReadFromCSV("./resources/Mobile_Food_Facility_Permit.csv");
            Console.WriteLine($"Num of records is {records.Count}");
            Assert.AreEqual(records.Count, 619); // The test file has 619 entries
            records.ForEach(item => Console.WriteLine(item));
            Console.WriteLine("Hello from test1");
            Assert.Pass();
        }

        [Test]
        public void ScratchPad()
        {
            Util.Expirement1();
            Console.WriteLine("Done");
        }
    }
}