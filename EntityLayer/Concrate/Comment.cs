namespace EntityLayer.Concrate
{
    public class Comment
    {
        public int id { get; set; }
        public string CommentUser { get; set; }
        public string CommentContent { get; set; }
        public bool status { get; set; }
        public DateTime CommentData { get; set; }
        public int Userid { get; set; }
        public int Destinitonid { get; set; }
        public Destiniton Destiniton { get; set; }
    }
}
