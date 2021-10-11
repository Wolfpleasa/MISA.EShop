<template>
  <div :class="['red-point' ,{'d-none': hideRedPoint}]">
      <div class="red-point-field">Vui lòng điền vào trường này.</div>
  </div>
</template>

<script>
export default {
  name: "BaseRedPoint",

  props: {
    hideRedPoint: Boolean,
  },

  methods: {
  },

  watch: {
  
  },
};
</script>

<style scoped>

</style>