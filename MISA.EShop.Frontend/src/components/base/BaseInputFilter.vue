<template>
  <div class="input-filter">
    <input
      class="input-filter"
      type="text"
      placeholder="Tìm theo mã, Tên hoặc Số điện thoại"
      v-model="valueFinding"
      @input="startFinding"
    />
  </div>
</template>

<script>
export default {
  name: "BaseFieldInputIcon",

  props: {
    searchContent: String,
  },

  data() {
    return {
      isHide: true,
      valueFinding: "",
    };
  },

  methods: {
    /**
     * Hàm bắt sự kiện nhập vào ô tìm kiém
     * Ngọc 14/8/2021
     */
    startFinding() {
      let me = this;
      me.isHide = false;

      if (me.valueFinding == "") {
        me.clearFinding();
      }
      
      me.$emit("input", me.valueFinding);
    },


  },

  watch: {
    searchContent: function () {
      this.valueFinding = this.searchContent;
    },
  },
};
</script>

<style></style>
