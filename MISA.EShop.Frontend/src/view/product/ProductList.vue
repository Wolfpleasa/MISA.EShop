<template>
  <div>
    <Header :mode="mode" :showFormDetail="!DialogHasDnone" />
    <Menu />
    <div class="content">
      <ToolBar
        @btnAddOnClick="btnAddOnClick"
        @btnDuplicateOnClick="btnDuplicateOnClick"
        @btnEditOnClick="btnEditOnClick"
        @btnDeleteOnClick="btnDeleteOnClick"
        @refreshOnClick="refreshOnClick"
      />

      <Table @getId="getId" :response="response" />
    </div>

    <ProductDetail
      v-bind:dnone="DialogHasDnone"
      :productId="productId"
      :formMode="formMode"
      :reopen="reopen"
      @btnDialogCancelOnClick="btnDialogCancelOnClick"
      @btnSaveOnClick="btnSaveOnClick"
      @resetAfterSaveData="resetAfterSaveData"
      @callToastMessage="callToastMessage"
    />

    <!-- 3 page làm mờ -->
    <!--  của warningpopup -->
    <div
      :class="['pageCover z-index-11', { 'd-none': WarningHasDnone }]"
    ></div>
   
    <Loader :HideLoader="HideLoader" />

    <WarningPopup
      :idPopup="idPopup"
      :dnone="WarningHasDnone"
      :warningText="warningText"
      :warning="warning"
      :subClass="bonusClass"
      @btnCancelOnClick="btnCancelOnClick"
      @btnConfirmOnClick="btnConfirmOnClick"
    />

    <ToastMessage
      :subClass="subClass"
      :HideToastMessage="HideToastMessage"
      :ToastMessageText="ToastMessageText"
      @closeToastMessage="closeToastMessage"
    />
  </div>
</template>

<script>
import axios from "axios";
import Constant from "../../common/constant1.js";
import Enumeration from "../../common/enumeration.js";
import ResourceVN from '../../common/resourceVN.js';

import Header from "../../components/layout/TheHeader.vue";
import Menu from "../../components/layout/TheMenu.vue";
import ToolBar from "../../components/base/BaseToolBar1.vue";
import Table from "../../components/base/BaseTable.vue";

import ToastMessage from "../../components/base/BaseToastMessage.vue";

import Loader from "../../components/base/BaseLoader.vue";

