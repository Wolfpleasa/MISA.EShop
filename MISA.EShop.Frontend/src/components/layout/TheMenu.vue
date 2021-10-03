<template>
  <div class="menu"> 
    <div class="menu-item">
      <div class="menu-item-icon inventory"></div>
      <div class="menu-item-text">Hàng hóa</div>
    </div>   

    <!-- <router-link to="./customer">
      <div :class="['menu-item' ,{'selected': isFocusCustomer}]">
        <div class="menu-item-icon employee"></div>
        <div :class="['menu-item-text', { 'v-hidden': !isToggle }]">
          Danh mục khách hàng
        </div>
      </div>
    </router-link> -->
  </div>
</template>

<script>
export default {
  name: "Menu",
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Roboto:wght@400&display=swap');
@import url('../../css/layout/menu.css');
</style>
