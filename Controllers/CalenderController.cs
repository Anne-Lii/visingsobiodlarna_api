using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace visingsobiodlarna_backend.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize] //Användaren måste vara inloggad för att använda denna controller
public class CalenderController : ControllerBase
{
    
}