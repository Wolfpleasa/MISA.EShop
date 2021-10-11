<template>
  <div class="finding" ref="myOperator">
    <div class="operator" @click="operatorOnClick">{{ value }}</div>
  </div>
</template>

<script>
export default {
  name: "BaseFilterPrice",
  components: {},

  props: {
    filterField:String,
    inputOperator:{
      type: Number,
    },
  },
  data() {
    return {
      value: "=",
    };
  },

  methods: {
    /**
     * Hàm bắt sự kiện bấm vào ô chọn
     * Created By: Ngọc 26/09/2021
     */
    operatorOnClick() {
      let left = this.$refs.myOperator.getBoundingClientRect().left;
      this.$emit("operatorOnClick", "FilterPriceName", this.filterField,  left);
    },

    /**
     * Hàm hiển thị dữ liệu cho ô chọn toán tử
     * Created By: Ngọc 26/09/2021
     */
    displayOperator(){
      switch (this.inputOperator) {
        case 0:
          this.value = "=";
          break;
        case 1:
          this.value = "<";
          break;
        case 2:
          this.value = "≤";
          break;
        case 3:
          this.value = ">";
          break;
        case 4:
          this.value = "≥";
          break;
      }
    }
  },  

  watch: {
     inputOperator: function () {
      this.displayOperator();
    },
  },

  created() {
    this.displayOperator();
  },
};
</script>

<style scoped></style>
