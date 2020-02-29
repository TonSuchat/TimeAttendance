using Microsoft.AspNetCore.Mvc;
using TimeAttendance.Models;

namespace TimeAttendance.Controllers
{
    public class BaseController : ControllerBase
    {
        public static new ResponseModel Response(int status, ResponseData data, string error = "")
        {
            if (string.IsNullOrEmpty(error)) return new ResponseModel() { status = status, result = data };
            else return new ResponseModel() { status = status, result = new ResponseData() { statusMessage = error } };
        }
    }
}