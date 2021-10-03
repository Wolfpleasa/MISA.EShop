<template>
  <div
    :id="idPopup"
    :class="[' z-index-12', { 'd-none': dnone }]"
  > 
    <div class="head">
      <div class="head-text">{{warning}}</div>
      <div @click="btnCancelOnClick" class="head-close"> </div>
    </div>
    <div class="main">
      <div class="warning"></div>
      <div class="text" v-html="warningText"></div>
    </div>
    <div class="foot">
      <ButtonIcon
        iconName="icon-cancel"
        buttonText="Hủy bỏ"
        subClass="cancel"
        @btn-click="btnCancelOnClick"
      />
         <ButtonIcon
        iconName="icon-delete"
        buttonText="Xóa"
        subClass="confirm"
        @btn-click="btnConfirmOnClick"
      />
    </div>
  </div>
</template>

<script>
  import ButtonIcon from "../base/BaseButtonIcon.vue";
export default {
  name: "BasePopup",
  components: {
    ButtonIcon,
  },

  props: {
    dnone: {
      type: Boolean,
      default: true,
      require: true,
    },
    warning: String,
    warningText: String,
    idPopup: String,
    btnCancelText: String,
    btnConfirmText: String,
    subClass: String,
  },

  data() {
    return {
      product: {},
      reFocus: false,
    };
  },
  methods: {
    /**
     * Hàm đóng popup
     * Created By: Ngọc 28/09/2021
     */
    btnCancelOnClick() {
      let me = this;
      me.$emit("btnCancelOnClick");
    },
    /**
     * Hàm bấm nút xác nhận
     * Created By: Ngọc 28/09/2021
     */
    btnConfirmOnClick() {
      this.$emit("btnConfirmOnClick" , this.idPopup);
    },
  },

  watch: {
    dnone: function() {
      this.reFocus = !this.reFocus;
    },
  },
};
</script>

<style scoped></style>
