<template>
  <div :class="['tooltip' ,{'d-none': hideToolTip}]">
    <div class="tooltip-text" v-html="toolTipText"></div>
  </div>
</template>

<script>

export default {
  name: "BaseToolTip",

  props: {
    toolTipText: String,
    hideToolTip: Boolean,
  },

  methods: {
  },

  watch: {
  
  },
};
</script>

<style scoped>

</style>