import Enumeration from "./enumeration";
import Resource from "./resourceVN";

export default class CommonFn {

    /**
     * Hàm format số tiền
     * Created By: Ngọc 28/9/2021
     */
    static formatMoney(money) {
        if (money && !isNaN(money)) {
            return money.toString().replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g, "$1.");
        } else {
            return money;
        }
    }

    /**
     * Hàm format chữ số về bình thường
     * Created By: Ngọc 06/09/2021
     */
    static formatNumber(number) {
        return number.replaceAll(".", "");
    }

    /**
     * Hàm kiểm tra chuổi chỉ chứa chữ số
     * Created By: Ngọc 31/08/2021
     */
    static isNumber(number) {
        let part = /[^0-9]/g,
            res = number.match(part);
        if (!res) {
            return true;
        }
        return false;
    }

    /**
     * Hàm kiểm tra mã nhân viên có đúng định dạng không
     * Created By: Ngọc 31/08/2021
     */
    static isCodeFormat(code) {
        let regex = new RegExp("(NV-[0-9]+)");
        return regex.test(code);
    }

    /**
     * Format ngày tháng để đẩy lên table vs input
     * Created By: Ngọc 28/08/2021
     */
    static formatDate(dateSrc) {
        let date = new Date(dateSrc),
            year = date.getFullYear().toString(),
            month = (date.getMonth() + 1).toString().padStart(2, "0"),
            day = date
            .getDate()
            .toString()
            .padStart(2, "0");

        return `${day}/${month}/${year}`;
    }

    /**
     * Kiểm tra xem có phải dạng date không và có vượt quá ngày tháng hiện tại không
     * Created By: Ngọc 31/08/2021
     * Modified By: Ngọc 02/09/2021
     */
    // static isDateFormat(date) {
    //     let regex = new RegExp(
    //         "(0[1-9]|[1-2][0-9]|3[0-1])[-](0[1-9]|1[0-2])[-](2020|(19[0-9]{2}|20[0-1][0-9]))" +
    //         "|(([0][1-9]|[1-2][0-9]|3[0-1])[-](0[1-8])|((0[1-3])[-]09))[-]2021"
    //     );
    //     return regex.test(date);
    // }

    /**
     * Kiểm tra xem có phải dạng date không và có vượt quá ngày tháng hiện tại không
     * Created By: Ngọc 31/08/2021
     * Modified By: Ngọc 02/09/2021
     */
    static isDateFormat(date) {
        let regex = new RegExp(
            "((19[0-9]{2}|20[0-1][0-9]|2020)[-](0[1-9]|1[0-2])[-]([0][1-9]|[1-2][0-9]|3[0-1]))|(2021[-](0[1-8][-]([0][1-9]|[1-2][0-9]|3[0-1]))|09[-]0[1-3])"
        );
        return regex.test(date);
    }

    /**
     * Chuyển đổi ngày tháng để đẩy lên api
     * Created By: Ngọc 28/08/2021
     */
    static convertDate(dateSrc) {
        let dateString = "";
        if (dateSrc) {
            let date = new Date(dateSrc),
                year = date.getFullYear().toString(),
                month = (date.getMonth() + 1).toString().padStart(2, "0"),
                day = date
                .getDate()
                .toString()
                .padStart(2, "0");

            dateString = `${year}-${month}-${day}`;
        } else {
            dateString = null;
        }
        return dateString;
    }

    /**
     * Lấy giá trị của một enum
     * Created By: Ngọc 27/09/2021
     * @param {*} data
     * @param {*} enumName
     * @returns
     */
    static getValueEnum(data, enumName) {
        let enumeration = Enumeration[enumName],
            resource = Resource[enumName];

        for (var propName in enumeration) {
            if (enumeration[propName] == data) {
                data = resource[propName];
            }
        }

        return data;
    }
}