// Attr field value khớp field dữ liệu backend để dễ bảo trì, thay đổi 
const FIELD_ATTR = "field";

// Số lượng button pag number tối đa hiển thị trên navigation
const PAG_NUMBER_MAX_SIZE = 4;
let pageMaxNumber = 1;
let pageMinNumber = PAG_NUMBER_MAX_SIZE;

// 2 chế độ form ADD và EDIT
const ADD_EMPLOYEE = 0;
const EDIT_EMPLOYEE = 1;
let formMode = ADD_EMPLOYEE;

// Đánh dấu trang cuối cùng
const LAST_PAGE = 0;

// Từ khóa tìm kiếm
let keyword = null;

// Bản ghi đang được chọn
let employee = null;

// Số lượng tối đa dòng hiển thị trên bảng trên một trang
let pageSize;
let pagePrevSize;

// Số thứ tự trang hiển thị hiện tại
let currentPage = 1;

// Vị trí đang được lựa chọn
let positionId = null;

// Phòng ban đang được lựa chọn
let departmentId = null;

// Thực hiện javascript khi html và css đã săn sàng:
$(document).ready(function () {
    
    init();

    // Tải danh sách nhân viên và cập nhật bảng
    loadData(currentPage);

    // Tải các phòng ban đưa vào combobox  
    loadDepartment();

    // Tải các vị trí đưa vào combobox
    loadPosition();
});

/**
 * Khởi tạo giá trị cho các biến và sự kiện cho các phần tử html
 * Author: THVUONG (09/09/2022)
 */
