namespace backend.Models
{
    public class Activity : BaseModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string ControllerName { get; set; }
        public string ActionType { get; set; }
        public string ResourceName { get; set; }
        public string Description { get; set; }
    }
}