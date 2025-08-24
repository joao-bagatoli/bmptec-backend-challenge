using Chu.Bank.Api.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using static Chu.Bank.Api.Domain.DTOs.TransferDtos;

namespace Chu.Bank.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TransferController : ControllerBase
{
    private readonly ITransferService _transferService;

    public TransferController(ITransferService transferService)
    {
        _transferService = transferService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransferById(Guid id)
    {
        if (id == Guid.Empty) return BadRequest("Invalid transfer ID");

        try
        {
            var transfer = await _transferService.GetByIdAsync(id);
            return Ok(transfer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> ExecuteTransfer(TransferRequestDto request, CancellationToken cancellationToken)
    {
        if (request == null) return BadRequest();

        try
        {
            var transfer = await _transferService.TransferAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetTransferById), new { id = transfer.Id }, transfer);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetTransferReport([FromQuery] Guid accountId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        if (accountId == Guid.Empty) return BadRequest("Invalid account ID");

        if (startDate > endDate) return BadRequest("Start date must be earlier than end date");

        try
        {
            var report = await _transferService.GetTransferReportAsync(accountId, startDate, endDate);
            return Ok(report);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