function init() {
    // Khởi tạo giá trị cho biến
    pagePrevSize = pageSize = parseInt($("#paging select").val());
    currentPage = parseInt($("#paging .navigation .number-group .number--selected").text());

    // Khởi tạo sự kiện nhấn nút thì hiển thị form thêm mới
    $("#addButton").click(function() {
        showNewForm();
    })

    // Khởi tạo hint
    $("#searchInput").val("Tìm kiếm theo mã hoặc tên");
    // Xác định từ khóa và hiển thị hint nếu không có nội dung
    $("#searchInput").blur(function (e) { 
        if ($(this).val() == "") {
            $("#searchInput").val("Tìm kiếm theo mã hoặc tên");
            keyword = null;
        } 
        else {
            keyword = $(this).val();
        }
    });
    // Enter thì kiểm tra có từ khóa thì gọi tải dữ liệu theo từ khóa
    $("#searchInput").keypress(function(e){
        if (e.which === 13) {
            $(this).blur();
            loadData(1); 
        }
    });

    // Khởi tạo sự kiện khi thay đổi thì load trang với lọc theo phòng ban mới
    $("#departmentComboBox").change(function(){
        departmentId = $(this).val() == "" ? null : $(this).val();
        loadData(1);
    })

    // Khởi tạo sự kiện khi thay đổi thì load trang với lọc theo vị trí mới
    $("#positionComboBox").change(function(){
        positionId = $(this).val() == "" ? null : $(this).val();
        loadData(1);
    })

    // Khởi tạo sự kiện khi nhấp thì hiển thị cảnh báo xóa
    $("#deleteButton").click(function() {
        if (employee != null){
            $("#deleteWarnPopUp .pop-up__body b").text(`[ Mã nhân viên: ${employee.EmployeeCode} ]`);
            $("#deleteWarnPopUp").show();
            $("#deleteWarnPopUp [cancelPopup]").focus();
        } 
    })

    // Khởi tạo sự kiện khi nhấp thì hiển thị form nhân bản
    $("#duplicateButton").click(function(){
        // Chỉ hiển thị form khi có dòng được chọn
        if (employee != null){
            showDuplicateForm();
        }
    });

    // Khởi tạo sự kiện khi nhấp thì tải lại
    $("#refreshButton").click(function(){
        loadData(currentPage);
    });

    // Khởi tạo sự kiện khi nhấp thì xác định dòng được chọn
    $(document).on("click", "#table tbody tr", function(){
        // Xóa tất cả các trạng thái được chọn của các dòng dữ liệu khác:
        $(this).siblings().removeClass('row--selected');
        // In đậm dòng được chọn:
        $(this).addClass("row--selected");
        employee = $(this).data('employee');
    });

    // Khởi tạo sự kiện khi nhấp đúp thì hiển thị form thông tin chi tiết
    $(document).on("dblclick", "#table tbody tr", function(){
        showInfoForm($(this), $(this).data('employee').EmployeeId);
    });

    // Khởi tạo sự kiện khi thay đổi thì load trang với số lượng dòng tối đa mới trong bảng 
    $("#paging select").change(function(){
        pagePrevSize = pageSize;
        pageSize = parseInt($(this).val());
        loadData(1);
    });

    // Khởi tạo sự kiện khi nhấp thì load trang đầu tiên
    $("#paging .icon--angle-dbleft").click(function(){
        loadData(1);
    });

    // Khởi tạo sự kiện khi nhấp thì load trang kề trước
    $("#paging .icon--angle-left").click(function(){ 
        loadData(currentPage > 1 ? currentPage - 1 : 1);
    });

    // Khởi tạo sự kiện khi nhấp thì load trang kế tiếp
    $("#paging .icon--angle-right").click(function(){
        loadData(currentPage + 1);
    });

    // Khởi tạo sự kiện khi nhấp thì load trang cuối cùng
    $("#paging .icon--angle-dbright").click(function(){ 
        loadData(LAST_PAGE);
    });

    // Khởi tạo sự kiện khi nhấp thì load trang tương ứng
    $(document).on("click", "#paging .navigation .number-group .number", function(){ 
        loadData(parseInt($(this).text()));
    });

    // Gửi yêu cầu xóa
    $("#deleteWarnPopUp [okPopup]").click(function(){
        deleteEmployee(employee.EmployeeId);
    });

    // Focus nút cancel khi tab
    $("#deleteWarnPopUp [okPopup]").focus(function() {
        $("#deleteWarnPopUp [cancelPopup]").attr("tabindex", 3);
    })
    
    // Focus nút ok khi tab
    $("#deleteWarnPopUp [cancelPopup]").focus(function() {
        $(this).attr("tabindex", 1);
    })

    // Kiểm tra validate form nếu hợp lệ thì gửi yêu cầu lưu
    $("#formPopUp [savePopup]").click(function(){ 
        if (checkValidateForm()){
            if (formMode == ADD_EMPLOYEE) {
                addEmployee();
            }
            else if (formMode == EDIT_EMPLOYEE) {
                updateEmployee(employee.EmployeeId); 
            }
        } 
    });
    
    // Thay đổi tabindex để Tab đến "#employeeCodeInput" 
    $("#formPopUp [cancelPopup]").focus(function(){ 
        $(this).attr("tabindex", 1);
    });

    // Thay đổi tabindex trở lại 
    $("#employeeCodeInput").focus(function() {
        $("#formPopUp [cancelPopup]").attr("tabindex", 18);
    })
     
    // Validate format ô thuộc loại email
    $("#formPopUp input[type=email]").blur(function(){ 
        validateFormatEmail(this);
    });

    // Validate những ô được yêu cầu không rỗng
    $("#formPopUp input[required]").blur(function(){
        validateRequiredInput(this);
    });

    // Validate giá trị ô thuộc loại date
    $("#formPopUp input[type=date]").blur(function(){ 
        validateDateInput(this);
    });

    // Validate format ô mã nhân viên và focus ô họ và tên khi tab
    $("#employeeCodeInput").blur(function(e){ 
        if($(this).val() != '') {
            if (!$(this).val().match(/^NV(([0-9])|([1-9][0-9]{1,17}))$/g)) {
                $(this).parent().addClass("input--error");
                $(this).attr('title', "Mã nhân viên không đúng định dạng.");
                $(this).parent().siblings(".notice").html("Mã nhân viên không đúng định dạng.");
            }
            else {
                $(this).parent().removeClass("input--error");
                $(this).removeAttr('title', "Mã nhân viên không đúng định dạng.");
                $(this).parent().siblings(".notice").html("");
            }
        }
    });

    // Đóng pop up 
    $("[closePopup], [cancelPopup]").click(function() {
        $(this).parents(".pop-up").hide();
    })

    // Loại bỏ "(VNĐ)" khi focus vào
    $("#salaryInput").focus(function() {
        if ($(this).val() != "") {
            let value = ""+ $(this).val();
            $(this).val(value.substring(0, value.length - 5));
        }
    })

    // Format và thêm "(VNĐ)" khi blur
    $("#salaryInput").blur(function() {
        if ($(this).val() != "") {
            // Chưa được vừa nhập vừa format với ngôn ngữ unicode nên sẽ forword kết quả
            let value = ("" + $(this).val()).replace(/[^0-9]+/g, "");
            let len = value.length;
            if (len <= 3) {
                $(this).val(value);
            }
            else {
                let arr = [];
                let i = len % 3;
                if (i == 1) {
                    arr.push(value.substring(0, 1))
                }
                else if (i == 2) {
                    arr.push(value.substring(0, 2))
                }
                for (; i < len; i += 3) {
                    arr.push(value.substring(i, i + 3)); 
                } 
                $(this).val(arr.join("."));
            }
            $(this).val($(this).val() + "(VNĐ)");
        }
    })
}

