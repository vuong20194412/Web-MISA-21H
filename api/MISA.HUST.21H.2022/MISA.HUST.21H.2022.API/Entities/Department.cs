namespace MISA.HUST._21H._2022.API.Entities
{
    /// <summary>
    /// Thông tin phòng ban
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Id phòng ban
        /// <sql  DepartmentId char (36) NOT NULL DEFAULT '' COMMENT 'Id phòng ban' >
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// <sql DepartmentCode char (20) NOT NULL COMMENT 'Mã phòng ban' >
        /// </summary>
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// <sql DepartmentName varchar(255) NOT NULL COMMENT 'Tên phòng ban' >
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        ///Thời điểm bản ghi được tạo
        ///<sql CreatedDate datetime NOT NULL COMMENT 'Thời điểm bản ghi được tạo' >
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        ///Người tạo bản ghi
        ///<sql CreatedBy varchar(100) NOT NULL COMMENT 'Người tạo bản ghi' >
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        ///Thời điểm gần nhất bản ghi được thay đổi
        ///<sql ModifiedDate datetime NOT NULL COMMENT 'Thời điểm gần nhất bản ghi được thay đổi' >
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        ///Người gần nhất thay đổi bản ghi
        ///<sql ModifiedBy varchar(100) NOT NULL COMMENT 'Người gần nhất thay đổi bản ghi' >
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
