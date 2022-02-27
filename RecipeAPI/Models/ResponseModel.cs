namespace RecipeAPI.Models
{
    public class ResponseModel
    {
        public string Type { get; set; }

        public string Message { get; set; }

        public int StatusCode { get; set; }

        public string ClientRequest { get; set; }

        public string Action { get; set; }

        public dynamic Errors { get; set; }
    }
}
