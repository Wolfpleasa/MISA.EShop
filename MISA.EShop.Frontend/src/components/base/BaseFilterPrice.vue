<template>
  <div class="finding" ref="myOperator">
    <div class="operator" @click="operatorOnClick">{{ inputOperator }}</div>
  </div>
</template>

<script>
export default {
  name: "BaseFilterPrice",
  components: {},

  props: {
    filterField:String,
    inputOperator:{
      type: String,
      default: '=',
    },
  },
  data() {
    return {
      
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
  },  

  watch: {
  
  },
};
</script>

<style scoped></style>