/**
 * Gửi yêu cầu xóa nhân viên
 * @param {guid} employeeId 
 * Author: THVUONG (09/09/2022) 
 */
 function deleteEmployee(employeeId) {
    // Gọi api thực hiện xóa:
    $.ajax({
        type: "DELETE",
        url: "http://localhost:42714/api/v1/Employees/" + employeeId,
        success: function(response) {
            // Ẩn popup:
            $("#deleteWarnPopUp").hide();
            // Đặt lại employee:
            employee = null;
            // Thông báo
            alert("Xóa dữ liệu thành công");
            // Load lại dữ liệu:
            loadData(currentPage);
        }, 
        error: function(res) {
            console.log(res);
            // Ẩn popup:
            $("#deleteWarnPopUp").hide();
             // Thông báo
            alert("Xóa dữ liệu không thành công");
        }
    });
}

/**
 * Gửi yêu cầu cập nhật thông tin nhân viên
 * @param {guid} employeeId 
 * Author: THVUONG (09/09/2022) 
 */
function updateEmployee(employeeId) {
    // Thu thập dữ liệu và build object:
    let employee = {};
    for (const element of $("#formPopUp input, select")) {
        // Đọc thông tin field:
        const field = $(element).attr(FIELD_ATTR);
        // Lấy ra value:
        if (field != null && $(element).val() != "") {
            if (field == "Salary") {
                employee[field] = $(element).val().substring(0, $(element).val().length - 5).replace(".", "");
            }
            else {
                employee[field] = $(element).val();
            }
        }
    }

    // Gọi api thực hiện cập nhật:
    $.ajax({
        type: "PUT",
        url: "http://localhost:42714/api/v1/Employees/" + employeeId,
        data: JSON.stringify(employee),
        dataType: "json",
        contentType: "application/json",
        success: function(response) {
            alert("Cập nhật dữ liệu thành công!");
            // load lại dữ liệu:
            loadData(currentPage);
            // Ẩn form chi tiết:
            $("#formPopUp").hide();
        },
        error: function(res) {
            console.log(res);
            if (res.responseJSON != null && (res.responseJSON.ErrorCode == 'e002' || res.responseJSON.ErrorCode == 'e003')) {
                alert(res.responseJSON.ErrorMessage);
            }
            else {
                alert("Cập nhật dữ liệu không thành công");
                // Ẩn form chi tiết:
                $("#formPopUp").hide();
            }
        }
    });
}

/**
 * Gửi yêu cầu thêm nhân viên
 * Author: THVUONG (09/09/2022) 
 */
function addEmployee() {
    // Thu thập dữ liệu và build object:
    let employee = {};
    for (const element of $("#formPopUp input, select")) {
        // Đọc thông tin field:
        const field = $(element).attr(FIELD_ATTR);
        // Lấy ra value:
        if (field != null && $(element).val() != "") {
            if (field == "Salary") {
                employee[field] = $(element).val().substring(0, $(element).val().length - 5).replace(".", "");
            }
            else {
                employee[field] = $(element).val();
            }
        }
    }
   
    // Gọi api thực hiện thêm mới:
    $.ajax({
        type: "POST",
        url: "http://localhost:42714/api/v1/Employees",
        data: JSON.stringify(employee),
        dataType: "json",
        contentType: "application/json",
        success: function(response) {
            alert("Thêm mới dữ liệu thành công!");
            // load lại dữ liệu:
            loadData(currentPage);
            // Ẩn form chi tiết:
            $("#formPopUp").hide();
        },
        error: function(res) {
            console.log(res);
            if (res.responseJSON != null && (res.responseJSON.ErrorCode == 'e002' || res.responseJSON.ErrorCode == 'e003')) {
                alert(res.responseJSON.ErrorMessage);
            }
            else {
                alert("Thêm mới dữ liệu không thành công");
                 // Ẩn form chi tiết:
                $("#formPopUp").hide();
            }
        }
    });
}

