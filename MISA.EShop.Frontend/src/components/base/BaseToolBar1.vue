<template>
  <div class="toolbar">
    <ButtonIcon
      iconName="icon-add"
      buttonText="Thêm mới"
      @btn-click="btnAddOnClick"
    />
    <ButtonIcon
      iconName="icon-duplicate"
      buttonText="Nhân bản"
      @btn-click="btnDuplicateOnClick"
      :subClass = "disable"
    />
    <ButtonIcon
      iconName="icon-edit"
      buttonText="Sửa"
      @btn-click="btnEditOnClick"
       :subClass = "disable"
    />
    <ButtonIcon
      iconName="icon-delete"
      buttonText="Xóa"
      @btn-click="btnDeleteOnClick"
      :subClass = "disable"
    />
    <ButtonIcon
      iconName="icon-refresh"
      buttonText="Nạp"
      subClass="border-2"
      @btn-click="refreshOnClick"
    />
  </div>
</template>

<script>
import ButtonIcon from "./BaseButtonIcon.vue";
export default {
  name: "BaseToolBar",

  components: {
    ButtonIcon,
  },

  props: {
    productId: String,
  },

  data(){
    return {
      disable: "",
      subclass: "",
    }
  },

  methods: {

    /**
     * Hàm bấm nút Thêm mới
     * Created By: Ngọc 22/9/2021
     */
    btnAddOnClick() {
      this.$emit("btnAddOnClick");
    },

    /**
     * Hàm bấm nút Nhân bản
     * Created By: Ngọc 22/9/2021
     */
    btnDuplicateOnClick() {
      this.$emit("btnDuplicateOnClick");
    },

    /**
     * Hàm bấm nút Sửa
     * Created By: Ngọc 22/9/2021
     */
    btnEditOnClick() {
      this.$emit("btnEditOnClick");
    },

    /**
     * Hàm bấm nút Xóa
     * Created By: Ngọc 22/9/2021
     */
    btnDeleteOnClick() {
      this.$emit("btnDeleteOnClick");
    },

    /**
     * Hàm bấm nút nạp
     * Created By: Ngọc 22/9/2021
     */
    refreshOnClick() {
      this.$emit("refreshOnClick");
    },
  },

  watch: {
    productId: function () {
      if (this.productId == null) {
        this.disable = "disable";
      }else{
        this.disable = "";
      }
    },
  },
};
</script>

<style scoped>
</style>
