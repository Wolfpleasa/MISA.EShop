<template>
  <div :class="['eshop-inp', { description: FieldName == 'Description' }]">
    <div class="d-flex" v-if="labelText.length > 0">
      {{ labelText }}
      <span v-if="obligate == 'true'"><span class="cl-red">*</span></span>
      <div v-if="FieldName == 'PurchasePrice'" class="quiz"></div>
    </div>
    <ToolTip :hideToolTip="hideToolTip" />
    <input
      :tabindex="tabindex"
      :type="type"
      :placeholder="placeholder"
      :FieldName="FieldName"
      :maxlength="maxlength"
      :class="['textbox-default', subClass, { borderRed: hasBorderRed }]"
      ref="inputREF"
      :obligate="obligate"
      :onlyHasNumber="onlyHasNumber"
      v-model="inputValue"
      @input="onInput($event.target.value)"
      @blur="onBlur($event.target.value)"
    />
  </div>
</template>

<script>
import { mixin as clickaway } from "vue-clickaway";

import ToolTip from "./BaseToolTip.vue";
import CommonFn from "../../common/common1";

export default {
  mixins: [clickaway],
  name: "BaseInput",
  components: {
    ToolTip,
  },

  props: {
    labelText: String,
    tabindex: String,
    type: String,
    placeholder: String,
    FieldName: String,
    initValue: String,
    subClass: String,
    // xác định tự động cho con trỏ chuột vào ô mã nhân viên
    autoFocus: String,
    reFocus: Boolean,
    // Xác định các trường chỉ chứa số
    onlyHasNumber: String,
    // Xác định các trường bắt buộc
    obligate: String,
    // Độ dài tối đa
    maxlength: String,
  },
  data() {
    return {
      // Giá trị/Nội dung hiện tại
      inputValue: "",
      // Ẩn/hiện viền đỏ
      hasBorderRed: false,
      // Ẩn/hiện tooltip
      hideToolTip: true,
    };
  },

  methods: {
    /**
     * Hàm bind dữ liệu vào input
     * Ngọc 7/8/2021
     */
    onInput(inputValue) {
      let me = this;
      //emit thắng vào v-model của cha
      me.$emit("input", inputValue);

      //format ô input tiền lương
      if (me.FieldName.includes("Price")) {
        me.$emit("convertMoney", me.FieldName);
      }

      // Nếu các ô bắt buộc nhập đã có dữ liệu thì bỏ viền đỏ
      if (inputValue != "" && me.obligate == "true") {
        me.hasBorderRed = false;
        me.hideToolTip = true;
      }

      // Nếu các ô chỉ chứa chữ số đúng định dạng rồi thì bỏ viền đỏ
      if (
        !me.isNumber(CommonFn.formatNumber(inputValue)) &&
        me.onlyHasNumber == "true"
      ) {
        me.hasBorderRed = false;
        me.hideToolTip = true;
      }
    },

    /**
     * Hàm kiểm tra khi vừa nhập liệu xong
     * Ngọc 10/8/2021
     */
    onBlur(inputValue) {
      let me = this;
      // Nếu các ô bắt buộc không đã có dữ liệu thì hiện viền đỏ và tooltip
      if (inputValue == "" && me.obligate == "true") {
        me.hasBorderRed = true;
        me.hideToolTip = false;
      }

      // Nếu các ô chỉ chứa chữ số đúng định dạng rồi thì bỏ viền đỏ
      if (
        !me.isNumber(CommonFn.formatNumber(inputValue)) &&
        me.onlyHasNumber == "true"
      ) {
        me.hasBorderRed = true;
        me.hideToolTip = false;
      }
    },

    /**
     * Hàm kiểm tra bắt buộc nhập các dữ liệu bắt buộc
     * Ngọc 8/8/2021
     */
    isRequired() {
      let me = this,
        val = me.$refs.inputREF.value;

      if (me.obligate == "true" && val == "") {
        // Thêm border đỏ
        me.hasBorderRed = true;
        // hiện tooltip
        me.hideToolTip = false;

        return false;
      }
      //bỏ boder đỏ
      me.hasBorderRed = false;

      return true;
    },

    /**
     * Hàm kiểm tra các trường chỉ chứa chữ số
     * Ngọc 8/8/2021
     */
    ContainNumber() {
      let me = this,
        number = me.$refs.inputREF.value;
      let val = me.isNumber(me.formatNumber(number));

      if (!val && me.onlyHasNumber == "true") {
        //thêm border đỏ
        me.hasBorderRed = true;
        // hiện tooltip
        me.hideToolTip = false;

        return false;
      }
      //bỏ boder đỏ
      me.hasBorderRed = false;

      return true;
    },

    /**
     * Hàm loại bỏ lỗi và tooltip cảnh báo khi mở lại form
     * Ngọc 10/8/2021
     */
    removeError() {
      let me = this;
      // Bỏ boder đỏ
      me.hasBorderRed = false;
      // Ẩn tooltip
      me.hideToolTip = true;
    },

    /**
     * Hàm kiểm tra chuổi chỉ chứa chữ số
     * Ngọc 24/07/2021
     */
    isNumber(number) {
      let part = /[^0-9]/g,
        //mảng res chứa các phần tử thừa
        res = number.match(part);

      //Nếu res null (tức là không có phần tử thừa) => number truyền vào chỉ chứa số=> true
      if (!res) {
        return true;
      }
      return false;
    },
  },
  watch: {
    initValue: function() {
      this.inputValue = this.initValue;
    },

    reFocus: function() {
      if (this.autoFocus == "true") {
        this.$refs.inputREF.focus();
      }
    },
  },
};
</script>

<style scoped></style>
