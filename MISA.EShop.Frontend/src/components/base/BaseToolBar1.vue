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
      @btn-click="btnDupicateOnClick"
    />
    <ButtonIcon
      iconName="icon-edit"
      buttonText="Sửa"
      @btn-click="btnEditOnClick"
    />
    <ButtonIcon
      iconName="icon-delete"
      buttonText="Xóa"
      @btn-click="btnDeleteOnClick"
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
    id: String,
    subClass: String,
    iconName: String,
    buttonText: String,
    tabindex: String,
    autoFocus: String,
    reFocus: Boolean,
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
    btnDupicateOnClick() {
      this.$emit("btnDupicateOnClick");
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
    reFocus: function () {
      if (this.autoFocus == "true") {
        this.$refs.btnREF.focus();
      }
    },
  },
};
</script>

<style scoped>
</style>
