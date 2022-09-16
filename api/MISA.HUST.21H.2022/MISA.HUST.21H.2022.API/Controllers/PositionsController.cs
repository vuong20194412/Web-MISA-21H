using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.HUST._21H._2022.API.Entities;
using MySqlConnector;
using Dapper;

namespace MISA.HUST._21H._2022.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        /// <summary>
        /// Lấy danh sách tất cả vị trí
        /// </summary>
        /// <returns>
        /// Danh sách tất cả vị trí
        /// </returns>
        /// Created by: THVUONG 01/09/2022
        [HttpGet]
        public IActionResult GetAllPosition()
        {
            try
            {   
                // Kết nối Database
                MySqlConnection connection = new(Constant.CONNECTION_STRING);
                connection.Open();

                // Tạo truy vấn SQL
                string query = "SELECT * FROM positions;";

                // Thực thi truy vấn
                List<Position> positions = connection.Query<Position>(query).ToList();

                // Đóng kết nối với Database
                connection.Close();

                return StatusCode(StatusCodes.Status200OK, positions);
            }
            catch (Exception ex)
            {
                ErrorInfo errorInfo = new(errorCode: "e001", errorMessage: ex.Message);

                return StatusCode(StatusCodes.Status400BadRequest, errorInfo);
            }
        }
    }
}
