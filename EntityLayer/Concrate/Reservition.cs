namespace EntityLayer.Concrate
{
    public class Reservition
    {
        public int id { get; set; }
        public string status { get; set; }
        public int HowmanyPapel { get; set; }
        public int Userid { get; set; }
        public User User { get; set; }
        public string Username { get; set; }
        public int Destintionid { get; set; }
        public Destiniton Destiniton { get; set; }
        public DateTime ReservDate { get; set; }
        public DateTime ReservStart { get; set; }
        public DateTime ReservEnd { get; set; }
        public int? Guidid { get; set; }
        public Guide Guide { get; set; }
        public string Note { get; set; }
    }
}
