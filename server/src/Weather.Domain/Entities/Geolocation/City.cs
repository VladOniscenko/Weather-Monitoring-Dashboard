using System.ComponentModel.DataAnnotations;

namespace Weather.Domain.Entities;

public class City : BaseEntity
{
    public Guid CountryId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public long Population { get; set; }

    [MaxLength(50)]
    public string Timezone { get; set; } = string.Empty;
}
