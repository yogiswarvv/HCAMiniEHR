using HCAMiniEHR.DTOs;
using HCAMiniEHR.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HCAMiniEHR.Pages.LabOrders;

public class IndexModel : PageModel
{
    private readonly LabOrderService _labOrderService;

    public IndexModel(LabOrderService labOrderService)
    {
        _labOrderService = labOrderService;
    }

    // DTO-based property (ONLY ONE)
    public List<LabOrderListDto> LabOrders { get; set; } = new();

    public async Task OnGetAsync()
    {
        LabOrders = await _labOrderService.GetLabOrderListAsync();
    }
}