/**
 * Kiểm tra validate các đầu vào trong form
 * @return {boolean} 
 * Author: THVUONG (09/09/2022)
 */
function checkValidateForm() {
    let firstValidatedItem;
    let inputs = $("#formPopUp input");
    for (const input of inputs) { 
        // Kiểm tra value:
        if ($(input).parent().hasClass("input--error")) {
            firstValidatedItem = input;
            break;
        }
    }

    if (firstValidatedItem == undefined) {
        return true;
    }
    else {
        $(firstValidatedItem).focus();
        return false;
    }
}

/**
 * Hiển thị form thông tin nhân viên
 * @param {trElementHtml} selectedRow 
 * @param {guid} employeeId
 * Author: THVUONG (09/09/2022)
 */
function showInfoForm(selectedRow, employeeId) {
    // Đặt chế độ là xem và sửa thông tin nhân viên
    formMode = EDIT_EMPLOYEE;

    // Truy vấn api lấy thông tin chi tiết nhân viên bằng id:
    $.ajax({
        method: "GET",
        url: "http://localhost:42714/api/v1/Employees/" + employeeId,
        success: function(employee) {
            // Duyệt tất cả các input:
            for (const element of $('#formPopUp input')) {
                // Đặt lại mặc định
                $(element).parent().siblings(".notice").html("");
                $(element).parent().removeClass("input--default");
                $(element).parent().removeClass("input--error");
                $(element).parent().removeClass("input--focus");
                $(element).parent().removeClass("input--done");

                // Đọc thông tin field:
                const field = $(element).attr(FIELD_ATTR);
                // Gán giá trị cho element:
                $(element).val(employee[field]);
                if (employee[field] != null) {
                    if ($(element).attr("type") == "date") {
                        const date = new Date(employee[field]);
                        const dd = date.getDate() >= 10 ?   `${date.getDate()}`  : `0${date.getDate()}`;
                        const mm = date.getMonth() < 9 ?  `0${date.getMonth() + 1}` :  `${date.getMonth() + 1}`;
                        const yyyy = date.getFullYear();
                        // Gán gái trị cho elementHtml có type="date"
                        $(element).val(`${yyyy}-${mm}-${dd}`);
                    }
                    else if (field == "Salary") {
                        let value = formatMoney(employee[field]);
                        if (value != "") $(element).val(value.substring(0, value.length - 2) + "(VNĐ)");
                    }
                }

                if ($(element).hasClass("input-left__body")) {
                    if ($(element).val() == "") {
                        $(element).parent().addClass("input--default");
                        $(element).parent().children(".input-left__icon").hide();
                    }
                    else {
                        $(element).parent().addClass("input--done");
                        $(element).parent().children(".input-left__icon").show();
                    }
                }
            }

            // Duyệt tất cả các combobox:
            for (const element of $('#formPopUp select')) {
                // Đọc thông tin field:
                const field = $(element).attr(FIELD_ATTR);
                // Gán giá trị cho element:
                $(element).val(employee[field]);
            }
        
            // Hiển thị form:
            $("#formPopUp").show();
        
            // Focus vào ô input mã nhân viên:
            $("#employeeCodeInput").focus();
        },
        error: function(res) {
            console.log(res);
            alert("Server không phản hồi");
        }
    });
}

/**
 * Hiển thị form nhân bản thông tin nhân viên
 * Author: THVUONG (09/09/2022)
 */
