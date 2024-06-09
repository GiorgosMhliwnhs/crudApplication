using Microsoft.AspNetCore.Mvc.Rendering;

public class ProsthikiPeripatouViewModel
{
    public Guid Id { get; set; }
    public string Onoma { get; set; }
    public string Perigrafh { get; set; }
    public double Mhkos { get; set; }
    public string? EikonaUrl { get; set; }
    public Guid DyskoliaId { get; set; }
    public Guid PerioxhId { get; set; }
    public List<SelectListItem> Perioxes { get; set; }
    public List<SelectListItem> Dyskolies { get; set; }
}
