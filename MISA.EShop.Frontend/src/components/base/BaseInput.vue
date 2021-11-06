<template>
  <div :class="['eshop-inp', { description: FieldName == 'Description' }]">
    <div class="d-flex" v-if="labelText.length > 0">
      {{ labelText }}
      <span v-if="obligate == 'true'"><span class="cl-red">*</span></span>
      <div v-if="FieldName == 'PurchasePrice'" class="quiz"></div>
    </div>
    <div class="input-tooltip">
      <textarea
        v-if="FieldName == 'Description'"
        :tabindex="tabindex"
        :type="type"
        :FieldName="FieldName"
        :maxlength="maxlength"
        v-model="inputValue"
        @input="onInput($event.target.value)"
        :rows="rows"
        :cols="cols"
      >
      </textarea>

      <money
        v-else-if="FieldName.includes('Price')"
        :tabindex="tabindex"
        :type="type"
        v-bind="money"
        :onlyHasNumber="onlyHasNumber"
        :FieldName="FieldName"
        :maxlength="maxlength"
        :class="['textbox-default', subClass]"
        min="0"
        v-model="moneyValue"
        @input="inputMoney()"
      >
      </money>

      <input
        v-else
        :tabindex="tabindex"
        :type="type"
        :placeholder="placeholder"
        :FieldName="FieldName"
        :maxlength="maxlength"
        :class="[
          'textbox-default',
          subClass,
          { 'border-red width-smaller': hasBorderRed },
        ]"
        :obligate="obligate"
        ref="inputREF"
        v-model="inputValue"
        @input="onInput($event.target.value)"
        @blur="onBlur($event.target.value)"
      />
      <RedPoint :hideRedPoint="hideRedPoint" :redPointText="redPointText" />
    </div>
  </div>
</template>

<script>
import { mixin as clickaway } from "vue-clickaway";
import { Money } from "v-money";

import RedPoint from "./BaseRedPoint.vue";
//import CommonFn from "../../common/common1";
//import ResourceVN from "../../common/resourceVN";

export default {
  mixins: [clickaway],
  name: "BaseInput",
  components: {
    RedPoint,
    Money,
  },

  props: {
    labelText: String,
    tabindex: String,
    type: String,
    placeholder: String,
    FieldName: String,
    initValue: String,
    initMoney: Number,
    subClass: String,
    // xác định tự động cho con trỏ chuột vào ô mã nhân viên
    autoFocus: String,
    reFocus: Boolean,
    // Xác định các trường chỉ chứa số
    onlyHasNumber: Boolean,
    // Xác định các trường bắt buộc
    obligate: String,
    // Độ dài tối đa
    maxlength: String,
    rows: String,
    cols: String,

    min: String,
  },
  data() {
    return {
      // Giá trị/Nội dung hiện tại
      inputValue: "",
      // Giá trị tiền hiện tại
      moneyValue: 0,
      // Ẩn/hiện viền đỏ
      hasBorderRed: false,
      // Ẩn/hiện chấm than đỏ
      hideRedPoint: true,
      //Nội dung
      redPointText: "",
      money: {
        thousands: ".",
        precision: 0,
        masked: false,
      },
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

      // Nếu các ô bắt buộc nhập đã có dữ liệu thì bỏ viền đỏ
      if (inputValue != "" && me.obligate == "true") {
        me.hasBorderRed = false;
        me.hideRedPoint = true;
      }
    },

    inputMoney() {
      let me = this;

      //Các ô tiền không nhận chữ cái
      if (me.onlyHasNumber) {
        //format ô input tiền để hiện thị lên
        me.$emit("input", this.moneyValue);
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
        me.hideRedPoint = false;
        //me.redPointText = `${ResourceVN.CANNOT_EMPTY}`;
        me.redPointText = "Vui lòng điền vào trường này.";
      }

      if (inputValue != "" && me.FieldName == "ProductName") {
        me.$emit("autoGenSKUCode");
      }
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
        me.hideRedPoint = false;

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
      me.hideRedPoint = true;
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
    initValue: function () {
      this.inputValue = this.initValue;
    },

     initMoney: function () {
      this.moneyValue = this.initMoney;
    },

    reFocus: function () {
      if (this.autoFocus == "true") {
        this.$refs.inputREF.focus();
      }
    },
  },
};
</script>

<style scoped></style>
