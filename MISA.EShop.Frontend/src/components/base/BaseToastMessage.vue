<template>
  <slide-up-down :active="active" :duration="500">
    <div :class="['toast', , subClass, { 'd-none': hideToastMessage }]">
      <div class="toast-message">
        <div class="d-flex">
          <div class="toast-icon"></div>
          <div class="toast-text">{{ toastMessageText }}</div>
        </div>
        <div class="toast-close" @click="$emit('closeToastMessage')"></div>
      </div>
    </div>
  </slide-up-down>
</template>

<script>
export default {
  name: "BaseToastMessage",
  props: {
    subClass: String,
    hideToastMessage: Boolean,
    toastMessageText: String,
    active: Boolean,
  },
  
  watch: {
    hideToastMessage: function () {
      setTimeout(() => {
        this.$emit("closeToastMessage");
      }, 3000);
    },
  },
};
</script>

<style></style>