<template>
  <div class="header">
            <div class="header-left"></div>
            <div class="header-right">
                <div :class="['product' , {'form-mode' : showFormDetail}]">
                    <div class="product-name">Hàng hóa</div>
                    <div v-if ="mode.length>0"  class = "fs-18">/</div>
                    <div v-if ="mode.length>0" class="mode">{{mode}}</div>
                </div>
                <div class="user">
                    <div class="back">
                        <div class="back-arrow"></div>
                        <div class="back-text">Quay lại trang bắt đầu sử dụng</div>               
                    </div>
                    <div class="brief">
                        <div class="brief-name">mnd</div>
                        <div class="brief-arrow"></div>
                    </div>
                    <div class="d-flex">
                        <div class="user-avatar"></div>
                        <div class="user-fullname">Đỗ Minh Ngọc</div>
                        <div class="user-arrow"></div>
                        <div class="phone"></div>
                        <div class="bell"></div>
                        <div class="help"></div>
                    </div>             
            
                </div>
            </div>
        </div>
</template>

<script>
export default {
    name: 'Header',
    props: {
        showFormDetail: {
            type: Boolean,
            default: false,
        },
        mode: String,
    },
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    @import url("../../css/layout/header.css")
</style>
