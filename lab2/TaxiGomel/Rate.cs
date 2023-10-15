using System;
using System.Collections.Generic;

namespace TaxiGomel;

public partial class Rate
{
    public int RateId { get; set; }

    public string? RateDescription { get; set; }

    public decimal? RatePrice { get; set; }

    public virtual ICollection<Call> Calls { get; set; } = new List<Call>();
}
