namespace TimeAttendance.RequestModels
{
    public class StampTime
    {
        public int Id { get; set; }

        // 1 = punchin, 2 = punchout
        public int Status { get; set; }
    }
}