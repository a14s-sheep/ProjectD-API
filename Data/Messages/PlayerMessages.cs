namespace ProjectD_API.Data.Messages
{
    public class PlayerStat
    {
        public string Name { get; set; }
        public float Value { get; set; }
    }

    public class PlayerCreateRequest
    {
        public string Username { get; set; }
    }

}
