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
    };

    static OperatorWord = {
        Contain: 0, //Chứa
        Equal: 1, //Bằng
        StartWith: 2, // Bắt đầu bằng
        EndWith: 3, // Kết thúc bằng
        NotContain: 4, // Không chứa
    }

    static OperatorNumber = {
        Lower: 1, //Bằng
        Equal: 0, //Nhỏ hơn 
        LE: 2, // Nhỏ hơn hoặc bằng
        Greater: 3, // Lớn hơn
        GE: 4, // Lớn hơn hoặc bằng
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
        NoContent: 204, // Api có kết quả rỗng
        BadRequest: 400, // Dữ liệu gửi lên bị lỗi
        NotFound: 404, // Không tìm thấy
        ServerError: 500, // Lỗi server
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
}