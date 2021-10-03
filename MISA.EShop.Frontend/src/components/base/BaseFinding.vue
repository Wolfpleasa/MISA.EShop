<template>
  <div class="finding" ref="myOperator">
    <div class="operator" @click="operatorOnClick">{{ inputOperator }}</div>
  </div>
</template>

<script>
export default {
  name: "BaseFinding",
  components: {},

  props: {
    filterField:String,
    inputOperator:{
      type: String,
      default: '*',
    },
  },
  data() {
    return {
      items: [],
      currentId: 0,
      currentIndex: 0,
    };
  },

  methods: {
    /**
     * Sự kiện chọn 1 toán tử
     * Created By: Ngọc 26/09/2021
     */
    setValue() {

    },

    /**
     * Hàm bắt sự kiện bấm vào ô chọn
     * Created By: Ngọc 26/09/2021
     */
    operatorOnClick() {
      let left = this.$refs.myOperator.getBoundingClientRect().left;
      this.$emit("operatorOnClick", "OperatorName", this.filterField,  left);
    },
  },
  created() {
    this.setValue(this.index);
  },

  watch: {
  
  },
};
</script>

<style scoped></style>
