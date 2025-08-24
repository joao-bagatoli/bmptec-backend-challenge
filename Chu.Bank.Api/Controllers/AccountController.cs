using Chu.Bank.Api.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using static Chu.Bank.Api.Domain.DTOs.AccountDtos;

namespace Chu.Bank.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById(Guid id)
    {
        if (id == Guid.Empty) return BadRequest("Invalid account ID");

        try
        {
            var account = await _accountService.GetByIdAsync(id);
            return Ok(account);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount(AccountRequestDto request, CancellationToken cancellationToken)
    {
        if (request == null) return BadRequest();

        try
        {
            var account = await _accountService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
