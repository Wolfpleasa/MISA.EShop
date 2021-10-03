<template>
  <div class="filter-option" ref="myComboBox">
      <input
        class="inp"
        @input="onInput()"
        v-model = "inputValue"
      />
      <div class="select-arrow" @click="selectOnClick">
        <div class="arrow"></div>
      </div>
  </div>
</template>

<script>
export default {
  name: "BaseFilterPrice",
  components: {},

  props: {
    filterField:String,
    inputValue:{
      type: String,
      default: 'Tất cả',
    },
  },
  data() {
    return {
      
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
  },  

  watch: {
  
  },
};
</script>

<style scoped></style>
