using MISA.HUST._21H._2022.API.Entities;

namespace MISA.HUST._21H._2022.API.DTO
{
    /// <summary>
    /// Dữ liệu phân trang
    /// </summary>
    public class PagingData
    {
        /// <summary>
        /// Tổng số lượng bản ghi
        /// </summary>
        public long TotalRecord { set; get; }

        /// <summary>
        /// Các bản ghi
        /// </summary>
        public List<Employee> Data { set; get; }

        public PagingData(long totalRecord, List<Employee> data)
        {
            TotalRecord = totalRecord;
            Data = data;
        }
    }
}
