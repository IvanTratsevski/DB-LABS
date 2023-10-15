using System;
using System.Collections.Generic;

namespace TaxiGomel;

public partial class CarModel
{
    public int ModelId { get; set; }

    public string? ModelName { get; set; }

    public string? TechStats { get; set; }

    public decimal? Price { get; set; }

    public string? Specifications { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
