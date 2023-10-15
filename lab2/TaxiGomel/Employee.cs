using System;
using System.Collections.Generic;

namespace TaxiGomel;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int? Age { get; set; }

    public int? PositionId { get; set; }

    public int? Experience { get; set; }

    public virtual ICollection<Call> Calls { get; set; } = new List<Call>();

    public virtual ICollection<Car> CarDrivers { get; set; } = new List<Car>();

    public virtual ICollection<Car> CarMechanics { get; set; } = new List<Car>();

    public virtual Position? Position { get; set; }
}
