export default class Resource {
    static StatusBusiness = {
        All: "Tất cả",
        Trading: "Đang kinh doanh",
        StopTrading: "Ngừng kinh doanh",
    };

    // Hiển thị
    static Display = {
        All: "Tất cả", // Tất cả
        Yes: "Có", // Có
        No: "Không", // Không
    }

    static DEFAULT_PICTUREID = "bdebe2cb-bccb-413e-aa18-089372975423";

    // Đối tượng
    static EMPLOYEE = "nhân viên";

    // Thông báo thành công
    static ADD_DATA_SUCCESS = "Thêm dữ liệu thành công.";
    static EDIT_DATA_SUCCESS = "Sửa dữ liệu thành công.";
    static DUPLICATE_DATA_SUCCESS = "Nhân bản dữ liệu thành công.";
    static DELETE_DATA_SUCCESS = "Xóa dữ liệu thành công.";
    static EXPORT_DATA_SUCCESS = "Xuất bảng dữ liệu thành công.";

    // Lỗi API
    static ERROR_MESSAGE_VN = "Có vấn đề xảy ra, Vui lòng liên hệ MISA.";

    // Thông báo lỗi ở form thông báo
    static CANNOT_EMPTY = "Không được để trống các trường bắt buộc.";
    static ERROR_CODE = "Mã nhân viên sai định dạng.";
    static EMPLOYEE_CODE = "Mã nhân viên";
    static WRONG_DATE = "Ngày tháng không hợp lệ.";
    static WRONG_EMAIL_FORMAT = "Email không đúng định dạng.";

    // Thông báo lỗi ở popup
    static NOT_EMPTY = "không được để trống.";
    static WRONG_FORMAT = "sai định dạng.";
    static CONTAIN_ONLY_NUMBER = "chỉ chứa chữ số.";
    static CODE_EXIST = "đã tồn tại trong hệ thống, vui lòng kiểm tra lại.";
    static WRONG_DEPARTMENT = "không tồn tại";
    static CONFIRM_DELETE = "Bạn có chắc chắn muốn xóa hàng hóa";
    static CONFIRM_MULTI_DELETE = "Bạn có thực sự muốn xóa";
    static CONFIRM_SAVE = "Bạn có thực sự muốn lưu Nhân viên";
    static CONFIRM_EDIT = "Bạn có thực sự muốn sửa Nhân viên";
    static CONFIRM_ADD_DATA_CHANGED =
        "Dữ liệu đã bị thay đổi. Bạn có muốn cất không?";
    static CONFIRM = "không?";

    //Chọn bao nhiêu bản ghi trên 1 trang
    static Record_Per_Page = "bản ghi trên 1 trang";

    // Các trạng thái popup
    static Popup = {
        Warning: "warning-popup",
        Notify: "notify-popup",
        Confirm: "confirm-popup",
        Export: "export-popup",
    }
}