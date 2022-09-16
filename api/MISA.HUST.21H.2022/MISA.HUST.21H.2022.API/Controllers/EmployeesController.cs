using Microsoft.AspNetCore.Mvc;
using MISA.HUST._21H._2022.API.DTO;
using MISA.HUST._21H._2022.API.Entities;
using MySqlConnector;
using Dapper;

namespace MISA.HUST._21H._2022.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        // Chưa có tài khoản gì nên sẽ cố định người tạo mới và người sửa gần nhất
        private const string CreatedBy = "Trần Hoàng Vượng";
        private const string ModifiedBy = "Vượng Trần Hoàng";

        /// <summary>
        /// API lấy danh sách nhân viên thỏa mãn điều kiện lọc
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="positionId"></param>
        /// <param name="departmentId"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns>
        /// Danh sách nhân viên và tổng số nhân viên thỏa mãn điều kiện lọc
        /// </returns>
        /// Created by: THVUONG 01/09/2022
        /// Note: chỉ muốn truyền thêm đuôi nên không có {}
        /// ----: pageNumber = 0 thì lấy dữ liêu cho trang cuối 
        [HttpGet]
        public IActionResult GetFilterEmployees([FromQuery] string? keyword,
                                                [FromQuery] Guid? positionId,
                                                [FromQuery] Guid? departmentId,
                                                [FromQuery] long? pageSize,
                                                [FromQuery] long? pageNumber)
        {
            try 
            {
                // Kiểm tra kích thước trang và số thứ tự trang
                if (pageSize != null && pageSize < 0)
                {
                    ErrorInfo errorInfo = new(errorCode: "e005", errorMessage: "Kích thước trang không được bé hơn 0");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }
                if (pageNumber != null && pageNumber < 0)
                {
                    ErrorInfo errorInfo = new(errorCode: "e005", errorMessage: "Số thứ tự trang không được bé hơn 0");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }

                // Kết nối Database
                MySqlConnection connection = new(Constant.CONNECTION_STRING);
                connection.Open();

                // Tạo truy vấn SQL
                string query = "SELECT * FROM employee";
                string query1 = "SELECT COUNT(*) FROM employee";
                string condition = "";
                string sort = " Order BY ModifiedDate DESC";
                if (keyword != null)
                {
                    if (positionId != null)
                    {
                        if (departmentId != null)
                        {
                            condition = $" WHERE (EmployeeCode LIKE '%{keyword}%' OR FullName LIKE '%{keyword}%') AND PositionId = '{positionId}' AND DepartmentId = '{departmentId}'";
                        }
                        else
                        {
                            condition = $" WHERE (EmployeeCode LIKE '%{keyword}%' OR FullName LIKE '%{keyword}%') AND PositionId = '{positionId}'";
                        }
                    }
                    else
                    {
                        if (departmentId != null)
                        {
                            condition = $" WHERE (EmployeeCode LIKE '%{keyword}%' OR FullName LIKE '%{keyword}%') AND DepartmentId = '{departmentId}'";
                        }
                        else
                        {
                            condition = $" WHERE EmployeeCode LIKE '%{keyword}%' OR FullName LIKE '%{keyword}%'";
                        }
                    }
                }
                else
                {
                    if (positionId != null)
                    {
                        if (departmentId != null)
                        {
                            condition = $" WHERE PositionId = '{positionId}' AND DepartmentId = '{departmentId}'";
                        }
                        else
                        {
                            condition = $" WHERE PositionId = '{positionId}'";
                        }
                    }
                    else
                    {
                        if (departmentId != null)
                        {
                            condition = $" WHERE DepartmentId = '{departmentId}'";
                        }
                    }
                }
                if (pageNumber == null)
                {
                    pageNumber = 1;
                }
                else if (pageNumber == 0)
                {
                    sort = " Order BY ModifiedDate ASC";
                }
                condition += sort;
                query += condition;
                query1 += condition + ";";
                if (pageSize != null)
                {
                    long offset = (long)(pageNumber > 0 ? (pageNumber - 1) * pageSize : 0);
                    long limit = (long)pageSize;
                    query += $" LIMIT {limit} OFFSET {offset};";
                }
                else
                {
                    pageSize = 0;
                }

                // Thực thi truy vấn
                PagingData pagingData = new(connection.QueryFirst<long>(query1), connection.Query<Employee>(query).ToList());

                // Đóng kết nối với Database
                connection.Close();

                // Xử lý kết quả
                if (pageNumber == 0 && pageSize > 0)
                {
                    long currentRecord = (long)(pagingData.TotalRecord % pageSize);
                    if (currentRecord > 0)
                    {
                        while (pagingData.Data.Count > currentRecord)
                        {
                            pagingData.Data.RemoveAt(0);
                        }
                    }
                    pagingData.Data.Reverse();
                }

                return StatusCode(StatusCodes.Status200OK, pagingData);
            }
            catch (Exception ex)
            {
                ErrorInfo errorInfo = new(errorCode: "e001", errorMessage: ex.Message);

                return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
            }
        }

        /// <summary>
        /// API lấy thông tin chi tiết của một nhân viên
        /// </summary>
        /// <param name="EmployeeId">Id nhân viên</param>
        /// <returns>
        /// Thông tin chi tiết nếu có
        /// </returns>
        /// Created by: THVUONG 01/09/2022
        /// Note: muốn truyền tham số thì có {}
        [HttpGet]
        [Route("{employeeId}")] 
        public IActionResult GetEmployeeById([FromRoute] Guid employeeId)
        {
            try
            {
                // Kết nối Database
                MySqlConnection connection = new(Constant.CONNECTION_STRING);
                connection.Open();

                // Tạo truy vấn SQL
                string query = $"SELECT * FROM employee WHERE EmployeeId = '{employeeId}';";

                // Thực thi truy vấn
                Employee employee = connection.QueryFirstOrDefault<Employee>(query);

                // Đóng kết nối với Database
                connection.Close();

                // Xử lý kết quả
                if (employee != null)
                {
                    return StatusCode(StatusCodes.Status200OK, employee);
                }
                else
                {
                    ErrorInfo errorInfo = new(errorCode: "e002", errorMessage: "Bản ghi muốn lấy không tồn tại");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }
            }
            catch (Exception ex)
            {
                ErrorInfo errorInfo = new(errorCode: "e001", errorMessage: ex.Message);

                return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
            }
        }

        /// <summary>
        /// API lấy mã nhân viên mới
        /// </summary>
        /// <returns>
        /// Mã nhân viên mới
        /// </returns>
        /// Created by: THVUONG 01/09/2022
        [HttpGet]
        [Route("newEmployeeCode")]
        public IActionResult GetAutoIncrementEmployeeCode()
        {
            try
            {
                // Kết nối Database
                MySqlConnection connection = new(Constant.CONNECTION_STRING);
                connection.Open();

                // Tạo truy vấn SQL
                string query1 = "SELECT MAX(CHAR_LENGTH(EmployeeCode)) FROM employee";
                string query = "SELECT MAX(EmployeeCode) FROM employee WHERE CHAR_LENGTH(EmployeeCode) = @maxLength;";

                // Thực thi truy vấn phụ
                long maxLength = connection.QueryFirstOrDefault<long>(query1);

                // Chuẩn bị tham số động truy vấn chính SQL
                DynamicParameters parameters = new();
                parameters.Add("@maxLength", maxLength);

                // Thực thi truy vấn chính
                string employeeCode = connection.QueryFirstOrDefault<string>(query, parameters);

                // Đóng kết nối Database
                connection.Close();

                // Xử lý kết quả
                string newEmployeeCode = "NV";
                if (employeeCode != null)
                {
                    long employeeCodeNumber = Convert.ToInt64(employeeCode.Substring(2)) + 1;

                    newEmployeeCode += $"{employeeCodeNumber}";
                }
                else
                {
                    newEmployeeCode += "0";
                }

                return StatusCode(StatusCodes.Status200OK, newEmployeeCode);
            }
            catch (Exception ex)
            {
                ErrorInfo errorInfo = new(errorCode: "e001", errorMessage: ex.Message);

                return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
            }
        }

        /// <summary>
        /// API thêm một nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>
        /// Id nhân viên nếu thành công 
        /// </returns>
        /// Created by: THVUONG 01/09/2022
        [HttpPost]
        public IActionResult InsertEmployee([FromBody] Employee employee)
        {
            try
            {
                // Kiểm tra format mã nhân viên 
                if (!(employee.EmployeeCode.Length > 2 && employee.EmployeeCode.StartsWith("NV") && typeof(long).IsInstanceOfType(Convert.ToInt64(employee.EmployeeCode.Substring(2)))))
                {
                    ErrorInfo errorInfo = new(errorCode: "e002", errorMessage: "Mã nhân viên phải có dạng NV<số tự nhiên>");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }

                
                Guid EmployeeId = Guid.NewGuid();
                DateTime CreatedDate = DateTime.Now;

                // Kết nối Database
                MySqlConnection connection = new(Constant.CONNECTION_STRING);
                connection.Open();

                // Tạo câu lệnh SQL
                string command =
                   "INSERT INTO employee(EmployeeId, EmployeeCode, FullName, DateOfBirth, Gender, IdentityNumber, IdentityDate, IdentityPlace, Email, PhoneNumber, PositionId, PositionName, DepartmentId, DepartmentName, PersonalTaxCode, Salary, JoinDate, WorkStatus, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy) "
                   + "VALUES(@EmployeeId, @EmployeeCode, @FullName, @DateOfBirth, @Gender, @IdentityNumber, @IdentityDate, @IdentityPlace, @Email, @PhoneNumber, @PositionId, (SELECT PositionName FROM positions WHERE PositionId = @PositionId), @DepartmentId, (SELECT DepartmentName FROM department WHERE DepartmentId = @DepartmentId), @PersonalTaxCode, @Salary, @JoinDate, @WorkStatus, @CreatedDate, @CreatedBy, @CreatedDate, @CreatedBy);";

                // Chuẩn bị tham số động câu lệnh SQL
                DynamicParameters parameters = new();
                parameters.Add("@EmployeeId", EmployeeId);
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@FullName", employee.FullName);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@IdentityDate", employee.IdentityDate);
                parameters.Add("@IdentityPlace", employee.IdentityPlace);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@PhoneNumber", employee.PhoneNumber);
                parameters.Add("@PositionId", employee.PositionId);
                parameters.Add("@DepartmentId", employee.DepartmentId);
                parameters.Add("@PersonalTaxCode", employee.PersonalTaxCode);
                parameters.Add("@Salary", employee.Salary);
                parameters.Add("@JoinDate", employee.JoinDate);
                parameters.Add("@WorkStatus", employee.WorkStatus);
                parameters.Add("@CreatedDate", CreatedDate);
                parameters.Add("@CreatedBy", CreatedBy);
                parameters.Add("@CreatedDate", CreatedDate);
                parameters.Add("@CreatedBy", CreatedBy);

                // Thực thi câu lệnh
                int NumberOfAffectedRows = connection.Execute(command, parameters);

                // Đóng kết nối với Database
                connection.Close();

                // Xử lý kết quả
                if (NumberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status201Created, EmployeeId);
                }
                else
                {
                    ErrorInfo errorInfo = new(errorCode: "e004", errorMessage: "Lỗi chưa biết");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }
            }
            catch (MySqlException mySqlEx)
            {
                if (mySqlEx.ToString().Contains($"Duplicate entry '{employee.EmployeeCode}' for key 'EmployeeCode'"))
                {
                    ErrorInfo errorInfo = new(errorCode: "e003", errorMessage: "Mã nhân viên đã tồn tại");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }
                else if (mySqlEx.ToString().Contains($"Duplicate entry '{employee.IdentityNumber}' for key 'IdentityNumber'"))
                {
                    ErrorInfo errorInfo = new(errorCode: "e003", errorMessage: "Số CMTND/Căn cước đã tồn tại");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }
                else if (mySqlEx.ToString().Contains($"Duplicate entry '{employee.PersonalTaxCode}' for key 'PersonalTaxCode'"))
                {
                    ErrorInfo errorInfo = new(errorCode: "e003", errorMessage: "Mã số thuế cá nhân đã tồn tại");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }
                else
                {
                    ErrorInfo errorInfo = new(errorCode: "e001", errorMessage: mySqlEx.Message);

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }
            } 
            catch (Exception ex)
            {
                ErrorInfo errorInfo = new(errorCode: "e001", errorMessage: ex.Message);

                return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
            }
        }

        /// <summary>
        /// API sửa thông tin chi tiết của một nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="employeeId"></param>
        /// <returns>
        /// Id nhân viên nếu thành công 
        /// </returns>
        /// Created by: THVUONG 01/09/2022
        [HttpPut]
        [Route("{employeeId}")]
        public IActionResult UpdateEmployee([FromBody] Employee employee, [FromRoute] Guid employeeId)
        {
            try
            {
                DateTime ModifiedDate = DateTime.Now;

                // Kiểm tra format mã nhân viên
                if (!(employee.EmployeeCode.Length > 2 && employee.EmployeeCode.StartsWith("NV") && typeof(long).IsInstanceOfType(Convert.ToInt64(employee.EmployeeCode.Substring(2)))))
                {
                    ErrorInfo errorInfo = new(errorCode: "e002", errorMessage: "Mã nhân viên phải có dạng NV<số tự nhiên>");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }

                // Kết nối Database
                MySqlConnection connection = new(Constant.CONNECTION_STRING);
                connection.Open();

                // Tạo câu lệnh SQL
                string command =
                   "UPDATE employee "
                   + "SET EmployeeCode = @EmployeeCode, FullName = @FullName, DateOfBirth = @DateOfBirth, Gender = @Gender, IdentityNumber = @IdentityNumber, IdentityDate = @IdentityDate, IdentityPlace = @IdentityPlace, Email = @Email, PhoneNumber = @PhoneNumber, PositionId = @PositionId, PositionName = (SELECT PositionName FROM positions WHERE PositionId = @PositionId), DepartmentId = @DepartmentId, DepartmentName = (SELECT DepartmentName FROM department WHERE DepartmentId = @DepartmentId), PersonalTaxCode = @PersonalTaxCode, Salary = @Salary, JoinDate = @JoinDate, WorkStatus = @WorkStatus, ModifiedDate = @ModifiedDate, ModifiedBy = @ModifiedBy "
                   + "WHERE EmployeeId = @EmployeeId;";

                // Chuẩn bị tham số động câu lệnh SQL
                DynamicParameters parameters = new();
                parameters.Add("@EmployeeId", employeeId);
                parameters.Add("@EmployeeCode", employee.EmployeeCode);
                parameters.Add("@FullName", employee.FullName);
                parameters.Add("@DateOfBirth", employee.DateOfBirth);
                parameters.Add("@Gender", employee.Gender);
                parameters.Add("@IdentityNumber", employee.IdentityNumber);
                parameters.Add("@IdentityDate", employee.IdentityDate);
                parameters.Add("@IdentityPlace", employee.IdentityPlace);
                parameters.Add("@Email", employee.Email);
                parameters.Add("@PhoneNumber", employee.PhoneNumber);
                parameters.Add("@PositionId", employee.PositionId);
                parameters.Add("@DepartmentId", employee.DepartmentId);
                parameters.Add("@PersonalTaxCode", employee.PersonalTaxCode);
                parameters.Add("@Salary", employee.Salary);
                parameters.Add("@JoinDate", employee.JoinDate);
                parameters.Add("@WorkStatus", employee.WorkStatus);
                parameters.Add("@ModifiedDate", ModifiedDate);
                parameters.Add("@ModifiedBy", ModifiedBy);

                // Thực thi câu lệnh
                int numberOfAffectedRows = connection.Execute(command, parameters);

                // Đóng kết nối với Database
                connection.Close();

                // Xử lý kết quả
                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeId);
                }
                else
                {
                    ErrorInfo errorInfo = new(errorCode: "e002", errorMessage: "Bản ghi muốn sửa không tồn tại");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }
            }
            catch (MySqlException mySqlEx)
            {
                if (mySqlEx.ToString().Contains($"Duplicate entry '{employee.EmployeeCode}' for key 'EmployeeCode'"))
                {
                    ErrorInfo errorInfo = new(errorCode: "e003", errorMessage: "Mã nhân viên đã tồn tại");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }
                else if (mySqlEx.ToString().Contains($"Duplicate entry '{employee.IdentityNumber}' for key 'IdentityNumber'"))
                {
                    ErrorInfo errorInfo = new(errorCode: "e003", errorMessage: "Số CMTND/Căn cước đã tồn tại");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }
                else if (mySqlEx.ToString().Contains($"Duplicate entry '{employee.PersonalTaxCode}' for key 'PersonalTaxCode'"))
                {
                    ErrorInfo errorInfo = new(errorCode: "e003", errorMessage: "Mã số thuế cá nhân đã tồn tại");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }
                else
                {
                    ErrorInfo errorInfo = new(errorCode: "e001", errorMessage: mySqlEx.Message);

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }
            }
            catch (Exception ex)
            {
                ErrorInfo errorInfo = new(errorCode: "e001", errorMessage: ex.Message);

                return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
            }
        }

        /// <summary>
        /// API xóa một nhân viên
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>
        /// Id nhân viên nếu thành công 
        /// </returns>
        /// Created by: THVUONG 01/09/2022
        [HttpDelete]
        [Route("{employeeId}")]
        public IActionResult DeleteEmployee([FromRoute] Guid employeeId)
        {
            try
            {
                // Kết nối Database
                MySqlConnection connection = new(Constant.CONNECTION_STRING);
                connection.Open();

                // Tạo câu lệnh SQL
                string command = $"DELETE FROM employee WHERE EmployeeId = '{employeeId}';";

                // Thực thi câu lệnh
                int NumberOfAffectedRows = connection.Execute(command);

                // Đóng kết nối với Database
                connection.Close();

                // Xử lý kết quả
                if (NumberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, employeeId);
                }
                else
                {
                    ErrorInfo errorInfo = new(errorCode: "e002", errorMessage: "Bản ghi muốn xóa không tồn tại");

                    return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
                }
            }
            catch (Exception ex)
            {
                ErrorInfo errorInfo = new(errorCode: "e001", errorMessage: ex.Message);

                return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
            }
        }
    }
}
