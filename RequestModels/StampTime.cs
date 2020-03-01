namespace TimeAttendance.RequestModels
{
    public class StampTime
    {
        public int Id { get; set; }

        /// <summary>
        /// 1 = PunchIn, 2 = PunchOut
        /// </summary>
        /// <value></value>
        public int Status { get; set; }
    }
}