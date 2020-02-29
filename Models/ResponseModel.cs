namespace TimeAttendance.Models
{
    public class ResponseModel
    {
        public int status { get; set; }
        public ResponseData result { get; set; }
    }

    public class ResponseData
    {
        public string statusMessage { get; set; }
        public object data { get; set; }
    }
}