import Enumeration from "./enumeration";
import Resource from "./resourceVN";

export default class CommonFn {

    /**
     * Hàm format số tiền
     * Created By: Ngọc 28/90/2021
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
     * Created By: Ngọc 28/90/2021
     */
    static formatNumber(number) {
        return number.replaceAll(".", "");
    }

    /**
     * Hàm kiểm tra chuổi chỉ chứa chữ số
     * Created By: Ngọc 31/09/2021
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

    /**
     * Hàm chuyển từ có dấu sang ko dấu
     * Created By: Ngọc 05/10/2021
     * @param {} str 
     * @returns 
     */
    static removeVietnameseTones(str) {
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");
        str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
        str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
        str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
        str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
        str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
        str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
        str = str.replace(/Đ/g, "D");
        // Some system encode vietnamese combining accent as individual utf-8 characters
        // Một vài bộ encode coi các dấu mũ, dấu chữ như một kí tự riêng biệt nên thêm hai dòng này
        str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // ̀ ́ ̃ ̉ ̣  huyền, sắc, ngã, hỏi, nặng
        str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // ˆ ̆ ̛  Â, Ê, Ă, Ơ, Ư
        // Remove extra spaces
        // Bỏ các khoảng trắng liền nhau
        str = str.replace(/ + /g, " ");
        str = str.trim();

        return str;
    }

    /**
     * Hàm tự tăng SKUCode
     * Created By: Ngọc 05/10/2021
     * @param {} name 
     * @param {*} skuCode 
     * @returns 
     */
    static increaseSKUCode(name, skuCode) {
        let arrName = name.split(" ");
        let code = "";
        let sku = "";

        for (let n of arrName) {
            sku = sku + n.substring(0, 1).toUpperCase();
        }

        if (skuCode == null) {
            code = "01";
        } else {
            //code = eval(`${skuCode.substring(sku.length)} + 1`);

            code = Number.parseInt(skuCode.substring(sku.length));
            code += 1;
            if (code < 10) {
                code = "0" + code;
            }
        }

        return sku + code;
    }
}