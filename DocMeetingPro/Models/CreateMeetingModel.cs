namespace DocMeetingPro.Models
{
    public class CreateMeetingModel
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int SaloonId { get; set; }
    }
}
