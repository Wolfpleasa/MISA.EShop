<template>
  <div :class="['tooltip' ,{'d-none': hideToolTip}]">
       
  </div>
</template>

<script>
export default {
  name: "BaseToolTip",

  props: {
    ToolTipText: String,
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