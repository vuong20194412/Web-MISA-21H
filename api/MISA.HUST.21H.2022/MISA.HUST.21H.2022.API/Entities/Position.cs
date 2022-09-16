namespace MISA.HUST._21H._2022.API.Entities
{
    /// <summary>
    /// Thông tin vị trí
    /// </summary>
    public class Position
    {  
        /// <summary>
        /// Id vị trí
        /// <sql PositionId char(36) NOT NULL DEFAULT '' COMMENT 'Id vị trí' >
        /// </summary>
        public Guid PositionId { get; set; }

        /// <summary>
        /// Mã vị trí
        /// <sql PositionCode char (20) NOT NULL COMMENT 'Mã vị trí' >
        /// </summary>
        public string PositionCode { get; set; }

        /// <summary>
        /// Tên vị trí
        /// <sql PositionName varchar(255) NOT NULL COMMENT 'Tên vị trí' >
        /// </summary>
        public string PositionName { get; set; }

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
