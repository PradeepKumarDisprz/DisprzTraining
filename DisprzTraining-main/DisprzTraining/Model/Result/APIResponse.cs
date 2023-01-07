

namespace DisprzTraining.Model
{
   public static class APIResponse
    {
        public static ErrorResponse ConflictResponse = new ErrorResponse()
        {
            language = "en",
            errorMessage = "Conflict occured between different meetings",
            errorCode = "AC_001"
        };
        public static ErrorResponse BadRequestResponse = new ErrorResponse()
        {
            language = "en",
            errorMessage = "Appointment for past time and days are not allowed or input starttime and endtime may be same",
            errorCode = "AC_002"
        };
        public static ErrorResponse NotFoundResponse = new ErrorResponse()
        {
            language = "en",
            errorMessage = "No Appointment found with the given Id",
            errorCode = "AC_003"
        };
    }
}