function showDuplicateForm() {
    // Đặt chế độ là thêm mới nhân viên
    formMode = ADD_EMPLOYEE;

    // Duyệt tất cả các input combobox:
    for (const element of $('#formPopUp input')) {
        // Đặt lại mặc định
        $(element).parent().siblings(".notice").html("");
        $(element).parent().removeClass("input--default");
        $(element).parent().removeClass("input--error");
        $(element).parent().removeClass("input--focus");
        $(element).parent().removeClass("input--done");

        // Đọc thông tin field:
        const field = $(element).attr(FIELD_ATTR);
        // Gán giá trị cho element:
        $(element).val(employee[field]);
        if (employee[field] != null) {
            if ($(element).attr("type") == "date") {
                const date = new Date(employee[field]);
                const dd = date.getDate() >= 10 ?   `${date.getDate()}`  : `0${date.getDate()}`;
                const mm = date.getMonth() < 9 ?  `0${date.getMonth() + 1}` :  `${date.getMonth() + 1}`;
                const yyyy = date.getFullYear();
                // Gán gái trị cho elementHtml có type="date"
                $(element).val(`${yyyy}-${mm}-${dd}`);
            }
            else if (field == "Salary") {
                let value = formatMoney(employee[field]);
                if (value != "") $(element).val(value.substring(0, value.length - 2) + "(VNĐ)");
            }
        }

        if ($(element).hasClass("input-left__body")) {
            if ($(element).val() == "") {
                $(element).parent().addClass("input--default");
                $(element).parent().children(".input-left__icon").hide();
            }
            else {
                $(element).parent().addClass("input--done");
                $(element).parent().children(".input-left__icon").show();
            }
        }
    }

    // Duyệt tất cả các combobox:
    for (const element of $('#formPopUp select')) {
        // Đọc thông tin field:
        const field = $(element).attr(FIELD_ATTR);
        // Gán giá trị cho element:
        $(element).val(employee[field]);
    }

    // Hiển thị form:
    $("#formPopUp").show();

    // Focus vào ô input mã nhân viên:
    $("#employeeCodeInput").focus();
}

/**
 * Hiển thị form thêm nhân viên
 * Author: THVUONG (09/09/2022)
 */
function showNewForm() {
    // Đặt chế độ là thêm mới nhân viên
    formMode = ADD_EMPLOYEE;

    // Duyệt tất cả các input:
    for (const element of $('#formPopUp input')) {
        // Làm rỗng nội dung cho element:
        $(element).val("");
        // Đặt lại mặc định
        $(element).parent().siblings(".notice").html("");
        $(element).parent().removeClass("input--done");
        $(element).parent().removeClass("input--error");
        $(element).parent().removeClass("input--focus");
        $(element).parent().addClass("input--default");
    }

    // Duyệt tất cả các combobox:
    for (const element of $('#formPopUp select')) {
        // Làm rỗng nội dung cho element:
        $(element).val("");
    }

    // Truy vấn api lấy mã nhân viên mới:
    $.ajax({
        method: "GET",
        url: "http://localhost:42714/api/v1/Employees/newEmployeeCode",
        success: function(newEmployeeCode) {
            // Hiển thị form:
            $("#formPopUp").show();

            // Focus vào ô input mã nhân viên:
            $("#employeeCodeInput").focus();
            
            // Gán mã nhân viên mới:
            $("#employeeCodeInput").val(newEmployeeCode);
        },
        error: function(res) {
            console.log(res);
            alert("Server không phản hồi");
        }
    });
}

/**
 * Lấy dữ liệu và cập nhật bảng
 * @param {number} pageNumber
 * Author: THVUONG (09/09/2022)
 */
