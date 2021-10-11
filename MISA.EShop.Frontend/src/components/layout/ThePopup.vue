<template>
  <vue-resizable
    :fitParent="fitParent"
    dragSelector=".head"
    :minWidth="minW"
    :minHeight="minH"
    :width="w"
    :height="h"
    :maxWidth="maxW"
    :maxHeight="maxH"
    :top="top"
    :left="left"
    :id="idPopup"
    :class="[' z-index-12', { 'd-none': dnone }]"
  >
    <div class="head">
      <div class="head-text">{{ warning }}</div>
      <div @click="btnCancelOnClick" class="head-close"></div>
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
  </vue-resizable>
</template>

<script>
import VueResizable from "vue-resizable";

import ButtonIcon from "../base/BaseButtonIcon.vue";
export default {
  name: "BasePopup",
  components: {
    ButtonIcon,
    VueResizable,
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
      top: 0,
      left: 0,
      minW: 400,
      minH: 163,
      w: 400,
      h: 163,
      maxW: 500,
      maxH: 263,
      fitParent: true,
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
      this.$emit("btnConfirmOnClick", this.idPopup);
    },

    /**
     * Hàm để hàm luôn xuất hiện ở giữa
     * Created By: Ngọc 28/09/2021
     */
    getPosition() {
      let bodyHeight = document.body.clientHeight;
      let bodyWidth = document.body.clientWidth;
      console.log(bodyHeight, bodyWidth)

      this.top = (bodyHeight - 192) / 2;
      this.left = (bodyWidth - 400) / 2;
      console.log(this.top, this.left)
    },
  },

  watch: {
    dnone: function () {
      this.reFocus = !this.reFocus;
    },
  },

  updated() {
    this.getPosition();
  },
};
</script>

<style scoped></style>
