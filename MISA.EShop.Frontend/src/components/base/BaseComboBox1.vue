
<template>
  <div :class="['eshop-cbb', className]" v-on-clickaway="closeComboBox">
    <div :class="{ 'd-none': !labelText }">
      {{ labelText }}
    </div>

    <div class="combo-box-tooltip">
      <div
        :class="['combo-box-select mt-4', { 'width-smaller': hasBorderRed }]"
        v-on:keydown="keydownOnSelect($event)"
        v-on:keyup.enter="chooseComboBoxItemByKey()"
        ref="myComboBox"
      >
        <input
          :tabindex="tabindex"
          :placeholder="placeholder"
          class="inp"
          v-model="currentName"
          ref="myInput"
          @input="onInput($event.target.value)"
          @blur="onBlur($event.target.value)"
          @focus="onFocus()"
        />
        <div class="select-arrow" @click="selectOnClick">
          <div :class="['arrow', rotate ? 'rot-180' : 'rot-0']"></div>
        </div>
        <div class="add-option" @click="addInfomation">
          <div class="icon-plus"></div>
        </div>
      </div>

      <RedPoint :hideRedPoint="hideRedPoint" :redPointText="redPointText" />
    </div>

    <div :class="['combo-box', { 'd-none': dnone }]">
      <slide-up-down :active="active" :duration="1000">
        <div
          v-for="(item, index) in filterItems"
          :key="item[itemId]"
          :class="[
            'combo-box-item',
            currentId == item[itemId] ? 'bg-select' : '',
            index == currentIndex ? 'bg-hover' : '',
          ]"
          @click="chooseComboBoxItem(item[itemId], item[itemName], index)"
        >
          {{ item[itemName] }}
        </div>
      </slide-up-down>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { mixin as clickaway } from "vue-clickaway";

import Constant from "../../common/constant1.js";
import ResourceVN from "../../common/resourceVN.js";

