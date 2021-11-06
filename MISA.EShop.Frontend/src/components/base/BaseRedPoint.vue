<template>
  <div :class="['red-point' ,{'d-none': hideRedPoint}]">
      <div class="red-point-field" v-html="redPointText"></div>
  </div>
</template>

<script>
export default {
  name: "BaseRedPoint",

  props: {
    hideRedPoint: Boolean,
    redPointText: {
      type: String,
      default: "",
    },
  },

  methods: {
  },

  watch: {
    
  },

  mounted() {
  
  },
};
</script>

<style scoped>

</style>