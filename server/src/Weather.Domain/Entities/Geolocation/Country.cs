using System.ComponentModel.DataAnnotations;

namespace Weather.Domain.Entities;

public class Country : BaseEntity
{
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2)]
    public string CCA2 { get; set; } = string.Empty;

    [MaxLength(3)]
    public string CCA3 { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Region { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Subregion { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Capital { get; set; } = string.Empty;

    [MaxLength(10)]
    public string Flag { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool Independent { get; set; }
    public bool Landlocked { get; set; }
}