import ProductDetail from "./ProductDetail.vue";
import WarningPopup from "../../components/layout/ThePopup.vue";
export default {
  name: "ProductList",
  components: {
    Header,
    Menu,
    ToolBar,
    Loader,
    ToastMessage,
    ProductDetail,
    WarningPopup,
    Table,
  },
  data() {
    return {
      //ProductList
      product: {},
      mode: "",

      //ToastMessage
      HideToastMessage: true,
      ToastMessageText: "",
      //Loader
      HideLoader: true,

      //ProductDetail
      DialogHasDnone: true,
      productId: null,
      parentId: null,
      reopen: true,
      formMode: -1,
      subClass: "",

      //--------------- form cảnh báo/thông báo ------------------
      WarningHasDnone: true,
      warning: "",
      warningText: "",
      idPopup: "",
      btnCancelText: "",
      btnConfirmText: "",
      notifyMode: -1,
      response: "",
      bonusClass: "",

      // -------------------- Phân trang ----------------------
      // Tổng số hàng hóa
      totalEntity: 0,
      // Tổng số trang
      totalPageNumber: 1,
      // Trang hiện tại
      currentPageNumber: 1,
      // Số bản ghi mỗi trang dự kiến
      entityPerPage: 5,
      // Số bản ghi thực tế mỗi trang
      realEntityPerPage: 1,

      // ------------ Tìm kiếm ------------
      // Nội dung tìm kiếm
      searchContent: "",
    };
  },

  methods: {
    /**
     * Hàm lấy id của hàng đang được chọn
     * Created By: Ngọc 28/09/2021
     */
    getId(productId, parentId) {
      this.productId = productId;
      this.parentId = parentId;
    },

    /**
     * Hàm Ẩn/hiện Loader
     * Ngọc 12/8/2021
     */
    refreshOnClick() {
      this.HideLoader = false;
      setTimeout(() => {
        this.HideLoader = true;
      }, 1000);
    },

    /**
     * Hàm mở popup
     * Created By: Ngọc 25/09/2021
     */
    btnAddOnClick() {
      let me = this;
      me.DialogHasDnone = false;
      this.formMode = Enumeration.FormMode.Add;
      this.mode = "Thêm mới";
    },

    /**
     * Hàm mở form detail để sửa
     * Created By: Ngọc 25/09/2021
     */
    btnEditOnClick() {
      this.DialogHasDnone = false;
      this.formMode = Enumeration.FormMode.Edit;
      this.mode = "Sửa";
    },

    /**
     * Hàm bấm checkbox ở td được BaseCheckBox gửi lên
     * Ngọc 1/8/2021
     */
    clickCheckboxTd(index) {
      let me = this;
      me.isSelected[index] = !me.isSelected[index];
      me.HideBtnDelete = false;
      me.HideBtnDuplicate = false;
      setTimeout(function () {
        me.checked = me.CheckAllCBTd();
      }, 10);
    },

    /**
     * Hàm kiểm tra tất cả checkbox của td có được chọn không
     * Ngọc 1/8/2021
     */
    CheckAllCBTd() {
      let me = true;
      for (var i = 0; i < this.isSelected.length; i++) {
        if (!this.isSelected[i]) {
          me = false;
          break;
        }
      }
      return me;
    },

    /**
     * Hàm bấm checkbox ở th được BaseCheckBox gửi lên
     * Ngọc 1/8/2021
     */
    clickCheckboxTh() {
      this.checked = !this.checked;
      if (this.checked) {
        this.isSelected.fill(true);
        this.HideBtnDelete = false;
        this.HideBtnDuplicate = false;
      } else {
        this.isSelected.fill(false);
        this.HideBtnDelete = true;
        this.HideBtnDuplicate = true;
      }
    },

    /**
     * Hàm bấm nút xóa
     * Ngọc 30/07/2021
     */
    async btnDeleteOnClick() {
      let me = this;
      await axios
        .get(`${Constant.LocalUrl}/Products/${me.productId}`)
        .then((res) => {
         res.data.forEach(data => {
           if(data.ProductId == me.productId){
             Object.assign(me.product, data);
           }
         });
        })
        .catch((err) => {
          console.log(err);
        });
      me.warningText = `${ResourceVN.CONFIRM_DELETE} <span>${me.product.SKUCode}-${me.product.ProductName}</span> ${ResourceVN.CONFIRM}`;
      me.warning = `Xóa dữ liệu`;

      me.WarningHasDnone = false;
      me.idPopup = "warning-popup";
    },

    /**
     * Hàm bấm nút nhân bản
     * Ngọc 30/07/2021
     */
    btnDuplicateOnClick() {
      let me = this;
      var count = 0;
      this.isSelected.forEach((selected) => {
        if (selected) {
          count = count + 1;
        }
      });
      if (count > 1) {
        me.warning = `Nhân bản thông tin của ${count} hàng hóa`;
        me.warningText = `Xác nhận nhân bản thông tin của <b>${count} hàng hóa </b> này không`;
      } else {
        let fullname = "";
        this.isSelected.forEach((selected, index) => {
          if (selected) {
            fullname = me.products[index].FullName;
          }
        });
        me.warning = `Nhân bản thông tin của nhân  viên ${fullname}`;
        me.warningText = `Xác nhận nhân bản thông tin của hàng hóa <b>${fullname} </b>`;
      }

      me.WarningHasDnone = false;
      me.idPopup = "notify-popup";
      me.btnCancelText = "Hủy";
      me.bonusClass = "w-100";
      me.btnConfirmText = "Lưu";
      me.notifyMode = 2;
    },

    /**
     * Hàm bấm nút xác nhận
     * Ngọc 30/07/2021
     */
    async btnConfirmOnClick(idPopup) {
      let me = this;
      // Nếu form cảnh báo xóa được gọi
      me.WarningHasDnone = true;
      if (idPopup == "warning-popup") {
        await axios
          .delete(`${Constant.LocalUrl}/Products/${me.productId}`)
          .then(() => {
            me.callToastMessage("Xóa dữ liệu thành công", "message-green");
            me.isSelected = [];
          })
          .catch(() => {
            me.callToastMessage(
              "Có vấn đề xảy ra, không thể xóa dữ liệu",
              "message-red"
            );
          });
        let responseTime = Number(new Date());
        me.response = `${responseTime}`;
      }

      me.idPopup = "";
    },

    /**
     * Hàm lấy mã hàng hóa mới
     * Ngọc 6/8/2021
     */
    getNewCode() {
      return new Promise((resolve) => {
        axios
          .get(`${Constant.LocalUrl}Products/NewProductCode`)
          .then((res) => {
            console.log(res.data, Number(new Date()));
            resolve(res.data);
          })
          .catch((err) => {
            console.log(err);
          });
      });
    },

    /**
     * Hàm lấy hàng hóa theo id hàng hóa
     * Ngọc 6/8/2021
     */
    getInfo(productId) {
      return new Promise((resolve) => {
        axios
          .get(`${Constant.LocalUrl}Products/${productId}`)
          .then((res) => {
            let newProduct = res.data;
            console.log(newProduct.FullName, Number(new Date()));
            resolve(newProduct);
          })
          .catch((err) => {
            console.log(err);
          });
      });
    },

    /**
     * Thêm hàng hóa được nhân bản
     * Ngọc 6/8/2021
     */
    InsertDuplicate(productDuplicated) {
      return new Promise((resolve) => {
        axios
          .post(`${Constant.LocalUrl}Products/`, productDuplicated)
          .then(() => {
            resolve(1);
          })
          .catch((err) => {
            console.log(err);
          });
      });
    },

    /**
     * Hàm được gọi khi thay đổi page hoặc số lượng hàng hóa/trang
     * Ngọc 12/8/2021
     */
    updatePage(currentPageNumber) {
      let me = this;
      me.currentPageNumber = currentPageNumber;
      me.refreshOnClick();
    },

    /**
     * Hàm đóng popup
     * Ngọc 29/07/2021
     */
    btnCancelOnClick() {
      let me = this;
      me.WarningHasDnone = true;
    },

    /**
     * Hàm bấm hủy bỏ ở form detail
     * Created By: Ngọc 22/09/2021
     */
    btnDialogCancelOnClick() {
      let me = this;
      me.DialogHasDnone = true;
      me.mode = "";
      me.formMode = Enumeration.FormMode.None;
    },

    /**
     * Hàm thực hiện những reset sau khi lưu
     * Ngọc 5/8/2021
     */
    resetAfterSaveData() {
      let me = this;
      //reset formMode
      me.formMode = Enumeration.FormMode.None;
      //Đống form thêm/sửa
      me.DialogHasDnone = true;
      //Đóng popup
      me.WarningHasDnone = true;
      let responseTime = Number(new Date());
      me.response = `${responseTime}`;
    },

    /**
     * Hàm bấm lưu được ProductDetail gửi lên
     * Ngọc 30/07/2021
     */
    btnSaveOnClick(productFullName) {
      let me = this,
        custom = "";
      if (this.formMode == 0) {
        custom = "thêm";
      } else {
        custom = "sửa";
      }
      //Hiển thị popup thông báo lưu
      me.WarningHasDnone = false;
      me.idPopup = "notify-popup";
      me.warning = "Lưu thông tin hàng hóa ";
      me.warningText = `Xác nhận ${custom} hàng hóa <b>${productFullName}</b> `;
      me.btnCancelText = "Hủy";
      me.btnConfirmText = "Lưu";
      me.bonusClass = "w-100";
      me.notifyMode = 0;
    },

    /**
     * Hàm hiển thị toastmessage
     * Ngọc 2/8/2021
     */
    callToastMessage(text, subClass) {
      this.HideToastMessage = false;
      this.ToastMessageText = text;
      this.subClass = subClass;
    },

    /**
     * Hàm đóng toastmessage
     * Ngọc 2/8/2021
     */
    closeToastMessage() {
      this.HideToastMessage = true;
      this.ToastMessageText = "";
      this.subClass = "";
    },

    /**
     * Hàm chuyển đổi menu do <Menu/> gửi lên
     * Ngọc 3/8/2021
     */
    toggleMenu() {
      this.isToggle = !this.isToggle;
    },
  },

  created() {},

  watch: {},

  mounted() {},
};
</script>

<style>
</style>
