namespace EasyWayRide.Shared.Responses
{
    public class ServiceResponseBase
    {
        public string? Message { get; set; }
        public bool Success { get; set; } = false;
    }
    public class ServiceResponseMessage : ServiceResponseBase
    {
    }
    public class ServiceResponseData<T> : ServiceResponseBase
    {
        public T? Data { get; set; }
    }

    #region Errors
    public class ServiceResponseError : ServiceResponseBase
    {
        public string? Error { get; set; }
    }
    public class ServiceResponseErrors : ServiceResponseBase
    {
        public List<string>? Errors { get; set; }
    }
    #endregion

    #region Auth
    public class AuthResponse : ServiceResponseBase
    {
        public string? Access_token { get; set; }

    }
    #endregion

}
