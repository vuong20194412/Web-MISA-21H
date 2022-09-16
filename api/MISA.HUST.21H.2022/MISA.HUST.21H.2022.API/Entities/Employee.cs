namespace MISA.HUST._21H._2022.API.Entities
{
    /// <summary>
    /// Thông tin nhân viên
    /// </summary>
    public class Employee
    {
        /// <summary>
        ///Id nhân viên
        ///<sql EmployeeId char (36) NOT NULL DEFAULT '' COMMENT 'Id nhân viên' >
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        ///Mã nhân viên
        ///<sql EmployeeCode varchar(20) NOT NULL DEFAULT '' COMMENT 'Mã nhân viên' >
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        ///Họ và tên
        ///<sql FullName varchar(100) NOT NULL DEFAULT '' COMMENT 'Họ và tên' >
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        ///Ngày sinh
        ///<sql DateOfBirth date DEFAULT NULL COMMENT 'Ngày sinh' >
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        ///Mã giới tính
        ///<sql Gender tinyint DEFAULT NULL COMMENT 'Mã giới tính' >
        /// </summary>
        public Int16? Gender { get; set; }

        /// <summary>
        ///Số cmtnd/căn cước
        ///<sql IdentityNumber varchar(25) NOT NULL DEFAULT '' COMMENT 'Số cmtnd/căn cước' >
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        ///Ngày cấp số cmtnd/căn cước
        ///<sql IdentityDate date DEFAULT NULL COMMENT 'Ngày cấp số cmtnd/căn cước' >
        /// </summary>
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        ///Nơi cấp số cmtnd/căn cước
        ///<sql IdentityPlace varchar(255) DEFAULT NULL COMMENT 'Nơi cấp số cmtnd/căn cước' >
        /// </summary>
        public string? IdentityPlace { get; set; }

        /// <summary>
        ///Địa chỉ thư điện tử
        ///<sql Email varchar(100) NOT NULL DEFAULT '' COMMENT 'Địa chỉ thư điện tử' >
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///Số điện thoại
        ///<sql PhoneNumber varchar(50) NOT NULL DEFAULT '' COMMENT 'Số điện thoại' >
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///Id vi trí
        ///<sql PositionId char (36) DEFAULT NULL COMMENT 'Id vi trí' >
        /// </summary>
        public Guid? PositionId { get; set; }

        /// <summary>
        ///Tên vi trí
        ///<sql PositionName varchar(255) DEFAULT NULL COMMENT 'Tên vi trí' >
        /// </summary>
        public string? PositionName { get; set; }

        /// <summary>
        ///Id phòng ban
        ///<sql DepartmentId char (36) DEFAULT NULL COMMENT 'Id phòng ban' >
        /// </summary>
        public Guid? DepartmentId { get; set; }

        /// <summary>
        ///Tên phòng ban
        ///<sql DepartmentName varchar(255) DEFAULT NULL COMMENT 'Tên phòng ban' >
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        ///Mã số thuế cá nhân
        ///<sql PersonalTaxCode varchar(20) DEFAULT NULL COMMENT 'Mã số thuế cá nhân' >
        /// </summary>
        public string? PersonalTaxCode { get; set; }

        /// <summary>
        ///Mức lương
        ///<sql Salary decimal (18, 4) DEFAULT NULL COMMENT 'Mức lương' >
        /// </summary>
        public decimal? Salary { get; set; }

        /// <summary>
        ///Ngày tham gia công ty
        ///<sql JoinDate date DEFAULT NULL COMMENT 'Ngày tham gia công ty' >
        /// </summary>
        public DateTime? JoinDate { get; set; }

        /// <summary>
        ///Tình trạng làm việc
        ///<sql WorkStatus tinyint DEFAULT NULL COMMENT 'Tình trạng làm việc' >
        /// </summary>
        public Int16? WorkStatus { get; set; }

        /// <summary>
        ///Thời điểm bản ghi được tạo
        ///<sql CreatedDate datetime NOT NULL COMMENT 'Thời điểm bản ghi được tạo' >
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        ///Người tạo bản ghi
        ///<sql CreatedBy varchar(100) NOT NULL COMMENT 'Người tạo bản ghi' >
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        ///Thời điểm gần nhất bản ghi được thay đổi
        ///<sql ModifiedDate datetime NOT NULL COMMENT 'Thời điểm gần nhất bản ghi được thay đổi' >
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        ///Người gần nhất thay đổi bản ghi
        ///<sql ModifiedBy varchar(100) NOT NULL COMMENT 'Người gần nhất thay đổi bản ghi' >
        /// </summary>
        public string? ModifiedBy { get; set; }
    }
}
