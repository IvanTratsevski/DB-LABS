using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Diagnostics.Metrics;
using System.Linq;

namespace TaxiGomel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (TaxiGomelContext db = new TaxiGomelContext())
            {
                Select(db);
                Insert(db);
                Delete(db);
                Update(db);
            }

            static void Print(string sqltext, IEnumerable items)
            {
                Console.WriteLine(sqltext);
                Console.WriteLine("Записи: ");
                foreach (var item in items)
                {
                    Console.WriteLine(item.ToString());
                }
                Console.WriteLine();
                Console.ReadKey();
            }
            static void Select(TaxiGomelContext db)
            {


                var carModelsLINQ = from cm in db.CarModels
                                    select new
                                    {
                                        Модель = cm.ModelName,
                                        Характеристики = cm.TechStats,
                                        Цена = cm.Price,
                                    };
                string comment1 = "1. Результат выполнения запроса на выборку всех данных из таблицы CarModels: \r\n";

                Print(comment1, carModelsLINQ.ToList());

                var carModelsSortLINQ = from cm in db.CarModels
                                        where (cm.Price > 70)
                                        select new
                                        {
                                            Модель = cm.ModelName,
                                            Характеристики = cm.TechStats,
                                            Цена = cm.Price,
                                        };
                string comment2 = "2. Результат выполнения запроса на выборку с условием из таблицы Cars: \r\n";
                Print(comment2, carModelsSortLINQ.ToList());

                var groupCatsLINQ = from c in db.Cars
                                 group c.Mileage by c.DriverId into gr
                                 select new
                                 {
                                     Водитель = gr.Key,
                                     Мили = gr.Sum(),
                                 };
                string comment3 = "3. Результат выполнения запроса на выборку с группировкой данных из таблицы Cars: \r\n";
                Print(comment3, groupCatsLINQ.ToList());

                var multiSelectLINQ = from c in db.Cars
                                      join m in db.CarModels
                                      on c.ModelId equals m.ModelId
                                    select new
                                    {
                                        Регистрационный_номер = c.RegistrationNumber,
                                        Номер_двигателя = c.EngineNumber,
                                        Модель = m.ModelName,
                                    };
                string comment4 = "4. Результат выполнения запроса к нескольким таблицам (Cars, CarsModels): \r\n";
                Print(comment4, multiSelectLINQ.ToList());

                var salaryEmployeeLINQ = from e in db.Employees
                                      join p in db.Positions
                                      on e.PositionId equals p.PositionId
                                      where (p.Salary > 30)
                                      select new
                                      {
                                          ФИО = e.FirstName + " " + e.LastName,
                                          Зарплата = p.Salary
                                      };
                string comment5 = "5. Результат выполнения запроса к нескольким таблицам с отбором (Employees, Positions): \r\n";
                Print(comment5, salaryEmployeeLINQ.ToList());
            };
            static void Insert(TaxiGomelContext db)
            {
                CarModel newCarModel = new CarModel
                {
                    ModelName = "ThisModelMadeViaFramework",
                    TechStats = "AbstractStats",
                    Price = 999,
                    Specifications = "Specifications 123"

                };
                db.CarModels.Add(newCarModel);
                db.SaveChanges();
                Car newCar = new Car
                {
                    RegistrationNumber = "842FE425",
                    ModelId = newCarModel.ModelId,
                    CarcaseNumber = "7742GH32-23",
                    EngineNumber = "97VHH24-123F",
                    ReleaseYear = new DateTime(2014, 9, 10),
                    Mileage = 999,
                    DriverId = 18,
                    LastTi = new DateTime(2022,11,6),
                    MechanicId = 19,
                    SpecialMarks = "No marks",
                };

                db.Cars.Add(newCar);
                db.SaveChanges();

            }
            static void Delete(TaxiGomelContext db)
            {
                int delPosition = 76;
                var pos = db.Positions.Where(p => p.PositionId == delPosition);

                var delEmployees = db.Employees
                    .Include("Position")
                    .Where(e => e.PositionId == delPosition);

                db.Employees.RemoveRange(delEmployees);
                db.SaveChanges();

                db.Positions.RemoveRange(pos);

                db.SaveChanges();

            }
            static void Update(TaxiGomelContext db)
            {

                var carsOutOfOrder = db.Cars.Where(c => (c.Mileage > 10000));
                if (carsOutOfOrder != null)
                {
                    foreach (var c in carsOutOfOrder)
                    {
                        c.SpecialMarks = "Old";
                    };


                }
                db.SaveChanges();

            }
        }
        }
    }


