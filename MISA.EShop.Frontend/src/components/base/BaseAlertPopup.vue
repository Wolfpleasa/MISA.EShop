<template>
  <div :class="['alert', subClass, { 'd-none': hideAlertPopup }]">
    <div class="head">
      <div class="head-text">MISA eShop</div>
      <div @click="closeAlertPopup" class="head-close"></div>
    </div>
    <div class="alert-message">
      <div class="alert-icon"></div>
      <div class="alert-text" v-html="alertMessageText"></div>
    </div>
    <div class="alert-close">
      <ButtonIcon
        tabindex="0"
        @btn-click="closeAlertPopup"
        buttonText="Đồng ý"
        ref="myButton"
      />
    </div>
  </div>
</template>

<script>
import ButtonIcon from "../../components/base/BaseButtonIcon.vue";
import { eventBus } from "../../main.js";

export default {
  name: "BaseAlertPopup",
  components: {
    ButtonIcon,
  },

  props: {
    subClass: String,
    hideAlertPopup: Boolean,
    alertMessageText: String,
  },

  methods: {
    /**
     * Hàm đóng alert-message
     * Created By: Ngọc 29/08/2021
     */

    closeAlertPopup() {
      eventBus.$emit("closeAlertPopup");
    },
  },

  watch: {
    // hideAlertPopup: function() {
    //   this.$ref.myButton.focus();
    // },
  },

  created() {},
};
</script>

<style></style>
