using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeAttendance.Models;
using TimeAttendance.RequestModels;
using TimeAttendance.Services;

namespace TimeAttendance.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class ActionController : BaseController
    {
        private const string INVALID_PARAMETER = "Invalid parameters.";
        private readonly TimeAttendanceContext db;
        private readonly TransactionService transactionService;
        private readonly UserService userService;

        public ActionController(TimeAttendanceContext db)
        {
            this.db = db;
            transactionService = new TransactionService(db);
            userService = new UserService(db);
        }

        [ActionName("login")]
        [HttpPost()]
        public async Task<ResponseModel> LogIn([FromBody] LogIn request)
        {
            if (!ModelState.IsValid) return Response(400, null, INVALID_PARAMETER);
            var user = await userService.GetUserByEmail(request.Email);
            // check user is exist
            if (user == null) return Response(400, null, "Email not found.");
            // check password is valid
            if (!HashUtility.PasswordIsValid(request.Password.Trim().ToLower(), user))
                return Response(400, null, "Password is invalid.");
            // add transaction for login type
            await transactionService.AddTransaction(new Transaction() { UserId = user.Id, Type = TransactionType.SIGNIN });
            // valid login
            return Response(200, new ResponseData() { data = new { id = user.Id }, statusMessage = "Login success." });
        }

        [ActionName("logout")]
        [HttpPost()]
        public async Task<ResponseModel> LogOut([FromBody] LogOut request)
        {
            if (!ModelState.IsValid) return Response(400, null, INVALID_PARAMETER);
            if (request.Status == 2) return Response(400, null, "Error can't log out");
            var user = await userService.GetUserById(request.Id);
            if (user == null) return Response(400, null, "User not found.");
            // add transaction for signout type
            await transactionService.AddTransaction(new Transaction() { UserId = user.Id, Type = TransactionType.SIGNOUT, Remark = $"status = {request.Status}" });
            return Response(200, new ResponseData() { statusMessage = "LogOut success." });
        }

        [ActionName("changepassword")]
        [HttpPut]
        public async Task<ResponseModel> ChangePassword([FromBody] ChangePassword request)
        {
            if (!ModelState.IsValid) return Response(400, null, INVALID_PARAMETER);
            var user = await userService.GetUserById(request.Id);
            if (user == null) return Response(400, null, "User not found.");
            if (!HashUtility.PasswordIsValid(request.CurrentPassword, user))
                return Response(400, null, "Current password is invalid.");
            if (await userService.ChangePassword(request.Id, request.NewPassword))
                return Response(200, new ResponseData() { statusMessage = "Change password success." });
            else return Response(400, null, INVALID_PARAMETER);
        }

        [ActionName("employee")]
        [HttpGet]
        public async Task<ResponseModel> GetEmployee([FromQuery] int id)
        {
            if (id == 0) return Response(400, null, INVALID_PARAMETER);
            var user = await userService.GetUserById(id);
            if (user == null) return Response(400, null, "User not found.");
            return Response(200, new ResponseData() { statusMessage = "Get employee information", data = user });
        }

        [ActionName("stamptime")]
        [HttpPost]
        public async Task<ResponseModel> StampTime([FromBody] StampTime request)
        {
            if (!ModelState.IsValid) return Response(400, null, INVALID_PARAMETER);
            var transaction = new Transaction()
            {
                UserId = request.Id,
                Type = request.Status == 1 ? TransactionType.PUNCHIN : TransactionType.PUNCHOUT
            };
            await transactionService.AddTransaction(transaction);
            return Response(200, new ResponseData()
            {
                data = new { StampTime = transaction.CreatedDate },
                statusMessage = "StampTime success."
            });
        }

        [ActionName("dashboard")]
        [HttpGet]
        public async Task<ResponseModel> Dashboard([FromQuery] int id)
        {
            if (!ModelState.IsValid) return Response(400, null, INVALID_PARAMETER);
            var dashboards = await transactionService.GetDashboards(id);
            return Response(200, new ResponseData() { data = dashboards });
        }

        [ActionName("historytime")]
        [HttpGet]
        public async Task<ResponseModel> HistoryTime([FromQuery] int id)
        {
            if (!ModelState.IsValid) return Response(400, null, INVALID_PARAMETER);
            var dashboard = await transactionService.GetDashboard(id, DateTime.Now.Date);
            return Response(200, new ResponseData() { data = dashboard });
        }

    }
}