<template>
  <div class="filter-option" ref="myComboBox">
    <input class="inp" @input="onInput()" v-model="value" />
    <div class="select-arrow" @click="selectOnClick">
      <div class="arrow"></div>
    </div>
  </div>
</template>

<script>
import CommonFn from "../../common/common1";
//import Enumeration from '../../common/enumeration';
export default {
  name: "BaseFilterOption",
  components: {},

  props: {
    filterField: String,
    inputValue: {
      type: Number,
    },
  },
  data() {
    return {
      value: "Tất cả"
    };
  },

  methods: {
    /**
     * Hàm bắt sự kiện bấm vào mũi tên
     * Created By: Ngọc 26/09/2021
     */
    selectOnClick() {
      let width = document.getElementsByClassName("filter-option")[0].offsetWidth;
      let left = this.$refs.myComboBox.getBoundingClientRect().left;
      this.$emit("selectOnClick", this.filterField, left, width);
    },

    setValue() {
      this.value = CommonFn.getValueEnum(this.inputValue, this.filterField);
    },
  },

  watch: {
    inputValue: function () {
      this.setValue();
    },
  },

  created() {
    this.setValue();
  },
};
</script>

<style scoped></style>
