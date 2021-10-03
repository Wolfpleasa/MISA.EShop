<template>
  <div :class="['checkbox', {'checked': checked}]" @click ="$emit('clickCheckBox')" >
      
  </div>
</template>

<script>
export default {
  name: "BaseCheckBox",
  props: {
      checked: Boolean,
  },
};
</script>

<style></style>