import RedPoint from "./BaseRedPoint.vue";
export default {
  mixins: [clickaway],
  name: "BaseComboBox",

  components: {
    RedPoint,
  },

  props: {
    className: String,
    Url: String,
    itemId: String,
    itemName: String,
    selectedId: String,
    tabindex: String,
    labelText: String,
    placeholder: String,
    FieldName: String,
  },

  data() {
    return {
      label: this.labelText,
      //các department,position
      items: [],
      filterItems: [],
      // quay mũi tên
      rotate: false,
      // ẩn/hiện các item
      dnone: true,
      // tên hiển thị
      currentName: this.defaultName,
      currentIndex: -1,
      currentId: "",
      //slidedown/up
      active: false,
      // Ẩn/hiện viền đỏ
      hasBorderRed: false,
      // Ẩn/hiện chấm than đỏ
      hideRedPoint: true,
      //Nội dung
      redPointText: "",
    };
  },
  methods: {
    /**
     * Hàm mở form thêm thông tin cơ bản
     * Created By: Ngọc 01/10/2021
     */
    addInfomation() {
      try {
        this.$emit("addInfomation", false, this.FieldName);
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm focus vào ô nhập
     * Created By: Ngọc 28/09/2021
     */
    focusOnField() {
      this.$refs.myInput.focus();
    },

    /**
     * Bắt sự kiện nhập vào ô input
     * Created By: Ngọc 28/09/2021
     */
    onInput(value) {
      try {
        let me = this;
        me.rotate = true;
        me.dnone = false;
        me.active = true;
        me.filterItems = me.items.filter((item) => {
          return item[me.itemName].toLowerCase().includes(value.toLowerCase());
        });

        if (me.filterItems.length == 0) {
          me.hideRedPoint = false;
          me.hasBorderRed = true;
          me.redPointText = `${ResourceVN.ERROR_TEXT}`;
        } else {
          me.hideRedPoint = true;
          me.hasBorderRed = false;
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm focus vào ô nhập
     * Created By: Ngọc 28/09/2021
     */
    onFocus() {
      try {
        let me = this;
        me.hasNotValid = false;
        me.hideRedPoint = true;
        this.rotate = true;
        this.dnone = false;
        this.active = true;
        me.$refs.myComboBox.style.borderColor = "#636dde";
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Hàm blur ô nhập
     * Created By: Ngọc 28/09/2021
     */
    onBlur(value) {
      let me = this;
      this.$refs.myComboBox.style.borderColor = "#c3c3c3";
      this.items.forEach((item) => {
        if (item[me.itemName] == value) {
          me.currentId = item[me.itemId];
          me.$emit("chooseComboBoxItem", me.itemId, me.currentId);
        }
      });
    },

    /**
     * Hàm ngăn chặn sự kiện mặc định của nút tab, dùng để thực hiện được nút enter
     * Created By: Ngọc 28/09/2021
     */
    keydownOnSelect(event) {
      try {
        if (event.code == "Enter") {
          event.preventDefault();
        }
        if (event.code == "Tab") {
          this.rotate = false;
          this.dnone = true;
          this.active = false;
        }
        if (event.code == "ArrowDown") {
          event.preventDefault();
          this.currentIndex = this.currentIndex < 0 ? -1 : this.currentIndex;
          this.currentIndex = (this.currentIndex + 1) % this.items.length;
        } else if (event.code == "ArrowUp") {
          event.preventDefault();
          this.currentIndex = this.currentIndex < 0 ? 0 : this.currentIndex;
          this.currentIndex =
            this.currentIndex == 0
              ? this.items.length - 1
              : this.currentIndex - 1;
        }
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Sự kiện chọn 1 lựa chọn bằng phím
     * Created By: Ngọc 28/09/2021
     */
    chooseComboBoxItemByKey() {
      try {
        let item = this.filterItems[this.currentIndex],
          itemName = item[this.itemName],
          itemValue = item[this.itemId];
        this.chooseComboBoxItem(itemValue, itemName, this.currentIndex);
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Sự kiện chọn 1 lựa chọn
     * Created By: Ngọc 28/09/2021
     */
    chooseComboBoxItem(itemValue, itemName, index) {
      try {
        let me = this;
        me.hasNotValid = false;
        me.hideRedPoint = true;
        me.$refs.myInput.focus();
        me.currentIndex = index;
        me.currentId = itemValue;
        me.selectOnClick();
        me.currentName = itemName;
        me.$emit("chooseComboBoxItem", this.itemId, itemValue);
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Sự kiện bấm để hiện/ ẩn ComboBox
     * Created By: Ngọc 28/09/2021
     */
    selectOnClick() {
      try {
        let me = this;
        me.rotate = !me.rotate;
        me.dnone = !me.dnone;
        me.active = !me.active;
        if (me.rotate) {
          me.$refs.myComboBox.style.borderColor = "#636dde";
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Sự kiện đóng ComboBox(sử dụng vue-clickaway)
     * Created By: Ngọc 28/09/2021
     */
    closeComboBox() {
      try {
        this.rotate = false;
        this.dnone = true;
        this.active = false;
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Xem hàm sửa có gọi không để lưu giá trị vào ComboBox
     * Created By: Ngọc 28/09/2021
     */
    setValueComboBox() {
      try {
        let me = this;
        // nếu selectedId(UnitId) mà tồn tại(tức là được Productlist truyền vào => form sửa dùng)
        if ((me.selectedId + "").length > 0) {
          // duyệt từng items(unit)
          me.items.forEach(function (item, index) {
            // nếu selectedId(UnitId) từ cha truyền vào mà trùng với itemId(UnitId) trong list
            if (me.selectedId == item[me.itemId]) {
              // gán currentId = selectedId (để hàng tương ứng được tích)
              me.currentId = me.selectedId;
              me.currentIndex = index;
              // hiện thị tên của itemName(UnitName) của hàng được chọn
              me.currentName = item[me.itemName];
            }
          });
        } else {
          // form thêm dùng
          // không hàng nào được tích
          me.currentId = "";
          me.currentIndex = -1;
          // không hiển thị tên nào cả
          me.currentName = "";
        }
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Hàm load lại tất cả dữ liệu
     * Created By: Ngọc 01/10/2021
     */
    getAll() {
      let me = this;
      switch (me.itemName) {
        default:
          axios
            .get(`${Constant.LocalUrl}/${me.Url}`)
            .then((res) => {
              me.items = res.data;
              me.filterItems = res.data;
              this.setValueComboBox();
            })
            .catch((res) => {
              console.log(res);
            });
          break;
      }
      me.currentName = me.defaultName;
    },
  },

  created() {
    this.getAll();
  },

  watch: {
    selectedId: function () {
      this.getAll();
    },
  },
};
</script>

<style></style>

            