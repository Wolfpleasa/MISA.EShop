export default class Enumeration {

    // Giới tính
    static Gender = {
        Female: 0, // Nữ
        Male: 1, // Nam
        Other: 2 // Khác
    }

    // Hiển thị
    static Display = {
        All: 2, // Tất cả
        Yes: 1, // Có
        No: 0, // Không
    }

    //Trạng thái
    static StatusBusiness = {
        All: 2,
        Trading: 1,
        StopTrading: 0,
    };

    // Form mode
    static FormMode = {
        None: -1,
        Add: 0, // Thêm
        Edit: 1, // Sửa
        Duplicate: 2, // Nhân bản
        Delete: 3, // Xóa
    }

    // phân biệt các button trong form detail
    static NotifyMode = {
        None: -1,
        Save: 0, // Cất 
        Cancel: 1, // Hủy
        SaveAndAdd: 2, // Cất và thêm
    }

    // Các trạng thái Api
    static Status = {
        OK: 200, // Api có kết quả
        NoContent: 204, // Api có kết quả trống
        BadRequest: 400, // Dữ liệu gửi lên bị lỗi
        NotFound: 404, // Không tìm thấy
    }

    // Các trạng thái popup
    static Popup = {
        Warning: "warning-popup",
        Notify: "notify-popup",
        Confirm: "confirm-popup",
        Export: "export-popup",
    }

    // Nội dung các nút 
    static ButtonText = {
        Yes: "Có",
        No: "Không",
        Save: "Cất",
        SaveAndAdd: "Cất và thêm",
        Cancel: "Hủy",
        Store: "Lưu",
    }

    // Các trạng thái của form cảnh báo và toast
    static Message = {
        Error: "message-red",
        Notify: "message-yellow",
        Complete: "message-green",
    }

    // Các trạng thái response
    static Response = {
        Yes: "Có",
        Save: "Cất",
    }
}