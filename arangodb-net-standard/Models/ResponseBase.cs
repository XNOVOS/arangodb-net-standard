using JetBrains.Annotations;

namespace ArangoDBNetStandard.Models
{
    public abstract class ResponseBase
    {
        protected ResponseBase([NotNull] ApiResponse responseDetails)
        {
            ResponseDetails = responseDetails;
        }

        public bool IsSuccess => ResponseDetails == null || !ResponseDetails.Error;
        public ApiResponse ResponseDetails { get; }
    }
}
