namespace MISA.HUST._21H._2022.API.Controllers
{
    /// <summary>
    /// Chứa thông tin lỗi
    /// </summary>
    public class ErrorInfo
    {
        /// <summary>
        /// Mã lỗi
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Nội dung của lỗi
        /// </summary>
        public string ErrorMessage { get; set; }

        public ErrorInfo(string errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
