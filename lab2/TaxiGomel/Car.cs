using System;
using System.Collections.Generic;

namespace TaxiGomel;

public partial class Car
{
    public int CarId { get; set; }

    public string? RegistrationNumber { get; set; }

    public int? ModelId { get; set; }

    public string? CarcaseNumber { get; set; }

    public string? EngineNumber { get; set; }

    public DateTime? ReleaseYear { get; set; }

    public int? Mileage { get; set; }

    public int? DriverId { get; set; }

    public DateTime? LastTi { get; set; }

    public int? MechanicId { get; set; }

    public string? SpecialMarks { get; set; }

    public virtual ICollection<Call> Calls { get; set; } = new List<Call>();

    public virtual Employee? Driver { get; set; }

    public virtual Employee? Mechanic { get; set; }

    public virtual CarModel? Model { get; set; }
}
