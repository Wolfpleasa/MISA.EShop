<template>
   <div :class="['loader' , {'d-none' : hideLoader}]">
       
    </div>
</template>

<script>
export default {
  name: "BaseLoader",

  props: {
    hideLoader: Boolean
  },

  methods: {
 
  },

  watch: {
   
  },
};
</script>

<style scoped>

</style>
