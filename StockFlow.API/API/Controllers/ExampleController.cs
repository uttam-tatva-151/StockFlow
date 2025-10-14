using StockFlow.Common.Models;
using StockFlow.service.DTOs.UserAuth;
using StockFlow.Service.DTOs.UserAuth;
using StockFlow.Service.Interfaces;
using StockFlow.Service.Record;
using Microsoft.AspNetCore.Mvc;
using static StockFlow.Common.Constants.Constant;

namespace StockFlow.API.Controllers;

public class ExampleController(IUserAuthService UserAuthService) : BaseController

{
    private readonly IUserAuthService _UserAuthService = UserAuthService;
    // [HttpPost("search")]
    // public async Task<IActionResult> Search(UserAuthSearchDTO dto, CancellationToken cancellationToken)
    // {
    //     PageResponseRecord<UserAuthDTO> data = await _UserAuthService.Search(dto, cancellationToken);
    //     return Ok(new ApiResponse()
    //     {
    //         Data = data,
    //     });
    // }

    // [HttpGet("{id:int:min(1)}")]
    // public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    // {
    //     UserAuthDTO item = await _UserAuthService.GetById(id, cancellationToken);
    //     return Ok(new ApiResponse()
    //     {
    //         Data = item,
    //     });
    // }

    // [HttpPost]
    // public async Task<IActionResult> Post(UserAuthDTO dto, CancellationToken cancellationToken)
    // {
    //     await _UserAuthService.Post(dto, cancellationToken);
    //     return Ok(new ApiResponse()
    //     {
    //         Message = string.Format(SuccessMessages.AddedSuccessfully, "Item")
    //     });
    // }


    // [HttpPut]
    // public async Task<IActionResult> Update(UserAuthDTO dto, CancellationToken cancellationToken)
    // {
    //     await _UserAuthService.Update(dto, cancellationToken);
    //     return Ok(new ApiResponse()
    //     {
    //         Message = string.Format(SuccessMessages.UpdatedSuccessfully, "Item")
    //     });
    // }


    // [HttpDelete("{id:int:min(1)}")]
    // public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    // {

    //     await _UserAuthService.Delete(id, cancellationToken);
    //     return Ok(new ApiResponse()
    //     {
    //         Message = string.Format(SuccessMessages.DeletedSuccessfully, "Item")
    //     });
    // }
}
