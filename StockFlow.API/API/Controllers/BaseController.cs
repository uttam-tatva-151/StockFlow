using StockFlow.API.FilterAttributes;
using Microsoft.AspNetCore.Mvc;

namespace StockFlow.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[ModelStateValidator]

public class BaseController : ControllerBase
{

}