function loadData(pageNumber) {
    // Tạo phần lọc trong truy vấn api:
    const filter = `keyword=${keyword||''}&positionId=${positionId||''}&departmentId=${departmentId||''}&pageSize=${pageSize}&pageNumber=${pageNumber}`;

    // Truy vấn api:
    $.ajax({
        type: "GET",
        async: false,
        url: "http://localhost:42714/api/v1/Employees?" + filter,
        success: function(res) {

            const totalRecord = parseInt(res["TotalRecord"]||0);
            const totalPage = Math.ceil(totalRecord / pageSize);
            const data = res["Data"];

            if (data.length > 0 || totalRecord == 0) {

                // Làm rỗng bảng danh sách:
                $('#table tbody').empty();

                // Gán giá trị số thứ tự trang nếu là trang cuối 
                if (pageNumber == LAST_PAGE) pageNumber = totalPage;

                // Xác định số thứ tự nhân viên bắt đầu sẽ hiển thị:
                let index = totalRecord > 0 ? (pageNumber - 1) * pageSize + 1 : 0;

                // Xác định số thứ tự nhân viên cuối cùng sẽ hiển thị:
                const endIndex = totalRecord > 0 ? index + data.length - 1 : 0;

                // Hiển thị thứ tự các nhân viên trong bảng trên tổng số nhân viên cùng điều kiện tìm kiếm:
                $('#paging div b').text(`${index>9?index:"0"+index}-${endIndex>9?endIndex:"0"+endIndex}/${totalRecord>9?totalRecord:"0"+totalRecord}`);

                // Xử lý dữ liệu từng đối tượng:
                for (const employee of data) {

                    // duyệt từng cột trong tiêu đề:
                    let trElement = $('<tr></tr>');

                    for (const th of  $('#table th')) {

                        // Lấy ra field tương ứng với các cột:
                        const field = $(th).attr(FIELD_ATTR);

                        // Lấy giá trị tương ứng với field trong đối tượng:
                        let value = field != "index" ? employee[field] : index;

                        // Lấy ra format tương ứng với các cột:
                        const format = $(th).attr("format");

                        // Lấy thêm thông tin theo format
                        let classAlign = "";
                        switch (format) {
                            case "date":
                                value = value ? formatDate(value) : "";
                                classAlign = "text-align--center";
                                break;
                            case "money":
                                value = formatMoney(value);
                                classAlign = "text-align--right";
                                break;
                            case "status":
                                if (value == '0') value = "Đang làm việc"; 
                                else if (value == '1') value = "Đang nghỉ phép";
                                else if (value == '2') value = "Khác";
                                else value = "";
                                break;
                            default:
                                break;
                        }

                        // Tạo tdHtml và đẩy vào trHMTL:
                        trElement.append(`<td class='${classAlign} body-content'>${value||""}</td>`);
                        
                    }

                    index++;
                    // Ghi lại thông tin
                    $(trElement).data('employee', employee);
                    // Đẩy trElement vào tbody
                    $('#table tbody').append(trElement);
                }

                // Làm mới các nút hiển thị số phân trang:
                $("#paging .navigation .number-group").empty();
                if (pageNumber <= PAG_NUMBER_MAX_SIZE) {
                    pageMinNumber = 1;
                    pageMaxNumber = PAG_NUMBER_MAX_SIZE;
                }
                else if (pageNumber == totalPage) {
                    pageMinNumber = totalPage - PAG_NUMBER_MAX_SIZE + 1;
                    pageMaxNumber = totalPage;
                }
                else if (pageNumber > pageMaxNumber) {
                    pageMinNumber = pageNumber - PAG_NUMBER_MAX_SIZE + 1;
                    pageMaxNumber = pageNumber;
                }
                else if (pageNumber < pageMinNumber) {
                    pageMinNumber = pageNumber;
                    pageMaxNumber = pageNumber + PAG_NUMBER_MAX_SIZE - 1;
                }
                for(let i = pageMinNumber; i <= Math.min(totalPage, pageMaxNumber); i++) {
                    $("#paging .navigation .number-group").append(`<div class="number body-content">${i}</div>`);
                }
                // Xác định trang được chọn:
                $($("#paging .navigation .number-group .number")[pageNumber - pageMinNumber]).addClass("number--selected");
                currentPage = pageNumber;
            }
        },
        error: function(res) {
            console.log(res);
            // Gán giá trị kích thước trang trước khi đổi
            if (pageSize != pagePrevSize) {
                pageSize = pagePrevSize;
                $("#paging select").val("" + pageSize);
            }
            alert("Tải dữ liệu không thành công");
        }
    })
}

/**
 * Lấy dữ liệu và cập nhật bảng
 * Author: THVUONG (09/09/2022)
 */
function loadDepartment() {
    // Truy vấn api:
    $.ajax({
        type: "GET",
        async: false,
        url: "http://localhost:42714/api/v1/Departments",
        success: function(res) {
            // Xử lý dữ liệu từng đối tượng:
            for (const department of res) {
                // Tạo optionHtml và đẩy vào selectHMTL:
                $("[name=departmentComboBox]").append(`<option value="${department["DepartmentId"]}">${department["DepartmentName"]}</option>`);
            }
        },
        error: function(res) {
            console.log(res);
            alert("Tải dữ liệu phòng ban không thành công");
        }
    })
}

/**
 * Lấy dữ liệu và cập nhật bảng
 * Author: THVUONG (09/09/2022)
 */
function loadPosition() {
    // Truy vấn api:
    $.ajax({
        type: "GET",
        async: false,
        url: "http://localhost:42714/api/v1/Positions",
        success: function(res) {
            // Xử lý dữ liệu từng đối tượng:
            for (const position of res) {
                // Tạo optionHtml và đẩy vào selectHMTL:
                $("[name=positionComboBox]").append(`<option value="${position["PositionId"]}">${position["PositionName"]}</option>`);
            }
        },
        error: function(res) {
            console.log(res);
            alert("Tải dữ liệu vị trí không thành công");
        }
    })
}

