<template>
  <div
    :tabindex="tabindex"
    @click="$emit('btn-click')"
    :class="['button-icon', subClass]"
    v-on:keyup.enter="$emit('btn-click')"
    v-on:keydown="keydownOnSelect($event)"
  >
    <div :class="['icon',iconName]"></div>
    <div class="button-text">{{ buttonText }}</div>
    <div v-if="iconName == 'icon-add'" class="btn-drop"></div>
  </div>
</template>

<script>
export default {
  name: "BaseButtonIcon",

  props: {
    subClass: String,
    iconName: String,
    buttonText: String,
    tabindex: String,
  },

  methods: {
     /**
     * Hàm ngăn chặn sự kiện mặc định của nút tab, dùng thì mới thực hiện được nút enter
     * Ngọc 4/8/2021
     */
    keydownOnSelect(event) {
      if (event.Code == "Tab") {
        event.preventDefault();
      }
    },
  },
};
</script>

<style scoped>
</style>
